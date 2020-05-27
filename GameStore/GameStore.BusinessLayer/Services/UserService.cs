using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;
        private readonly ICommentService _commentService;
        private readonly IOrderService _orderService;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<User> _userRepository;

        public UserService(
            IUnitOfWork unitOfWork,
            IRoleService roleService,
            ICommentService commentService,
            IOrderService orderService,
            IPublisherService publisherService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _commentService = commentService;
            _orderService = orderService;
            _publisherService = publisherService;
            _mapper = mapper;
            _userRepository = _unitOfWork.GetRepository<IAsyncRepository<User>>();
        }

        public async Task<string> CreateGuestAsync()
        {
            var guestRole = await _roleService.GetByNameAsync(DefaultRoles.Guest);
            var userRoles = new[]
            {
                new UserRole
                {
                    RoleId = guestRole.Id
                }
            };
            var user = new User
            {
                UserRoles = userRoles
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user.Id;
        }

        public async Task CreateAsync(UserDto userDto, string password)
        {
            if (userDto == null)
            {
                throw new InvalidServiceOperationException("Is null user dto");
            }

            ThrowIfNotValidPassword(password);
            var user = await CreateUserAsync(userDto, password);
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task MergeUserActionsAsync(string guestId, string userId)
        {
            var guest = await GetByIdAsync(guestId);

            if (guest == null)
            {
                return;
            }

            await _commentService.UpdateCommentsOwnerAsync(guestId, userId);
            await _orderService.MergeOrdersAsync(guestId, userId);
            await _userRepository.DeleteAsync(guestId);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRolesAsync(string userId, IEnumerable<string> roles)
        {
            var user = await _userRepository.FindSingleAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException<User>();
            }

            user.UserRoles = roles?.Select(role => new UserRole
            {
                UserId = userId,
                RoleId = role
            }).ToList();

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<string> TrySignInAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidServiceOperationException("Is empty password");
            }

            var hash = GetPasswordHash(password);
            var user = await _userRepository.FindSingleAsync(u => u.Email == email && hash == u.PasswordHash);

            return user?.Id;
        }

        public async Task AssignToPublisherAsync(string userId, string publisherId)
        {
            var publisherDto = await _publisherService.GetByIdAsync(publisherId);
            var user = await _userRepository.FindSingleAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException<User>(userId);
            }

            await RemoveUserIdInPreviousPublisherAsync(userId);

            publisherDto.UserId = userId;
            await _publisherService.UpdateAsync(publisherDto);

            if (user.UserRoles.Any(r => r.Role?.Name == DefaultRoles.Publisher))
            {
                return;
            }

            var role = await _roleService.GetByNameAsync(DefaultRoles.Publisher);
            user.UserRoles.Add(new UserRole {RoleId = role.Id, UserId = userId});
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task BanAsync(string userId, BanTerm term)
        {
            var timeSpanTerm = term == BanTerm.Permanent ? TimeSpan.MaxValue : TimeSpan.FromMinutes((double) term);
            var user = await _userRepository.FindSingleAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new EntityNotFoundException<User>(userId);
            }

            user.BannedTo = DateTime.UtcNow + timeSpanTerm;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userRepository.FindSingleAsync(u => u.Id == id);
            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.FindAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }

        public UserDto GetDefaultUserDto(string id, string email)
        {
            var userDto = new UserDto
            {
                Id = id,
                Email = email,
                Roles = new[]
                {
                    GetDefaultRole()
                }
            };

            return userDto;
        }

        private static void ThrowIfNotValidPassword(string password)
        {
            const int minPasswordLength = 8;

            if (string.IsNullOrEmpty(password) || password.Length < minPasswordLength)
            {
                throw new InvalidServiceOperationException(PasswordValidationErrors.MinLength);
            }

            if (!password.Any(char.IsDigit))
            {
                throw new InvalidServiceOperationException(PasswordValidationErrors.ShouldContainDigit);
            }

            if (!password.Any(char.IsUpper))
            {
                throw new InvalidServiceOperationException(PasswordValidationErrors.ShouldContainUpperSymbol);
            }

            if (!password.Any(char.IsLower))
            {
                throw new InvalidServiceOperationException(PasswordValidationErrors.ShouldContainLowerSymbol);
            }
        }

        private async Task<User> CreateUserAsync(UserDto userDto, string password)
        {
            var hash = GetPasswordHash(password);
            var roleNames = userDto.Roles.Select(r => r.Name);
            var roles = await _roleService.GetByNamesAsync(roleNames);

            var user = new User
            {
                Id = userDto.Id,
                Email = userDto.Email,
                PasswordHash = hash,
                UserRoles = roles.Select(r => new UserRole
                {
                    RoleId = r.Id,
                    UserId = userDto.Id
                }).ToList()
            };

            return user;
        }

        private static string GetPasswordHash(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = MD5.Create().ComputeHash(passwordBytes);
            var stringRepresentation = new Guid(hash).ToString();

            return stringRepresentation;
        }

        private async Task RemoveUserIdInPreviousPublisherAsync(string userId)
        {
            var oldPublisher = await _publisherService.GetByUserIdAsync(userId);

            if (oldPublisher == null)
            {
                return;
            }
            
            oldPublisher.UserId = default;
            var modifyPublisherDto = _mapper.Map<ModifyPublisherDto>(oldPublisher);
            
            await _publisherService.UpdateAsync(modifyPublisherDto);
        }
        
        private static RoleDto GetDefaultRole()
        {
            var role = new RoleDto
            {
                Name = DefaultRoles.User
            };

            return role;
        }
    }
}
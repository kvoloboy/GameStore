using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private const string GuestId = "1";
        private const string UserId = "2";
        private const string Email = "Email";
        private const string Password = "Password";

        private IUnitOfWork _unitOfWork;
        private IRoleService _roleService;
        private IPublisherService _publisherService;
        private IOrderService _orderService;
        private ICommentService _commentService;
        private IMapper _mapper;
        private IAsyncRepository<User> _userRepository;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _roleService = A.Fake<IRoleService>();
            _publisherService = A.Fake<IPublisherService>();
            _orderService = A.Fake<IOrderService>();
            _commentService = A.Fake<ICommentService>();
            _mapper = A.Fake<IMapper>();
            _userRepository = A.Fake<IAsyncRepository<User>>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<User>>()).Returns(_userRepository);

            _userService = new UserService(_unitOfWork,
                _roleService,
                _commentService,
                _orderService,
                _publisherService,
                _mapper);
        }

        [Test]
        public void CreateGuestAsync_CreatesUserWithGuestRole_Always()
        {
            _userService.CreateGuestAsync();

            A.CallTo(() => _userRepository.AddAsync(A<User>.That.Matches(u => u.UserRoles.Any())))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullUserDto()
        {
            Func<Task> action = async () => await _userService.CreateAsync(null, string.Empty);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null user dto");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenPasswordLengthIsLessThanMinLength()
        {
            var userDto = CreateUserDto();

            Func<Task> action = async () => await _userService.CreateAsync(userDto, string.Empty);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Password should contain at least 8 symbols");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenPasswordDoesNotContainNumber()
        {
            const string password = "aaaaaaaaa";
            var userDto = CreateUserDto();

            Func<Task> action = async () => await _userService.CreateAsync(userDto, password);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage(PasswordValidationErrors.ShouldContainDigit);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenPasswordDoesNotContainLowerCaseSymbol()
        {
            const string password = "AAAAAAAA1";
            var userDto = CreateUserDto();

            Func<Task> action = async () => await _userService.CreateAsync(userDto, password);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage(PasswordValidationErrors.ShouldContainLowerSymbol);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenPasswordDoesNotContainUpperCaseSymbol()
        {
            const string password = "aaaaaaaaa1";
            var userDto = CreateUserDto();

            Func<Task> action = async () => await _userService.CreateAsync(userDto, password);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage(PasswordValidationErrors.ShouldContainUpperSymbol);
        }

        [Test]
        public void CreateAsync_CallsRepository_WhenValidParameters()
        {
            const string password = "aaaaaaaaa1A";
            var userDto = CreateUserDto();

            _userService.CreateAsync(userDto, password);

            A.CallTo(() => _userRepository.UpdateAsync(A<User>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void MergeUserActionsAsync_DoNothing_WhenOldUserNotFound()
        {
            A.CallTo(() => _mapper.Map<UserDto>(A<User>._)).Returns(null);

            _userService.MergeUserActionsAsync(GuestId, UserId);

            A.CallTo(() => _userRepository.DeleteAsync(GuestId)).MustNotHaveHappened();
        }

        [Test]
        public void MergeUserActionsAsync_CallsCommentServiceToUpdateCommentsOwner_WhenOldUserExists()
        {
            _userService.MergeUserActionsAsync(GuestId, UserId);

            A.CallTo(() => _commentService.UpdateCommentsOwnerAsync(GuestId, UserId))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void MergeUserActionsAsync_CallsOrderServiceToMergeOrders_WhenOldUserExists()
        {
            _userService.MergeUserActionsAsync(GuestId, UserId);

            A.CallTo(() => _orderService.MergeOrdersAsync(GuestId, UserId))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void MergeUserActionsAsync_DeletesOldUser_WhenExists()
        {
            _userService.MergeUserActionsAsync(GuestId, UserId);

            A.CallTo(() => _userRepository.DeleteAsync(GuestId)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateRolesAsync_ThrowsException_WhenNotFoundUser()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            Func<Task> action = async () => await _userService.UpdateRolesAsync(UserId, Enumerable.Empty<string>());

            await action.Should().ThrowAsync<EntityNotFoundException<User>>();
        }

        [Test]
        public void UpdateRolesAsync_CallsRepository_WhenFoundUser()
        {
            _userService.UpdateRolesAsync(UserId, Enumerable.Empty<string>());

            A.CallTo(() => _userRepository.UpdateAsync(A<User>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task TrySignInAsync_ThrowsException_WhenEmptyPassword()
        {
            Func<Task> action = async () => await _userService.TrySignInAsync(Email, string.Empty);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void TrySignInAsync_ReturnsEmptyId_WhenNotFoundByEmailAndPassword()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            var id = _userService.TrySignInAsync(Email, Password).Result;

            id.Should().BeNullOrEmpty();
        }

        [Test]
        public void TrySignInAsync_ReturnsTrueAndUserId_WhenFoundByEmailsAndPassword()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns(new User());

            var id = _userService.TrySignInAsync(Email, Password);

            id.Should().NotBeNull();
        }

        [Test]
        public async Task AssignToPublisherAsync_ThrowsException_WhenUserNotFound()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            Func<Task> action = async () => await _userService.AssignToPublisherAsync(UserId, UserId);

            await action.Should().ThrowAsync<EntityNotFoundException<User>>();
        }

        [Test]
        public void AssignToPublisherAsync_SetsUserIdToPublisher_WhenFoundPublisher()
        {
            var user = CreateUser();
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns(user);

            _userService.AssignToPublisherAsync(UserId, UserId);

            A.CallTo(() => _publisherService.UpdateAsync(A<ModifyPublisherDto>.That.Matches(p => p.UserId == UserId)))
                .MustHaveHappenedOnceExactly();
        }


        [Test]
        public void AssignToPublisherAsync_UpdatesUser_WhenUserExists()
        {
            var user = CreateUser();
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns(user);

            _userService.AssignToPublisherAsync(UserId, UserId);

            A.CallTo(() => _userRepository.UpdateAsync(A<User>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task BanAsync_ThrowsException_WhenNotExist()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            Func<Task> action = async () => await _userService.BanAsync(UserId, BanTerm.Day);

            await action.Should().ThrowAsync<EntityNotFoundException<User>>();
        }

        [Test]
        public void BanAsync_UpdatesUser_WhenExist()
        {
            _userService.BanAsync(UserId, BanTerm.Day);

            A.CallTo(() => _userRepository.UpdateAsync(A<User>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByIdAsync_CallsRepository_Always()
        {
            _userService.GetByIdAsync(UserId);

            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_CallsRepository_Always()
        {
            _userService.GetAllAsync();

            A.CallTo(() => _userRepository.FindAllAsync(A<Expression<Func<User, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        private static UserDto CreateUserDto()
        {
            var userDto = new UserDto
            {
                Roles = new List<RoleDto>()
            };

            return userDto;
        }

        private static User CreateUser()
        {
            var user = new User
            {
                UserRoles = new List<UserRole>()
            };

            return user;
        }
    }
}
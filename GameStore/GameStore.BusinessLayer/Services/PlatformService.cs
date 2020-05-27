using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Platform> _platformRepository;

        public PlatformService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _platformRepository = unitOfWork.GetRepository<IAsyncRepository<Platform>>();
        }

        public async Task CreateAsync(PlatformDto platformDto)
        {
            if (platformDto == null)
            {
                throw new InvalidServiceOperationException("Is null platform dto");
            }

            await ValidatePlatformExistingByNameAsync(platformDto.Name);

            var platform = _mapper.Map<Platform>(platformDto);
            await _platformRepository.AddAsync(platform);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PlatformDto platformDto)
        {
            if (platformDto == null)
            {
                throw new InvalidServiceOperationException("Is null platform dto");
            }

            await ValidatePlatformNameAsync(platformDto.Id, platformDto.Name);

            var platform = _mapper.Map<Platform>(platformDto);
            await _platformRepository.UpdateAsync(platform);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exists = await _platformRepository.AnyAsync(g => g.Id == id);

            if (!exists)
            {
                throw new EntityNotFoundException<Genre>(id);
            }

            await _platformRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<PlatformDto>> GetAllAsync()
        {
            var platforms = await _platformRepository.FindAllAsync();
            var platformsDto = _mapper.Map<IEnumerable<PlatformDto>>(platforms);

            return platformsDto;
        }

        public async Task<PlatformDto> GetByIdAsync(string id)
        {
            var existingPlatform = await _platformRepository.FindSingleAsync(p => p.Id == id);

            if (existingPlatform == null)
            {
                throw new EntityNotFoundException<Platform>(id);
            }

            var dto = _mapper.Map<PlatformDto>(existingPlatform);

            return dto;
        }
        
        private async Task ValidatePlatformNameAsync(string id, string name)
        {
            var existingPlatformDto = await GetByIdAsync(id);
            var isChangedName = name != existingPlatformDto.Name;

            if (isChangedName)
            {
                await ValidatePlatformExistingByNameAsync(name);
            }
        }

        private async Task ValidatePlatformExistingByNameAsync(string name)
        {
            var alreadyExist = await _platformRepository.AnyAsync(g => g.Name == name);

            if (alreadyExist)
            {
                throw new EntityExistsWithKeyValueException<Platform>(nameof(Platform.Name), name);
            }
        }
    }
}
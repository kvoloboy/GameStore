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
    public class GameImageService : IGameImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<GameImage> _gameImageRepository;

        public GameImageService(IUnitOfWork unitOfWork, IGameService gameService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
            _mapper = mapper;
            _gameImageRepository = _unitOfWork.GetRepository<IAsyncRepository<GameImage>>();
        }

        public async Task<IEnumerable<GameImageDto>> GetByGameKeyAsync(string gameKey)
        {
            if (string.IsNullOrEmpty(gameKey))
            {
                throw new InvalidServiceOperationException("Is empty game key");
            }

            var images = await _gameImageRepository.FindAllAsync(image => image.GameRoot.Key == gameKey);
            var imagesDto = _mapper.Map<IEnumerable<GameImageDto>>(images).ToList();

            return imagesDto;
        }

        public async Task<GameImageDto> GetByIdAsync(string id)
        {
            var image = await _gameImageRepository.FindSingleAsync(gameImage => gameImage.Id == id);

            if (image == null)
            {
                throw new EntityNotFoundException<GameImage>(id);
            }

            var imageDto = _mapper.Map<GameImageDto>(image);

            return imageDto;
        }

        public async Task CreateAsync(GameImageDto imageDto)
        {
            ThrowIfNotValidImage(imageDto);
            var image = await GetGameImageAsync(imageDto);

            await _gameImageRepository.AddAsync(image);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exist = await _gameImageRepository.AnyAsync(gameImage => gameImage.Id == id);

            if (!exist)
            {
                throw new EntityNotFoundException<GameImage>(id);
            }

            await _gameImageRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        private static void ThrowIfNotValidImage(GameImageDto image)
        {
            if (image?.Content == null || !image.Content.Any())
            {
                throw new InvalidServiceOperationException("Is empty image");
            }

            if (string.IsNullOrEmpty(image.GameKey))
            {
                throw new InvalidServiceOperationException("Is empty game key");
            }
        }

        private async Task<GameImage> GetGameImageAsync(GameImageDto imageDto)
        {
            var game = await _gameService.GetByKeyAsync(imageDto.GameKey);

            var image = new GameImage
            {
                Id = imageDto.Id,
                Content = imageDto.Content,
                ContentType = imageDto.ContentType,
                GameRootId = game.Id
            };

            return image;
        }
    }
}
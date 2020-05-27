using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly IAsyncRepository<Rating> _ratingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RatingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ratingRepository = _unitOfWork.GetRepository<IAsyncRepository<Rating>>();
        }

        public async Task CreateOrUpdateAsync(string gameId, string userId, int ratingValue)
        {
            Expression<Func<Rating, bool>> predicate = rating => rating.UserId == userId && rating.GameRootId == gameId;
            var existingRecord = await _ratingRepository.FindSingleAsync(predicate);

            if (existingRecord != null)
            {
                existingRecord.Value = ratingValue;
                await _ratingRepository.UpdateAsync(existingRecord);
                await _unitOfWork.CommitAsync();

                return;
            }

            await ValidateGameExisting(gameId);
            await ValidateUserExisting(userId);

            existingRecord = new Rating
            {
                GameRootId = gameId,
                UserId = userId,
                Value = ratingValue
            };

            await _ratingRepository.AddAsync(existingRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task<RatingDto> GetForGameAsync(string gameId)
        {
            var ratings = await _ratingRepository.FindAllAsync(r => r.GameRootId == gameId);
            var votesSum = ratings.Sum(r => r.Value);
            var votesCount = ratings.Count();

            var dto = new RatingDto(gameId, votesSum, votesCount);

            return dto;
        }

        private async Task ValidateUserExisting(string userId)
        {
            var userRepository = _unitOfWork.GetRepository<IAsyncRepository<User>>();
            var isExistUser = await userRepository.AnyAsync(u => u.Id == userId);

            if (!isExistUser)
            {
                throw new EntityNotFoundException<User>(userId);
            }
        }

        private async Task ValidateGameExisting(string gameId)
        {
            var gameRepository = _unitOfWork.GetRepository<IAsyncRepository<GameRoot>>();
            var isExistGame = await gameRepository.AnyAsync(g => g.Id == gameId);

            if (!isExistGame)
            {
                throw new EntityNotFoundException<GameRoot>(gameId);
            }
        }
    }
}
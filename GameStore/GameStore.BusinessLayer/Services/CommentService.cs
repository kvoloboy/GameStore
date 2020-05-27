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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Comment> _commentsRepository;

        public CommentService(IUnitOfWork unitOfWork, IGameService gameService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
            _mapper = mapper;
            _commentsRepository = unitOfWork.GetRepository<IAsyncRepository<Comment>>();
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsTreeByGameKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidServiceOperationException("Is not valid key");
            }

            var commentEntities = await _commentsRepository.FindAllAsync(c => c.GameRoot.Key == key);
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(commentEntities);
            var rootCommentsDto = commentsDto.Where(c => c.ParentId == null).ToList();
            var repliesDto = commentsDto.Except(rootCommentsDto);

            foreach (var comment in rootCommentsDto)
            {
                comment.Children = GetReplies(comment, repliesDto).ToList();
            }

            return rootCommentsDto;
        }

        public async Task CreateAsync(CommentDto commentDto)
        {
            ThrowIfNotValid(commentDto);
            
            var comment = _mapper.Map<Comment>(commentDto);
            await AttachGameToCommentAsync(comment);
            await _commentsRepository.AddAsync(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(CommentDto commentDto)
        {
            ThrowIfNotValid(commentDto);

            var comment = _mapper.Map<Comment>(commentDto);
            await AttachGameToCommentAsync(comment);
            await _commentsRepository.UpdateAsync(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exists = await _commentsRepository.AnyAsync(c => c.Id == id);

            if (!exists)
            {
                throw new EntityNotFoundException<Comment>(id);
            }

            await _commentsRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CommentDto> GetByIdAsync(string id)
        {
            var comment = await _commentsRepository.FindSingleAsync(c => c.Id == id);
            
            if (comment == null)
            {
                throw new EntityNotFoundException<Comment>(id);
            }

            var dto = _mapper.Map<CommentDto>(comment);

            return dto;
        }

        public async Task UpdateCommentsOwnerAsync(string oldUserId, string newUserId)
        {
            if (oldUserId == null || newUserId == null)
            {
                throw new InvalidServiceOperationException("Old and new user id shouldn't be null");
            }
            
            var comments = await _commentsRepository.FindAllAsync(c => c.UserId == oldUserId);

            foreach (var comment in comments)
            {
                comment.UserId = newUserId;
                await _commentsRepository.UpdateAsync(comment);
            }
        }

        private static IEnumerable<CommentDto> GetReplies(CommentDto root, IEnumerable<CommentDto> source)
        {
            var replies = source.Where(g => g.ParentId == root.Id);

            foreach (var reply in replies)
            {
                reply.Children = GetReplies(reply, source).ToList();
            }

            return replies;
        }

        private async Task AttachGameToCommentAsync(Comment comment)
        {
            var gameKey = comment.GameRoot.Key;
            var game = await _gameService.GetByKeyAsync(gameKey);

            if (game.IsDeleted)
            {
                throw new InvalidServiceOperationException("Can't attach comment to deleted game");
            }

            comment.GameRootId = game.Id;
        }

        private static void ThrowIfNotValid(CommentDto commentDto)
        {
            if (commentDto == null)
            {
                throw new InvalidServiceOperationException("Is null comment dto");
            }

            if (string.IsNullOrEmpty(commentDto.GameKey))
            {
                throw new InvalidServiceOperationException("Is empty game key");
            }

            if (string.IsNullOrEmpty(commentDto.UserId))
            {
                throw new InvalidServiceOperationException("Is empty user id");
            }
        }
    }
}
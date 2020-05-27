using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsTreeByGameKeyAsync(string key);
        Task CreateAsync(CommentDto comment);
        Task UpdateAsync(CommentDto commentDto);
        Task DeleteAsync(string id);
        Task<CommentDto> GetByIdAsync(string id);
        Task UpdateCommentsOwnerAsync(string oldUserId, string newUserId);
    }
}
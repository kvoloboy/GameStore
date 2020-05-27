using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreDto genre);
        Task UpdateAsync(GenreDto genre);
        Task DeleteAsync(string id);
        Task<IEnumerable<Genre>> GetAllAsync(Expression<Func<Genre, bool>> filter = null);
        Task<GenreDto> GetByIdAsync(string id);
        Task<IEnumerable<GenreNodeDto>> GetGenresTreeAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Genre> _genreRepository;


        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _genreRepository = unitOfWork.GetRepository<IAsyncRepository<Genre>>();
        }

        public async Task CreateAsync(GenreDto genreDto)
        {
            if (genreDto == null)
            {
                throw new InvalidServiceOperationException("Is null genre dto");
            }

            await ValidateGenreExistingByNameAsync(genreDto.Name);

            var genreEntity = _mapper.Map<Genre>(genreDto);
            await _genreRepository.AddAsync(genreEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(GenreDto genreDto)
        {
            if (genreDto == null)
            {
                throw new InvalidServiceOperationException("Is null genre dto");
            }

            await ValidateGenreNameAsync(genreDto.Id, genreDto.Name);

            var genreEntity = _mapper.Map<Genre>(genreDto);
            await _genreRepository.UpdateAsync(genreEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exists = await _genreRepository.AnyAsync(g => g.Id == id);

            if (!exists)
            {
                throw new EntityNotFoundException<Genre>(id);
            }

            await _genreRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllAsync(Expression<Func<Genre, bool>> filter = null)
        {
            var genres = await _genreRepository.FindAllAsync(filter);

            return genres;
        }

        public async Task<GenreDto> GetByIdAsync(string id)
        {
            var existingGenre = await _genreRepository.FindSingleAsync(g => g.Id == id);

            if (existingGenre == null)
            {
                throw new EntityNotFoundException<Genre>(id);
            }

            var dto = _mapper.Map<GenreDto>(existingGenre);

            return dto;
        }

        public async Task<IEnumerable<GenreNodeDto>> GetGenresTreeAsync()
        {
            var genres = await GetAllAsync();
            var genreNodesDto = _mapper.Map<IEnumerable<GenreNodeDto>>(genres).ToList();
            var rootGenres = genreNodesDto.Where(dto => dto.ParentId == null);
            var subGenres = genreNodesDto.Except(rootGenres);

            foreach (var genre in rootGenres)
            {
                genre.Children = GetChildren(genre, subGenres);
            }

            return rootGenres;
        }

        private static IEnumerable<GenreNodeDto> GetChildren(GenreNodeDto root, IEnumerable<GenreNodeDto> source)
        {
            var children = source.Where(g => g.ParentId == root.Id);

            foreach (var child in children)
            {
                child.Children = GetChildren(child, source);
            }

            return children;
        }

        private async Task ValidateGenreNameAsync(string id, string name)
        {
            var existingPlatformDto = await GetByIdAsync(id);
            var isChangedName = name != existingPlatformDto.Name;

            if (isChangedName)
            {
                await ValidateGenreExistingByNameAsync(name);
            }
        }

        private async Task ValidateGenreExistingByNameAsync(string name)
        {
            var alreadyExist = await _genreRepository.AnyAsync(g => g.Name == name);

            if (alreadyExist)
            {
                throw new EntityExistsWithKeyValueException<Genre>(nameof(Genre.Name), name);
            }
        }
    }
}
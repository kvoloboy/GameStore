using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.GenreViewModels;

namespace GameStore.Web.Factories
{
    public class GenreViewModelFactory : IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel>
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public GenreViewModelFactory(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }
        
        public async Task<GenreViewModel> CreateAsync(ModifyGenreViewModel model)
        {
            var modifyGenreViewModels = GetSelectionGenres(model.Id);
            var genreViewModel = new GenreViewModel
            {
                ModifyGenreViewModel = model,
                Parents = await modifyGenreViewModels
            };

            return genreViewModel;
        }
        
        private async Task<IEnumerable<ModifyGenreViewModel>> GetSelectionGenres(string genreId)
        {
            var genres = await _genreService.GetAllAsync(g => g.ParentId != genreId);
            var genreViewModels = _mapper.Map<IEnumerable<ModifyGenreViewModel>>(genres);

            return genreViewModels;
        }
    }
}
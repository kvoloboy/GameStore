using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Helpers.ViewModelHelpers;
using GameStore.Web.Models.ViewModels.CommentViewModels;

namespace GameStore.Web.Factories
{
    public class DisplayCommentViewModelFactory : IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel>
    {
        private readonly ICommentService _commentServices;
        private readonly IMapper _mapper;

        public DisplayCommentViewModelFactory(ICommentService commentServices, IMapper mapper)
        {
            _mapper = mapper;
            _commentServices = commentServices;
        }

        public async Task<DisplayCommentsViewModel> CreateAsync(CommentViewModel model)
        {
            var comments = await _commentServices.GetCommentsTreeByGameKeyAsync(model.GameKey);
            var commentViewModels = _mapper.Map<IEnumerable<CommentViewModel>>(comments);

            foreach (var viewModel in commentViewModels)
            {
                CommentViewModelHelper.SetupHtmlId(viewModel);
            }

            var displayCommentViewModel = new DisplayCommentsViewModel
            {
                Comment = new CommentViewModel {GameKey = model.GameKey},
                Comments = commentViewModels
            };

            return displayCommentViewModel;
        }
    }
}
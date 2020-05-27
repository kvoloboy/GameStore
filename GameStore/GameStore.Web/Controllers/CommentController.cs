using System;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Helpers.ViewModelHelpers;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("games/{key}")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentServices;
        private readonly IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel> _commentViewModelFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GameController> _logger;

        public CommentController(
            ICommentService commentServices,
            IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel> displayCommentViewModelFactory,
            IMapper mapper,
            ILogger<GameController> logger)
        {
            _commentServices = commentServices;
            _commentViewModelFactory = displayCommentViewModelFactory;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("reply")]
        [HasPermission(Permissions.CreateComment)]
        public async Task<IActionResult> ReplyAsync(string parentId, bool isQuote = false)
        {
            var commentViewModel = new CommentViewModel
            {
                ParentId = parentId
            };
            
            await FillReplyModelAsync(commentViewModel, isQuote);

            return PartialView("Reply", commentViewModel);
        }

        [HttpGet("comment")]
        [HasPermission(Permissions.CreateComment)]
        public IActionResult Create(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            var viewModel = new CommentViewModel
            {
                GameKey = key
            };

            return PartialView("Create", viewModel);
        }
        
        [HttpPost("comment")]
        [HasPermission(Permissions.CreateComment)]
        public async Task<IActionResult> CreateAsync(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                var isReply = !string.IsNullOrEmpty(comment.ParentId);
                var viewName = isReply ? nameof(ReplyAsync) : nameof(CreateAsync);

                if (isReply)
                {
                    await FillReplyModelAsync(comment);
                }

                return PartialView(viewName, comment);
            }

            comment.Id = Guid.NewGuid().ToString();
            CommentViewModelHelper.SetupHtmlId(comment);
            
            var commentDto = _mapper.Map<CommentDto>(comment);
            commentDto.UserId = User?.GetId();
            await _commentServices.CreateAsync(commentDto);
            _logger.LogDebug($"Add new comment to game with key {comment.GameKey}");

            return PartialView("DisplayComment", comment);
        }

        [HttpGet("comments")]
        public async Task<IActionResult> GetByGameKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return NotFound();
            }

            var commentViewModel = new CommentViewModel {GameKey = key};
            var displayCommentsViewModel =  await _commentViewModelFactory.CreateAsync(commentViewModel);
            _logger.LogDebug($"Getting comments by game key {key}");

            return PartialView("Comments", displayCommentsViewModel);
        }

        [HttpPost("delete")]
        [HasPermission(Permissions.DeleteComment)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _commentServices.DeleteAsync(id);
            return RedirectToAction("GetAllAsync", "Game");
        }

        private async Task FillReplyModelAsync(CommentViewModel commentViewModel, bool isQuote = false)
        {
            var parent = await _commentServices.GetByIdAsync(commentViewModel.ParentId);
            commentViewModel.Parent = _mapper.Map<CommentViewModel>(parent);
            commentViewModel.QuoteText = isQuote ? parent.Body : commentViewModel.QuoteText;
            commentViewModel.GameKey = parent.GameKey;
            
            CommentViewModelHelper.SetupHtmlId(commentViewModel);
        }
    }
}
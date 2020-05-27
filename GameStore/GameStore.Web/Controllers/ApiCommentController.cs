using System;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Extensions;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("api/games/{game-id}/comments")]
    [ApiController]
    public class ApiCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<ApiCommentController> _logger;
        private readonly IMapper _mapper;

        public ApiCommentController(
            ICommentService commentService,
            ILogger<ApiCommentController> logger,
            IMapper mapper)
        {
            _commentService = commentService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("new")]
        [ApiAuthorizationFilter(Permissions.CreateComment)]
        public async Task<IActionResult> CreateAsync(CommentViewModel comment)
        {
            comment.Id = Guid.NewGuid().ToString();
            comment.UserId = User?.GetId();

            var commentDto = _mapper.Map<CommentDto>(comment);
            await _commentService.CreateAsync(commentDto);
            _logger.LogDebug($"Add new comment to game with key {comment.GameKey}");

            return CreatedAtAction(nameof(GetByIdAsync), new {id = comment.Id}, comment);
        }

        [HttpPut("{id}")]
        [ApiAuthorizationFilter(Permissions.UpdateComment)]
        public async Task<IActionResult> UpdateAsync(CommentViewModel commentViewModel)
        {
            var commentDto = _mapper.Map<CommentDto>(commentViewModel);
            await _commentService.UpdateAsync(commentDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiAuthorizationFilter(Permissions.DeleteComment)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _commentService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentViewModel>> GetByIdAsync(string id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            var viewModel = _mapper.Map<CommentViewModel>(comment);

            return viewModel;
        }
    }
}
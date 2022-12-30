using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Posterr.Domain.Core.Pagination;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Services;
using Posterr.RestAPI.ApiInputs;
using Posterr.RestAPI.ApiResponses;
using Posterr.RestAPI.Controllers.Base;

namespace Posterr.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : AppControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepository;

        public PostsController(IMapper mapper, IPostService postService, IPostRepository postRepository)
        {
            this._mapper = mapper;
            this._postService = postService;
            this._postRepository = postRepository;
        }

        [HttpPost("New")]
        public async Task<IActionResult> NewPost([FromBody] NewPostInput input)
        {
            var authenticatedUserId = base.GetAuthenticatedUserId();
            var validationResult = await _postService.AddNewPostAsync(authenticatedUserId, input.Text, input.QuotePostId);
            validationResult?.AddToModelState(ModelState, null);
            if (ModelState.IsValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Repost")]
        public async Task<IActionResult> Repost([FromBody] RepostInput input)
        {
            var authenticatedUserId = base.GetAuthenticatedUserId();
            var validationResult = await _postService.RepostAsync(authenticatedUserId, input.PostId ?? 0);
            validationResult?.AddToModelState(ModelState, null);
            if (ModelState.IsValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("Search")]
        [ProducesResponseType(typeof(IEnumerable<GetFeedPostsResponse>), 200)]
        public async Task<IActionResult> Search([FromQuery] string textToSearch)
        {
            var posts = await _postRepository.SearchAsync(textToSearch);
            if (posts.Count() == 0)
            {
                return NotFound();
            }
            var result = _mapper.Map<IEnumerable<GetFeedPostsResponse>>(posts);
            return Ok(result);
        }
    }
}

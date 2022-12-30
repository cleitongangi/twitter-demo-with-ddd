using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Posterr.Domain.Core.Pagination;
using Posterr.Domain.Interfaces.GlobalSettings;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.RestAPI.ApiResponses;
using Posterr.RestAPI.Controllers.Base;

namespace Posterr.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : AppControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfigSettings _configSettings;
        private readonly IPostRepository _postRepository;
        

        public FeedController(IMapper mapper, IConfigSettings configSettings, IPostRepository postRepository)
        {
            this._mapper = mapper;
            this._configSettings = configSettings;
            this._postRepository = postRepository;            
        }

        [HttpGet("Posts")]
        [ProducesResponseType(typeof(PagedResult<GetFeedPostsResponse>), 200)]
        public async Task<IActionResult> AllPosts([FromQuery] int page = 1)
        {            
            var posts = await _postRepository.GetAllPostsAsync(page, _configSettings.PaginationHomeFeedPageSize);
            var result = _mapper.Map<PagedResult<GetFeedPostsResponse>>(posts);
            return Ok(result);
        }

        [HttpGet("Posts/Folowing")]
        [ProducesResponseType(typeof(PagedResult<GetFeedPostsResponse>), 200)]
        public async Task<IActionResult> FollowingPost([FromQuery] int page = 1)
        {
            var authenticatedUserId = base.GetAuthenticatedUserId();
            var posts = await _postRepository.GetFollowingPostsAsync(page, _configSettings.PaginationHomeFeedPageSize, authenticatedUserId);
            var result = _mapper.Map<PagedResult<GetFeedPostsResponse>>(posts);
            return Ok(result);
        }
    }
}

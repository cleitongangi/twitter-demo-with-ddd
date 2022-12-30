using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Posterr.Domain.Core.Pagination;
using Posterr.Domain.Interfaces.GlobalSettings;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Services;
using Posterr.RestAPI.ApiInputs;
using Posterr.RestAPI.ApiResponses;
using Posterr.RestAPI.Controllers.Base;

namespace Posterr.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : AppControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfigSettings _configSettings;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public UsersController(IMapper mapper, IConfigSettings configSettings, IUserService userService, IUserRepository userRepository, IPostRepository postRepository)
        {
            this._mapper = mapper;
            this._configSettings = configSettings;
            this._userService = userService;
            this._userRepository = userRepository;
            this._postRepository = postRepository;
        }

        [HttpGet("{userId:long}")]
        [ProducesResponseType(typeof(GetUserResponse), 200)]
        public async Task<IActionResult> Get(long userId)
        {
            var userData = await _userRepository.GetUserProfileDataAsync(id: userId, authenticatedUserId: base.GetAuthenticatedUserId());
            if (userData == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<GetUserResponse>(userData);
            return Ok(result);
        }

        [HttpPost("Following")]
        public async Task<IActionResult> Following([FromBody] FollowingUserInput input)
        {
            var authenticatedUserId = base.GetAuthenticatedUserId();
            var validationResult = await _userService.FollowUserAsync(authenticatedUserId, input.TargetUserId ?? 0);
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

        [HttpDelete("Following/{targetUserId:long}")]
        public async Task<IActionResult> Unfollowing([FromRoute] long? targetUserId)
        {
            var authenticatedUserId = base.GetAuthenticatedUserId();
            var validationResult = await _userService.UnfollowUserAsync(authenticatedUserId, targetUserId ?? 0);
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

        [HttpGet("{userId:long}/Posts")]
        [ProducesResponseType(typeof(PagedResult<GetFeedPostsResponse>), 200)]
        public async Task<IActionResult> UserPosts([FromRoute] long userId, [FromQuery] int page = 1)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(page, _configSettings.PaginationUserPostsPageSize, userId);
            var result = _mapper.Map<PagedResult<GetFeedPostsResponse>>(posts);
            return Ok(result);
        }
    }
}

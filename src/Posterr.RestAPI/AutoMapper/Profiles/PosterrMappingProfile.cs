using AutoMapper;
using Posterr.Domain.Core.Pagination;
using Posterr.Domain.DTOs;
using Posterr.Domain.Entities;
using Posterr.RestAPI.ApiResponses;
using Posterr.RestAPI.AutoMapper.CustomConverters;

namespace Posterr.RestAPI.AutoMapper.Profiles
{
    public class PosterrMappingProfile : Profile
    {
        public PosterrMappingProfile()
        {
            CreateMap<UserProfileDataDTO, GetUserResponse>(MemberList.None)
                .ConstructUsing(x => new GetUserResponse(x.UserId,
                                                         x.Username,
                                                         x.JoinedAtFormatted,
                                                         x.FollowersCount,
                                                         x.FollowingCount,
                                                         x.PostsCount,
                                                         x.Following)
                );

            CreateMap<PostEntity, GetFeedPostsResponse>(MemberList.None)
                .ForMember(
                    dest => dest.PostType,
                    opt => opt.MapFrom(src => (src.PostType == null ? string.Empty : src.PostType.TypeDescription)));
            CreateMap<PostEntity, GetFeedPostsResponse.ParentPost>(MemberList.None);

            CreateMap<PagedResult<PostEntity>, PagedResult<GetFeedPostsResponse>>(MemberList.None)
                .ConvertUsing<PagedResultConverter<PostEntity, GetFeedPostsResponse>>();
        }
    }
}
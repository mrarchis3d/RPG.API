using AutoMapper;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Dtos.Responses;

namespace RPG.Identity.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserStandardResponse>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}

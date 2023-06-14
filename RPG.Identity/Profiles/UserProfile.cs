using AutoMapper;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Dtos.Responses;

namespace RPG.Identity.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
           
        }

    }
}

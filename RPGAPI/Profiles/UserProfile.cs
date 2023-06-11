using AutoMapper;
using RPGAPI.Domain.UserAggregate;
using RPGAPI.Dtos.Responses;

namespace RPGAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
           
        }

    }
}

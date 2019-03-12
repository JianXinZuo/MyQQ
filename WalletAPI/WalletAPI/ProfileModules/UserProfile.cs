using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletComponent.Domains;
using WalletComponent.DtoModel;

namespace WalletAPI.ProfileModules
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDTO>();
            CreateMap<UserDTO, Users>();

            CreateMap<Users, ChatMessageUser>();
            CreateMap<ChatMessageUser, Users>();
        }
    }
}

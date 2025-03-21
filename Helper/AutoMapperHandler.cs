using AutoMapper;
using JwtUser.Modal;
using JwtUser.Repos.Models;

namespace JwtUser.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<TblCustomer, CustomerModal>().ForMember(dest => dest.StatusName, act => act.MapFrom(src => (src.IsActive.HasValue && src.IsActive.Value) ? "Active" : "InActive")).ReverseMap();
            CreateMap<TblUser, UserModel>().ForMember(dest => dest.Statusname, act => act.MapFrom(src => (src.Isactive.HasValue && src.Isactive.Value) ? "Active" : "InActive")).ReverseMap();
            CreateMap<TblRefreshtoken, RefreshToken>().ForMember(dest => dest.UserId , act => act.MapFrom(src => src.Userid)).ReverseMap();
            CreateMap<TblUser, UserDTO>()
                .ForMember(dest => dest.IsActive,
                    act => act.MapFrom(src => (src.Isactive.HasValue && src.Isactive.Value) ? "Active" : "Inactive"))
                .ReverseMap();
        }
    }
}

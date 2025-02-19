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
        }
        //public static Mapper InitializeAutoMapper()
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<TblCustomer, CustomerModal>().ForMember(dest => dest.StatusName, act => act.MapFrom(src => (src.IsActive.HasValue && src.IsActive.Value) ? "Active" : "InActive")).ReverseMap();
        //    });

        //    var mapper = new Mapper(config);
        //    return mapper;
        //}
    }
}

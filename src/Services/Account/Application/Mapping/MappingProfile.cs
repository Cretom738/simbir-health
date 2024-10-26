using Application.Classes.Data;
using Application.Dtos;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpDto, Account>()
                .ForMember(a => a.Roles, o => o.MapFrom(_ => new List<Role>() { Role.User }));

            CreateMap<JwtPairData, AuthDto>();

            CreateMap<JwtPayloadData, ValidationResultDto>();

            CreateMap<Account, AccountDto>();

            CreateMap<CreateAccountDto, Account>();

            CreateMap<UpdateAccountDto, Account>()
                .ForMember(a => a.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateCurrentAccountDto, Account>()
                .ForMember(a => a.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));
        }
    }
}

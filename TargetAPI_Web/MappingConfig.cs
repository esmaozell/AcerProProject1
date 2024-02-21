using AutoMapper;
using TargetAPI_Web.Models.Dto;

namespace TargetAPI_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<TargetAPIDto,TargetAPICreateDto>().ReverseMap();
            CreateMap<TargetAPIDto, TargetAPIUpdateDto>().ReverseMap();
        }
    }
}

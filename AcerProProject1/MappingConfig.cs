using AcerProProject1.Models;
using AcerProProject1.Models.Dto;
using AutoMapper;

namespace AcerProProject1
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<TargetAPI, TargetAPIDto>();
            CreateMap<TargetAPIDto, TargetAPI>();

            CreateMap<TargetAPI,TargetAPICreateDto>().ReverseMap();
            CreateMap<TargetAPI, TargetAPIUpdateDto>().ReverseMap();
        }
    }
}

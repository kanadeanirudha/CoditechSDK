using AutoMapper;
using Coditech.API.Data;
using Coditech.Common.API.Model;

namespace Coditech.API.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<BioradMedisyMediaModel, MediaDetail>().ReverseMap();
        }
    }
}

using System.Collections.Generic;
using AutoMapper;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.Settlements.Models;

namespace Thornless.UI.Web.ViewModels.Settlement
{
    public class SettlementMapperProfile : Profile
    {
        public SettlementMapperProfile()
        {
            CreateMap<SettlementTypeModel, ListSettlementTypeResponse.SettlementType>();

            CreateMap<SettlementResultsModel, GenerateSettlementResponse>()
                .ForMember(dest => dest.BuildingTypes, opts => opts.MapFrom(src => src.Buildings));

            CreateMap<KeyValuePair<BuildingTypeModel, List<BuildingNameResultModel>>, GenerateSettlementResponse.BuildingTypeModel>()
                .ForMember(dest => dest.Code, opts => opts.MapFrom(src => src.Key.Code))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Key.Name))
                .ForMember(dest => dest.Buildings, opts => opts.MapFrom(src => src.Value));

            CreateMap<BuildingNameResultModel, GenerateSettlementResponse.BuildingResultModel>();
        }
    }
}

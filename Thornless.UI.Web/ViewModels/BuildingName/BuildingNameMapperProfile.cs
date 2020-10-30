using AutoMapper;
using Thornless.Domain.BuildingNames.Models;

namespace Thornless.UI.Web.ViewModels.BuildingName
{
    public class BuildingNameMapperProfile : Profile
    {
        public BuildingNameMapperProfile()
        {
            CreateMap<BuildingTypeModel, BuildingTypesViewModel.BuildingType>();

            CreateMap<BuildingNameResultModel, BuildingNameResultViewModel>();
        }
    }
}

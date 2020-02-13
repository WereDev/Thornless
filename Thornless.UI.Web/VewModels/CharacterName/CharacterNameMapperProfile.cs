using AutoMapper;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.UI.Web.ViewModels.CharacterName
{
    public class CharacterNameMapperProfile : Profile
    {
        public CharacterNameMapperProfile()
        {
            CreateMap<AncestryModel, AncestryViewModel>();

            CreateMap<AncestryDetailsModel, AncestryDetailViewModel>();

            CreateMap<AncestryDetailsModel.AncestryOptionsModel, AncestryDetailViewModel.AncestryOption>();

            CreateMap<CharacterNameResultModel, CharacterNameViewModel>();

            CreateMap<CharacterNameResultModel.CharacterNameDefinition, CharacterNameViewModel.CharacterNameDefinition>();
        }
    }
}

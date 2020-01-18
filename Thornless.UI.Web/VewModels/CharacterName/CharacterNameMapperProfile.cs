using AutoMapper;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.UI.Web.ViewModels.CharacterName
{
    public class CharacterNameMapperProfile : Profile
    {
        public CharacterNameMapperProfile()
        {
            CreateMap<AncestryModel, AncestryViewModel>();
        }
    }
}

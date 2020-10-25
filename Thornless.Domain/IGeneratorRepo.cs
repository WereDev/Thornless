using System.Threading.Tasks;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.CharacterNames.Models;

namespace Thornless.Domain
{
    public interface IGeneratorRepo
    {
        Task<AncestryModel[]> ListAncestries();

        Task<AncestryDetailsModel> GetAncestry(string ancestryCode);

        Task<BuildingTypeModel[]> ListBuildingTypes();

        Task<BuildingTypeDetailsModel> GetBuildingType(string buildingTypeCode);

        Task<BuildingNameGroups> GetBuildingNameGroups();
    }
}

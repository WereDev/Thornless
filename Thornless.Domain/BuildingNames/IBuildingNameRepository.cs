using System.Threading.Tasks;
using Thornless.Domain.BuildingNames.Models;

namespace Thornless.Domain.BuildingNames
{
    public interface IBuildingNameRepository
    {
        Task<BuildingTypeModel[]> ListBuildingTypes();

        Task<BuildingTypeDetailsModel?> GetBuildingType(string buildingTypeCode);

        Task<BuildingNameGroups> GetBuildingNameGroups();
    }
}

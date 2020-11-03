using System.Threading.Tasks;
using Thornless.Domain.BuildingNames.Models;

namespace Thornless.Domain.BuildingNames
{
    public interface IBuildingNameGenerator
    {
        Task<BuildingTypeModel[]> ListBuildingTypes();

        Task<BuildingNameResultModel> GenerateBuildingName(string buildingTypeCode);
    }
}

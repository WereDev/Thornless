using System.Collections.Generic;
using Thornless.Domain.BuildingNames.Models;

namespace Thornless.Domain.Settlements.Models
{
    public class SettlementResultsModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Population { get; set; }

        public Dictionary<BuildingTypeModel, List<BuildingNameResultModel>> Buildings { get; } = new Dictionary<BuildingTypeModel, List<BuildingNameResultModel>>();        
    }
}

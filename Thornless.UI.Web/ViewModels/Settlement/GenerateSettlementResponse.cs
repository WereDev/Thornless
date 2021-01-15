namespace Thornless.UI.Web.ViewModels.Settlement
{
    public class GenerateSettlementResponse
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Population { get; set; }

        public BuildingTypeModel[] BuildingTypes { get; set; } = new BuildingTypeModel[0];

        public class BuildingTypeModel
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;

            public BuildingResultModel[] Buildings { get; set; } = new BuildingResultModel[0];
        }

        public class BuildingResultModel
        {
            public string BuildingName { get; set; } = string.Empty;
        }
    }
}

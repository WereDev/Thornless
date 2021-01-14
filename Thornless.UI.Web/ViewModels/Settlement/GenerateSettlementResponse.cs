namespace Thornless.UI.Web.ViewModels.Settlement
{
    public class GenerateSettlementResponse
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Population { get; set; }

        public BuildingTypeModel[] Buildings { get; set; } = new BuildingTypeModel[0];

        public class BuildingTypeModel
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;

            public BuildingNameResultModel[] BuildingNames { get; set; } = new BuildingNameResultModel[0];
        }

        public class BuildingNameResultModel
        {
            public string BuildingName { get; set; } = string.Empty;
        }
    }
}

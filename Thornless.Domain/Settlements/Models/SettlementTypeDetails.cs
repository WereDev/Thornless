namespace Thornless.Domain.Settlements.Models
{
    public class SettlementTypeDetails : SettlementTypeModel
    {
        public SettlementBuildingModel[] Buildings { get; set; } = new SettlementBuildingModel[0];
    }
}

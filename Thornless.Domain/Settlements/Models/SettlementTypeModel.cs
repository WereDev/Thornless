namespace Thornless.Domain.Settlements.Models
{
    public class SettlementTypeModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public int MinSize { get; set; }

        public int MaxSize { get; set; }
    }
}

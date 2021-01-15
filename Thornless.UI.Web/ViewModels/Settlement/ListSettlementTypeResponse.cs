namespace Thornless.UI.Web.ViewModels.Settlement
{
  public class ListSettlementTypeResponse
  {
    public SettlementType[] SettlementTypes { get; set; } = new SettlementType[0];

    public class SettlementType
    {
      public string Code { get; set; } = string.Empty;

      public string Name { get; set; } = string.Empty;

      public int SortOrder { get; set; }

      public int MinSize { get; set; }

      public int MaxSize { get; set; }
    }
  }
}

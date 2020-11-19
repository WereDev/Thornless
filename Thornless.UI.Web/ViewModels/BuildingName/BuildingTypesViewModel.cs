using System;

namespace Thornless.UI.Web.ViewModels.BuildingName
{
    public class BuildingTypesViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

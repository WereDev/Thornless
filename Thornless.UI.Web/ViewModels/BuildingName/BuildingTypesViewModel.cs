using System;

namespace Thornless.UI.Web.ViewModels.BuildingName
{
    public class BuildingTypesViewModel
    {
        public BuildingType[] BuildingTypes { get; set; }

        public class BuildingType
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Copyright { get; set; }
            public DateTimeOffset LastUpdatedDate { get; set; }
            public int SortOrder { get; set; }
        }
    }
}

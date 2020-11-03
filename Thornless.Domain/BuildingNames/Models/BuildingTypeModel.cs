using System;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingTypeModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Copyright { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

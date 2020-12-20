using System;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingTypeModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

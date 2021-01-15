using System;

namespace Thornless.Domain.CharacterNames.Models
{
    public class AncestryModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string FlavorHtml { get; set; } = string.Empty;
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

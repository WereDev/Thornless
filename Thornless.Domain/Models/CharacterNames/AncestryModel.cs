using System;

namespace Thornless.Domain.Models.CharacterNames
{
    public class AncestryModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Copyright { get; set; }
        public string FlavorHtml { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

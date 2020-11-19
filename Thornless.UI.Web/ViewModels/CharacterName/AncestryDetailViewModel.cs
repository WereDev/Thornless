using System;

namespace Thornless.UI.Web.ViewModels.CharacterName
{
    public class AncestryDetailViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string FlavorHtml { get; set; } = string.Empty;
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
        public AncestryOption[] Options { get; set; } = new AncestryOption[0];

        public class AncestryOption
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }
    }
}

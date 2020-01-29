using System;

namespace Thornless.UI.Web.ViewModels.CharacterName
{
    public class AncestryDetailViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Copyright { get; set; }
        public string FlavorHtml { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int SortOrder { get; set; }
        public AncestryOption[] Options { get; set; }

        public class AncestryOption
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
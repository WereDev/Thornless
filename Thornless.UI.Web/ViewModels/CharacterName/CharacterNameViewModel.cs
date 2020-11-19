namespace Thornless.UI.Web.ViewModels.CharacterName
{
    public class CharacterNameViewModel
    {
        public string Name { get; set; } = string.Empty;
        
        public string AncestryCode { get; set; } = string.Empty;

        public string AncestryName { get; set; } = string.Empty;

        public string OptionCode { get; set; } = string.Empty;

        public string OptionName { get; set; } = string.Empty;

        public CharacterNameDefinition[] Definitions { get; set; } = new CharacterNameDefinition[0];

        public class CharacterNameDefinition
        {
            public string NamePart { get; set; } = string.Empty;

            public string[] Meanings { get; set; } = new string[0];
        }
    }
}

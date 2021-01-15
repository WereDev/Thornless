namespace Thornless.Domain.CharacterNames.Models
{
    public class CharacterNameResultModel
    {
        public string Name { get; set; } = string.Empty;
        
        public string AncestryCode { get; set; } = string.Empty;

        public string AncestryName { get; set; } = string.Empty;

        public string OptionCode { get; set; } = string.Empty;

        public string OptionName { get; set; } = string.Empty;

        public CharacterNameDefinition[] Definitions { get; set; } = new CharacterNameDefinition[0];

        public class CharacterNameDefinition
        {
            public string NamePartCode { get; set; } = string.Empty;
            
            public string NamePart { get; set; } = string.Empty;

            public string[] Meanings { get; set; } = new string[0];

            public override string ToString()
            {
                return NamePart;
            }
        }
    }
}

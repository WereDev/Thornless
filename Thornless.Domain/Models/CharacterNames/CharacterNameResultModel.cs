namespace Thornless.Domain.Models.CharacterNames
{
    public class CharacterNameResultModel
    {
        public string Name { get; set; }
        
        public string AncestryCode { get; set; }

        public string AncestryName { get; set; }

        public string OptionCode { get; set; }

        public string OptionName { get; set; }

        public CharacterNameDefinition[] Definitions { get; set; }

        public class CharacterNameDefinition
        {
            public string NamePartCode { get; set; }
            
            public string NamePart { get; set; }

            public string[] Meanings { get; set; }

            public override string ToString()
            {
                return NamePart;
            }
        }
    }
}

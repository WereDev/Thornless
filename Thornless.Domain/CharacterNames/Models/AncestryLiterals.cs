namespace Thornless.Domain.CharacterNames.Models
{
    public static class AncestryLiterals
    {
        public const string Space = "literal_space";
        public const string Apostrophe = "literal_'";
        public const string Empty = "literal_";

        public static readonly string[] Literals = new string[]
        {
            Space,
            Apostrophe,
            Empty,
        };
    }
}

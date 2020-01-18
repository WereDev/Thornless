namespace Thornless.Domain.Models.CharacterNames
{
    public class AncestryOptionsModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string[] NamePartSeperators { get; set; }

        public int SeperatorChancePercentage { get; set; }

        public int SortOrder { get; set; }

        public AncestrySegmentGroupModel[] SegmentGroups { get; set; }
    }
}

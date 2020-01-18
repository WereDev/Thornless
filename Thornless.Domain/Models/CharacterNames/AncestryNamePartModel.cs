namespace Thornless.Domain.Models.CharacterNames
{
    public class AncestryNamePartModel : BaseWeightedItemModel
    {
        public string NameSegmentCode { get; set; }

        public string[] NameParts { get; set; }

        public string[] NameMeanings { get; set; }
    }
}

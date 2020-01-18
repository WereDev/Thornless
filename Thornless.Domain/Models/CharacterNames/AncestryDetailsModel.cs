namespace Thornless.Domain.Models.CharacterNames
{
    public class AncestryDetailsModel : AncestryModel
    {
        public AncestryOptionsModel[] Options { get; set; }

        public AncestryNamePartModel[] NameParts { get; set; }
    }
}

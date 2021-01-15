using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("CharacterAncestryNamePart")]
    internal class CharacterAncestryNamePartDto : BasedWeightedItemDto
    {
        public int CharacterAncestryId { get; set; }

        public string NameSegmentCode { get; set; } = string.Empty;

        public string NamePartsJson { get; set; } = string.Empty;

        public string NameMeaningsJson { get; set; } = string.Empty;
    }
}

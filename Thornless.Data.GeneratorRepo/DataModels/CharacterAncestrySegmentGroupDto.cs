using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("CharacterAncestrySegmentGroup")]
    internal class CharacterAncestrySegmentGroupDto : BasedWeightedItemDto
    {
        public int CharacterAncestryOptionId { get; set; }

        public string NameSegmentCodesJson { get; set; } = string.Empty;
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("CharacterAncestryOption")]
    internal class CharacterAncestryOptionDto : BaseIdDto
    {
        public int CharacterAncestryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NamePartSeperatorJson { get; set; }

        public int SeperatorChancePercentage { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey(nameof(CharacterAncestrySegmentGroupDto.CharacterAncestryOptionId))]
        public List<CharacterAncestrySegmentGroupDto> SegmentGroups {get;set;}
    }
}

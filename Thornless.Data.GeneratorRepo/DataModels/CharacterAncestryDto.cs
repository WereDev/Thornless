using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("CharacterAncestry")]
    internal class CharacterAncestryDto : BaseIdDto
    {
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Copyright { get; set; }
        
        public string FlavorHtml { get; set; }
        
        public DateTimeOffset LastUpdatedDate { get; set; }
        
        public int SortOrder { get; set; }

        [ForeignKey(nameof(CharacterAncestryOptionDto.CharacterAncestryId))]
        public List<CharacterAncestryOptionDto> Options { get; set; }

        [ForeignKey(nameof(CharacterAncestryNamePartDto.CharacterAncestryId))]
        public List<CharacterAncestryNamePartDto> NameParts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("CharacterAncestry")]
    internal class CharacterAncestryDto : BaseIdDto
    {
        
        public string Code { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
        
        public string Copyright { get; set; } = string.Empty;
        
        public string FlavorHtml { get; set; } = string.Empty;
        
        public DateTimeOffset LastUpdatedDate { get; set; }
        
        public int SortOrder { get; set; }

        [ForeignKey(nameof(CharacterAncestryOptionDto.CharacterAncestryId))]
        public List<CharacterAncestryOptionDto> Options { get; set; } = new List<CharacterAncestryOptionDto>();

        [ForeignKey(nameof(CharacterAncestryNamePartDto.CharacterAncestryId))]
        public List<CharacterAncestryNamePartDto> NameParts { get; set; } = new List<CharacterAncestryNamePartDto>();
    }
}

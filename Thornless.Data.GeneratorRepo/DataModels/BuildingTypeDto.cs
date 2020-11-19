using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("BuildingType")]
    internal class BuildingTypeDto : BaseIdDto
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Copyright { get; set; } = string.Empty;

        public string LastUpdatedDate { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        [ForeignKey(nameof(BuildingNameFormatDto.BuildingTypeId))]
        public List<BuildingNameFormatDto> NameFormats {get;set;} = new List<BuildingNameFormatDto>();
    }
}

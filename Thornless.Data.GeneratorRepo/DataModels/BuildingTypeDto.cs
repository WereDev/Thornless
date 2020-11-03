using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("BuildingType")]
    internal class BuildingTypeDto : BaseIdDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Copyright { get; set; }

        public string LastUpdatedDate { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey(nameof(BuildingNameFormatDto.BuildingTypeId))]
        public List<BuildingNameFormatDto> NameFormats {get;set;}
    }
}

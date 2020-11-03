using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("BuildingNameFormat")]
    internal class BuildingNameFormatDto : BasedWeightedItemDto
    {
        public int BuildingTypeId { get; set; }

        public string NameFormat { get; set; }
    }
}

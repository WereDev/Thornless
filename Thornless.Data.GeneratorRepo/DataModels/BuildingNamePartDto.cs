using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("BuildingNamePart")]
    internal class BuildingNamePartDto : BasedWeightedItemDto
    {
        public string GroupCode { get; set; } = string.Empty;

        public string NamePart { get; set; } = string.Empty;
    }
}

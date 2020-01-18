namespace Thornless.Data.GeneratorRepo.DataModels
{
    internal abstract class BasedWeightedItemDto : BaseIdDto
    {
        public int RandomizationWeight { get; set; }
    }
}

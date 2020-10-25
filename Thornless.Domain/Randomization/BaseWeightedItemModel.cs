namespace Thornless.Domain.Randomization
{
    public abstract class BaseWeightedItemModel : IWeightedItem
    {
        public int RandomizationWeight { get; set; }
    }
}

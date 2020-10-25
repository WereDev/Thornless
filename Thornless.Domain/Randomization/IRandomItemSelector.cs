namespace Thornless.Domain.Randomization
{
    public interface IRandomItemSelector
    {
        T GetRandomWeightedItem<T>(T[] items)
            where T : IWeightedItem;

        T GetRandomItem<T>(T[] items);
    }
}

namespace Thornless.Domain.Interfaces
{
    public interface IRandomItemSelector
    {
        T GetRandomWeightedItem<T>(T[] items)
            where T : IWeightedItem;

        T GetRandomItem<T>(T[] items);
    }
}

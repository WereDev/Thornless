using System;
using System.Linq;

namespace Thornless.Domain.Randomization
{
    public class RandomItemSelector : IRandomItemSelector
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public RandomItemSelector(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }

        public T GetRandomWeightedItem<T>(T[] items)
            where T : class, IWeightedItem
        {
            if (items == null || items.Length == 0)
                throw new ArgumentException($"{nameof(items)} is null or empty.");

            if (items.Length == 1)
                return items[0];

            var totalWeight = items.Sum(x => x.RandomizationWeight);
            var randomValue = _randomNumberGenerator.GetRandomInteger(totalWeight);

            foreach (var item in items)
            {
                randomValue -= item.RandomizationWeight;
                if (randomValue <= 0) return item;
            }

            throw new InvalidOperationException("Error trying to determine random item.");
        }

        public T GetRandomItem<T>(T[] items)
            where T : class
        {
            if (items == null || items.Length == 0)
                throw new ArgumentException($"{nameof(items)} is null or empty.");

            if (items.Length == 1)
                return items[0];

            var randomValue = _randomNumberGenerator.GetRandomInteger(items.Length);

            return items[randomValue - 1];
        }
    }
}

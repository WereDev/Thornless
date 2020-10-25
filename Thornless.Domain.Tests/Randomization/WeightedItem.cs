using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests.Randomization
{
    internal class WeightedItem : IWeightedItem
    {
        public int RandomizationWeight { get; set; }
    }
}

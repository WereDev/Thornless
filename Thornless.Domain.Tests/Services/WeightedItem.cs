using Thornless.Domain.Interfaces;

namespace Thornless.Domain.Tests.Services
{
    internal class WeightedItem : IWeightedItem
    {
        public int RandomizationWeight { get; set; }
    }
}

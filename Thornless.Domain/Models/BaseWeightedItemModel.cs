using Thornless.Domain.Interfaces;

namespace Thornless.Domain.Models
{
    public abstract class BaseWeightedItemModel : IWeightedItem
    {
        public int RandomizationWeight { get; set; }
    }
}

using Moq;
using NUnit.Framework;
using Thornless.Domain.Interfaces;
using Thornless.Domain.Services;

namespace Thornless.Domain.Tests.Services
{
    [TestFixture]
    public class RandomItemGeneratorTests
    {
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        public void GetRandomItem_WithCollection_ReturnsCorrectItem(int randomNumber, int expectedItem)
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator.Setup(x => x.GetRandomInteger(3)).Returns(randomNumber);
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);

            var items = new WeightedItem[]
            {
                new WeightedItem { RandomizationWeight = 1 },
                new WeightedItem { RandomizationWeight = 2 },
                new WeightedItem { RandomizationWeight = 3 },
            };

            var item = randomItemSelector.GetRandomItem<WeightedItem>(items);
            Assert.AreEqual(items[expectedItem], item);
            mockRandomNumberGenerator.Verify(x => x.GetRandomInteger(3), Times.Once);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [Test]
        public void GetRandomItem_WithEmptyArray_ReturnsNull()
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);
            var item = randomItemSelector.GetRandomItem<WeightedItem>(null);
            Assert.IsNull(item);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [Test]
        public void GetRandomItem_WithNullArray_ReturnsNull()
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);
            var item = randomItemSelector.GetRandomItem<WeightedItem>(null);
            Assert.IsNull(item);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 2)]
        [TestCase(6, 2)]
        public void GetRandomWeightedItem_WithCollection_ReturnsCorrectItem(int randomNumber, int expectedItem)
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator.Setup(x => x.GetRandomInteger(6)).Returns(randomNumber);
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);

            var items = new WeightedItem[]
            {
                new WeightedItem { RandomizationWeight = 1 },
                new WeightedItem { RandomizationWeight = 2 },
                new WeightedItem { RandomizationWeight = 3 },
            };

            var item = randomItemSelector.GetRandomWeightedItem<WeightedItem>(items);
            Assert.AreEqual(items[expectedItem], item);
            mockRandomNumberGenerator.Verify(x => x.GetRandomInteger(6), Times.Once);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [Test]
        public void GetRandomWeightedItem_WithEmptyArray_ReturnsNull()
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);
            var item = randomItemSelector.GetRandomWeightedItem<WeightedItem>(new WeightedItem[0]);
            Assert.IsNull(item);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [Test]
        public void GetRandomWeightedItem_WithNullArray_ReturnsNull()
        {
            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            var randomItemSelector = new RandomItemSelector(mockRandomNumberGenerator.Object);
            var item = randomItemSelector.GetRandomWeightedItem<WeightedItem>(null);
            Assert.IsNull(item);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }
    }
}

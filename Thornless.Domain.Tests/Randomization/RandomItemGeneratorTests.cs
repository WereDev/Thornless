using System;
using Moq;
using NUnit.Framework;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests.Randomization
{
    [TestFixture]
    public class RandomItemGeneratorTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetRandomItem_WithCollection_ReturnsCorrectItem(int randomNumber)
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
            Assert.AreEqual(items[randomNumber - 1], item);
            mockRandomNumberGenerator.Verify(x => x.GetRandomInteger(3), Times.Once);
            mockRandomNumberGenerator.VerifyNoOtherCalls();
        }

        [Test]
        public void GetRandomItem_WithEmptyArray_Throws()
        {
            var randomItemSelector = new RandomItemSelector(new RandomNumberGenerator());
            Assert.Throws<ArgumentException>(() => randomItemSelector.GetRandomItem<WeightedItem>(new WeightedItem[0]));
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
        public void GetRandomWeightedItem_WithEmptyArray_Throws()
        {
            var randomItemSelector = new RandomItemSelector(new RandomNumberGenerator());
            Assert.Throws<ArgumentException>(() => randomItemSelector.GetRandomWeightedItem<WeightedItem>(new WeightedItem[0]));
        }
    }
}

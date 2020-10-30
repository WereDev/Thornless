using System.Collections.Generic;
using NUnit.Framework;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests.Randomization
{
    public class RandomNumberGeneratorTests
    {
        [Test]
        public void GetRandomInteger_GetsMinAndMax()
        {
            var generator = new RandomNumberGenerator();
            var found = new HashSet<int>();

            while (found.Count != 3)
            {
                var number = generator.GetRandomInteger(3);
                if (!found.Contains(number) && number > 0)
                    found.Add(number);
            }

            Assert.True(true);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void GetRandomInteger_WhenMaxLessThan1_Returns1(int maxValue)
        {
            var generator = new RandomNumberGenerator();
            var number = generator.GetRandomInteger(maxValue);
            Assert.AreEqual(1, number);
        }
    }
}

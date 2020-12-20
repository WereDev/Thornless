using Moq;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests
{
    public static class RandomizationHelper
    {
        public static Mock<IRandomNumberGenerator> CreateMockRandomNumberGenerator()
        {
            var mockRng = new Mock<IRandomNumberGenerator>();
            mockRng.Setup(x => x.GetRandomInteger(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((min, max) => min);
            mockRng.Setup(x => x.GetRandomInteger(It.IsAny<int>()))
                .Returns<int>((rnd) => rnd);
            return mockRng;
        }
    }
}
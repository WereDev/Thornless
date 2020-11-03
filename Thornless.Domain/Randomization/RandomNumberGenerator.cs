using System;

namespace Thornless.Domain.Randomization
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;

        public RandomNumberGenerator()
        {
            _random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
        }

        public int GetRandomInteger(int maxValue)
        {
            if (maxValue <= 0)
                return 1;

            return _random.Next(0, maxValue) + 1;
        }
    }
}

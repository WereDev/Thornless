using System;
using Thornless.Domain.Interfaces;

namespace Thornless.Domain.Services
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;

        public RandomNumberGenerator()
        {
            _random = new Random((Int32)(DateTime.Now.Ticks % Int32.MaxValue));
        }

        public int GetRandomInteger(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}

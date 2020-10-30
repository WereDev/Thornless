namespace Thornless.Domain.Randomization
{
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Generates a random integer.
        /// </summary>
        /// <param name="maxValue">The upper bound of the value to return.</param>
        /// <returns>An integer greater than or equal to 1 and less than or equal to maxValue.</returns>
        int GetRandomInteger(int maxValue);
    }
}

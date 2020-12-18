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

        /// <summary>
        /// Generates a random integer.
        /// </summary>
        /// <param name="minValue">The lower bound of the value to return: >= 0.</param>
        /// <param name="maxValue">The upper bound of the value to return: >= minValue.</param>
        /// <returns>An integer greater than or equal to minValue and less than or equal to maxValue.</returns>
        int GetRandomInteger(int minValue, int maxValue);
    }
}

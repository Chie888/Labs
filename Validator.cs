namespace CafeMenuApp
{
    /// <summary>
    /// Класс для проверки корректности вводимых значений.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Проверяет, что целое число неотрицательное.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True, если значение неотрицательное; иначе false.</returns>
        public static bool IsNonNegative(int value) => value >= 0;

        /// <summary>
        /// Проверяет, что десятичное число неотрицательное.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True, если значение неотрицательное; иначе false.</returns>
        public static bool IsNonNegative(decimal value) => value >= 0;

        /// <summary>
        /// Проверяет, что число с плавающей точкой неотрицательное.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True, если значение неотрицательное; иначе false.</returns>
        public static bool IsNonNegative(double value) => value >= 0;
    }
}

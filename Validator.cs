using System;

namespace CafeMenuApp
{
    /// <summary>
    /// Класс для проверки корректности вводимых данных.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Проверяет, что целое число неотрицательное.
        /// </summary>
        public static bool IsNonNegative(int value) => value >= 0;

        /// <summary>
        /// Проверяет, что десятичное число неотрицательное.
        /// </summary>
        public static bool IsNonNegative(decimal value) => value >= 0;

        /// <summary>
        /// Проверяет, что число с плавающей точкой неотрицательное.
        /// </summary>
        public static bool IsNonNegative(double value) => value >= 0;
    }
}

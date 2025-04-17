using System;

public static class InputHelper
{
    public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out value) && value >= min && value <= max)
                return value;
            Console.WriteLine($"Ошибка! Введите целое число от {min} до {max}.");
        }
    }

    public static string ReadString(string prompt, bool allowEmpty = false)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input != null && (allowEmpty || input.Trim().Length > 0))
                return input.Trim();
            Console.WriteLine("Ошибка! Введите непустую строку.");
        }
    }

    public static char ReadChar(string prompt, char[]? allowedChars = null)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && input.Length == 1)
            {
                char c = input[0];
                if (allowedChars == null || Array.Exists(allowedChars, ch => ch == c))
                    return c;
            }
            Console.WriteLine("Ошибка! Введите один символ из допустимых.");
        }
    }
}

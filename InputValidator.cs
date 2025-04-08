using System;

public class InputValidator
{
    public static double GetPositiveDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out double value) && value > 0)
                return value;
            else
                Console.WriteLine("Пожалуйста, введите положительное число.");
        }
    }
}

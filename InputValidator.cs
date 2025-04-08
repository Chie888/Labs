using System;

public class InputValidator
{
    public static int GetInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value > 0 )
                return value;
            else
                Console.WriteLine("Пожалуйста, введите целое число больше нуля.");
        }
    }

    public static string GetString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}

using System;
using System.IO;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {            
            string[] lines = File.ReadAllLines("test_input.txt");
            Console.WriteLine("Исходный текст программы:");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();

            // объединяем строки в один текст с переводами строк
            string fullText = string.Join("\n", lines);

           
            using (var outputFile = new StreamWriter("codes_symbols.txt"))
            {
                foreach (char ch in fullText)
                {
                    outputFile.Write(((int)ch) + " ");
                }
                outputFile.Write("0 "); // признак конца файла
            }
            

            InputOutput.SetSource(fullText);

            
            LexicalAnalyzer lexer = new LexicalAnalyzer();

            
            InputOutput.ErrorTable.Clear();

            
            Parser parser = new Parser(lexer);
            parser.ParseProgram();

            
            InputOutput.ErrorTable.PrintErrors();

            Console.WriteLine("Парсинг завершён.");
        }
    }
}

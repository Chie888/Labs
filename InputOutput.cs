using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Компилятор
{
    public struct TextPosition
    {
        public uint lineNumber; // номер строки
        public byte charNumber; // номер позиции в строке

        public TextPosition(uint ln, byte c)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }
    public class Err
    {
        public TextPosition errorPosition { get; set; }
        public byte errorCode { get; set; }

        public Err(TextPosition position, byte code)
        {
            errorPosition = position;
            errorCode = code;
        }
    }


    public class ErrorTable
    {
        private List<Err> errors = new List<Err>();
        private const int ERRMAX = 9;

        public void AddError(Err err)
        {
            if (errors.Count < ERRMAX)
                errors.Add(err);
        }

        public void AddError(int line, int column, byte errorCode)
        {
            AddError(new Err(new TextPosition((uint)line, (byte)column), errorCode));
        }

        public void PrintErrors()
        {
            if (errors.Count == 0)
            {
                Console.WriteLine("Ошибок нет.");
                return;
            }

            int count = 1;
            foreach (var err in errors)
            {
                string message = ErrorMessageByCode(err.errorCode);
                Console.WriteLine($"Ошибка {count} в строке {err.errorPosition.lineNumber}, столбце {err.errorPosition.charNumber - 1}: {message}");
                count++;
            }
        }

        public bool HasErrors() => errors.Count > 0;

        public void Clear() => errors.Clear();

        private string ErrorMessageByCode(byte code)
        {
            switch (code)
            {
                case 201: return "Неизвестный символ";
                case 203: return "Константа превышает предел";
                case 204: return "Ожидалось числовое значение после точки";
                
                case 100: return "Ожидался другой символ";
                case 101: return "Ожидалось ключевое слово 'program'";
                case 102: return "Ожидалось имя программы";
                case 103: return "Ожидалось ключевое слово 'begin'";
                case 104: return "Ожидалось ключевое слово 'end'";
                case 105: return "Ожидалось ключевое слово 'var'";
                case 106: return "Ожидался идентификатор";
                case 107: return "Ожидался тип данных";
                case 108: return "Ожидался идентификатор";
                case 109: return "Ожидалось имя процедуры";
                case 110: return "Неизвестный или пустой оператор";
                case 111: return "Ожидалась точка в конце программы";
                case 112: return "Ожидалось двоеточие после объявления идентификатора";
                case 113: return "Ожидался нижний индекс массива";
                case 114: return "Ожидался верхний индекс массива";
                case 115: return "Ожидалось ключевое слово 'of'";



                default: return "Неизвестная ошибка";
            }
        }
    }



    class InputOutput
    {
        
        public static ErrorTable ErrorTable = new ErrorTable();

        public static void Error(byte errorCode, TextPosition position)
        {
            ErrorTable.AddError(new Err(position, errorCode));
        }
        
        public static char Ch { get; set; }
        public static TextPosition positionNow = new TextPosition();
        static string line;
        static byte lastInLine = 0;
        
        public static StreamReader File { get;  set; }
        
        static string sourceText;
        static int index = 0; // текущий индекс в sourceText
        static int length = 0;

        
        public static void SetSource(string text)
        {
            sourceText = text;
            length = sourceText.Length;
            index = 0;
            positionNow = new TextPosition(1, 0); // строка 1, позиция 0
            NextCh(); // загрузить первый символ
        }
        
        public static void SetFile(StreamReader file)
        {
            File = file;
            sourceText = null;
            index = 0;
            length = 0;
            positionNow = new TextPosition(1, 0);
            NextCh();
        }
        public static void NextCh()
        {
            if (sourceText == null)
                throw new InvalidOperationException("Источник текста не установлен.");

            if (index >= length)
            {
                Ch = '\0'; // конец текста
                return;
            }

            Ch = sourceText[index];
            index++;

            
            if (Ch == '\r')
            {                
                if (index < length && sourceText[index] == '\n')
                {
                    index++;
                }
                positionNow.lineNumber++;
                positionNow.charNumber = 0;
                Ch = '\n'; 
            }
            else if (Ch == '\n')
            {
                positionNow.lineNumber++;
                positionNow.charNumber = 0;
            }
            else
            {
                positionNow.charNumber++;
            }
        }


        public static void PrevCh()
        {
            if (index <= 0)
                throw new InvalidOperationException("Невозможно откатиться назад за начало текста.");

            index--;

            Ch = sourceText[index];
            Console.WriteLine(Ch);
           
            if (Ch == '\n')
            {
                positionNow.lineNumber--;
                
                positionNow.charNumber = 0; // упрощённо
            }
            else
            {
                positionNow.charNumber--;
                if (positionNow.charNumber < 0)
                    positionNow.charNumber = 0;
            }
        }

        public static char PeekChar()
        {
            if (index >= length)
                return '\0';
            return sourceText[index];
        }



        private static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        private static void ReadNextLine()
        {
            if (!File.EndOfStream)
            {
                line = File.ReadLine();
                //err = new List<Err>();
            }
            else
            {
                End();
            }
        }

        static void End()
        {
            //Console.WriteLine($"Компиляция завершена: : ошибок — {ErrorTable.errCount}!");
        }

        
    }
}
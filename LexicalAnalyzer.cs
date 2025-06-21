using System;
using System.Collections.Generic;

namespace Компилятор
{
    public class LexicalAnalyzer
    {
        Keywords keywords = new Keywords();
        
        public const byte
            star = 21, // *
            slash = 60, // /
            equal = 16, // =
            comma = 20, // ,
            semicolon = 14, // ;
            colon = 5, // :
            point = 61,	// .
            arrow = 62,	// ^
            leftpar = 9,	// (
            rightpar = 4,	// )
            lbracket = 11,	// [
            rbracket = 12,	// ]
            flpar = 63,	// {
            frpar = 64,	// }
            later = 65,	// <
            greater = 66,	// >
            laterequal = 67,	//  <=
            greaterequal = 68,	//  >=
            latergreater = 69,	//  <>
            plus = 70,	// +
            minus = 71,	// –
            lcomment = 72,	//  (*
            rcomment = 73,	//  *)
            assign = 51,	//  :=
            twopoints = 74,	//  ..
            ident = 2,	// идентификатор
            floatc = 82,	// вещественная константа
            intc = 15,	// целая константа
            casesy = 31,
            elsesy = 32,
            filesy = 57,
            gotosy = 33,
            thensy = 52,
            typesy = 34,
            untilsy = 53,
            dosy = 54,
            withsy = 37,
            ifsy = 56,
            insy = 100,
            ofsy = 101,
            orsy = 102,
            tosy = 103,
            endsy = 104,
            varsy = 105,
            divsy = 106,
            andsy = 107,
            notsy = 108,
            forsy = 109,
            modsy = 110,
            nilsy = 111,
            setsy = 112,
            beginsy = 113,
            whilesy = 114,
            arraysy = 115,
            constsy = 116,
            labelsy = 117,
            downtosy = 118,
            packedsy = 119,
            recordsy = 120,
            repeatsy = 121,
            programsy = 122,
            functionsy = 123,
            procedurensy = 124,
            xorsy = 125,
            integersy = 126,
            realsy = 127,
            charsy = 128,
            booleansy = 129,
            writelnsy = 130;

        public byte symbol; // код символа
        public TextPosition token; // позиция символа
        public string addrName; // адрес идентификатора в таблице имен
        public int nmb_int; // значение целой константы
        public float nmb_float; // значение вещественной константы
        public char one_symbol; // значение символьной константы

        public byte NextSym()
        {
            
            while (char.IsWhiteSpace(InputOutput.Ch)) InputOutput.NextCh();

            
            token = new TextPosition(InputOutput.positionNow.lineNumber, InputOutput.positionNow.charNumber);

            
            if (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
            {
                byte digit;
                Int16 maxint = Int16.MaxValue;
                bool overflow = false;
                nmb_int = 0;
                while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                {
                    digit = (byte)(InputOutput.Ch - '0');
                    if (nmb_int < maxint / 10 || (nmb_int == maxint / 10 && digit <= maxint % 10))
                    {
                        nmb_int = 10 * nmb_int + digit;
                    }
                    else
                    {
                        overflow = true;
                        break;
                    }
                    InputOutput.NextCh();
                }
                if (overflow)
                {
                    InputOutput.Error(203, token);
                    // пропуск остатка числа
                    while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9') InputOutput.NextCh();
                    nmb_int = 0;
                }

                if (InputOutput.Ch == '.')
                {
                    char nextChar = InputOutput.PeekChar();
                    if (nextChar == '.')
                    {
                        symbol = intc; InputOutput.NextCh(); InputOutput.NextCh();

                    }
                    else
                    {
                        float fraction = 0;
                        float divisor = 10;

                        nmb_float = 0;

                        if (!(InputOutput.Ch >= '0' && InputOutput.Ch <= '9'))
                        {

                            InputOutput.Error(204, token); //ожидалось число после точки
                            nmb_float = 0;

                        }

                        while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                        {
                            digit = (byte)(InputOutput.Ch - '0');
                            fraction += digit / divisor;
                            divisor *= 10;
                            InputOutput.NextCh();
                        }

                        // Формируем вещественное число
                        nmb_float = nmb_int + fraction;
                        symbol = floatc;
                    }
                }
                else symbol = intc;
            }
            else if ((InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z') || (InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z'))
            {
                string name = "";
                while ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                       (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z') ||
                       (InputOutput.Ch >= '0' && InputOutput.Ch <= '9'))
                {
                    name += InputOutput.Ch;
                    InputOutput.NextCh();
                }
                
                string nameLower = name.ToLower(); // приведение к нижнему регистру


                byte code = ident; 
                foreach (var group in keywords.Kw.Values)
                {
                    if (group.TryGetValue(name.ToLower(), out byte keywordCode))
                    {
                        code = keywordCode;
                        break;
                    }
                }
                addrName = name;
                symbol = code;
            }
            else
            {
                switch (InputOutput.Ch)
                {                    
                    case '<':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '=')
                        {
                            symbol = laterequal; InputOutput.NextCh();
                        }
                        else if (InputOutput.Ch == '>')
                        {
                            symbol = latergreater; InputOutput.NextCh();
                        }
                        else
                            symbol = later;
                        break;
                    case ':':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '=')
                        {
                            symbol = assign; InputOutput.NextCh();
                        }
                        else
                            symbol = colon;

                        break;
                    case ';':
                        InputOutput.NextCh();
                        symbol = semicolon;
                        
                        break;
                    case '.':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '.')
                        {
                            symbol = twopoints; InputOutput.NextCh();
                        }
                        else symbol = point;
                        break;
                    case ',':
                        InputOutput.NextCh();
                        symbol = comma;
                        break;
                    case '(':
                        InputOutput.NextCh();
                        symbol = leftpar;
                        break;
                    case ')':
                        InputOutput.NextCh();
                        symbol = rightpar;
                        break;
                    case '[':
                        InputOutput.NextCh();
                        symbol = lbracket;
                        break;
                    case ']':
                        InputOutput.NextCh();
                        symbol = rbracket;
                        break;
                    case '+':
                        InputOutput.NextCh();
                        symbol = plus;
                        break;
                    case '-':
                        InputOutput.NextCh();
                        symbol = minus;
                        break;
                    case '*':
                        InputOutput.NextCh();
                        symbol = star;
                        break;
                    case '/':
                        InputOutput.NextCh();
                        symbol = slash;
                        break;
                    case '=':
                        InputOutput.NextCh();
                        symbol = equal;
                        break;
                    case '>':
                        InputOutput.NextCh();
                        if (InputOutput.Ch == '=')
                        {
                            InputOutput.NextCh();
                            symbol = greaterequal; // >=
                        }
                        else
                        {
                            symbol = greater; // >
                        }
                        break;
                    case '\0':
                        symbol = 0; // конец файла
                        break;
                    default:
                        
                        TextPosition errorPos = InputOutput.positionNow;
                        InputOutput.Error(201, errorPos); // неизвестный символ
                        InputOutput.NextCh();
                        symbol = 0;
                        break;

                }
            }

            return symbol;
        }


    }
}

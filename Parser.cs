using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Компилятор
{
    public class Parser
    {
        private static readonly HashSet<byte> syncTokens = new HashSet<byte>
        {
            LexicalAnalyzer.semicolon,
            LexicalAnalyzer.endsy,
            LexicalAnalyzer.beginsy,
            LexicalAnalyzer.procedurensy,
            LexicalAnalyzer.point,

            // другие подходящие токены
        };

        private LexicalAnalyzer lexer;
        private byte symbol;           // текущий токен
        private TextPosition tokenPos; // позиция текущего токена

        public Parser(LexicalAnalyzer lexer)
        {
            this.lexer = lexer;
            NextSym();
        }

        private void Recover()
        {
            while (!syncTokens.Contains(symbol) && symbol != 0)
            {
                NextSym();
            }
        }

        private void NextSym()
        {
            symbol = lexer.NextSym();
            tokenPos = lexer.token;
        }

        private void Error(byte code, TextPosition pos)
        {
            InputOutput.ErrorTable.AddError(new Err(pos, code));
        }

        private void Expect(byte expectedSymbol)
        {
            if (symbol == expectedSymbol)
            {
                NextSym();
            }
            else
            {
                Error(100, tokenPos); // ожидался другой символ
                Recover();
            }
        }

        public void ParseProgram()
        {
            if (symbol == LexicalAnalyzer.programsy)
            {
                NextSym();
                if (symbol == LexicalAnalyzer.ident)
                {
                    NextSym();
                    Expect(LexicalAnalyzer.semicolon);

                    ParseBlock();

                    Expect(LexicalAnalyzer.point);
                }
                else
                {
                    Error(102, tokenPos); // ожидалось имя программы
                    Recover();

                }
            }
            else
            {
                Error(101, tokenPos); // ожидалось 'program'
                Recover();

            }
        }

        private void ParseBlock()
        {
            ParseVariableDeclarations();

            while (symbol == LexicalAnalyzer.procedurensy)
            {
                ParseProcedure();
            }

            if (symbol == LexicalAnalyzer.beginsy)
            {
                NextSym();
                ParseStatementSequence();
                if (symbol != LexicalAnalyzer.point)
                {
                    Error(111, tokenPos); // ожидалась точка в конце программы
                    Recover();
                }

            }
            else
            {
                Error(103, tokenPos); // ожидалось 'begin'
                Recover();
            }
        }

        private void ParseVariableDeclarations()
        {
            if (symbol == LexicalAnalyzer.varsy)
            {
                NextSym();
                while (symbol == LexicalAnalyzer.ident)
                {
                    ParseVariableDeclaration();
                }
            }
            else if (symbol == LexicalAnalyzer.procedurensy)
            {
                ParseProcedure();
            }
            else
            {
                Error(105, tokenPos); // ожидалось 'var'
            }
        }

        private void ParseVariableDeclaration()
        {
            // Список идентификаторов, разделённых запятыми
            do
            {
                if (symbol == LexicalAnalyzer.ident)
                {
                    NextSym();
                }
                else
                {
                    Error(106, tokenPos); // ожидался идентификатор
                    Recover();
                   
                }

                if (symbol == LexicalAnalyzer.comma)
                {
                    NextSym();
                }
                else
                {
                    break;
                }
            } while (true);

            if (symbol != LexicalAnalyzer.colon)
            {
                Error(112, tokenPos); //Ожидалось двоеточие
                Recover();
            }
            
            NextSym();  
            ParseType();
            Expect(LexicalAnalyzer.semicolon);
        }

        private void ParseType()
        {
            
            if (symbol == LexicalAnalyzer.arraysy)
            {
                // array[<intc> .. <intc>] of <type>
                NextSym();
                Expect(LexicalAnalyzer.lbracket);
                if (symbol != LexicalAnalyzer.intc)
                {
                    Error(113, tokenPos); //Ожидался нижний индекс массива
                    Recover();
                }
                else 
                {

                    Expect(LexicalAnalyzer.twopoints);
                    if (symbol != LexicalAnalyzer.intc)
                    {
                        Error(114, tokenPos); //Ожидался верхний индекс массива
                        Recover();
                    }
                    else
                    {
                        Expect(LexicalAnalyzer.rbracket);
                        if (symbol == LexicalAnalyzer.ofsy)
                        {
                            ParseType();
                        }
                        else
                        {
                            Error(115, tokenPos);
                            Recover();
                        }
                    }
                }
            }
            else if (symbol == LexicalAnalyzer.integersy ||
                     symbol == LexicalAnalyzer.realsy ||
                     symbol == LexicalAnalyzer.charsy ||
                     symbol == LexicalAnalyzer.booleansy)
            {
                NextSym();
            }
            else
            {
                
                Error(107, tokenPos); // ожидался тип данных
                NextSym();
            }
        }

        private void ParseProcedure()
        {
            Expect(LexicalAnalyzer.procedurensy);

            if (symbol == LexicalAnalyzer.ident)
            {
                NextSym();
            }
            else
            {
                Error(109, tokenPos); // ожидалось имя процедуры
                Recover();
            }

            if (symbol == LexicalAnalyzer.leftpar)
            {
                NextSym();
                ParseParameterList();
                Expect(LexicalAnalyzer.rightpar);
            }

            Expect(LexicalAnalyzer.semicolon);

            if (symbol == LexicalAnalyzer.varsy)
            {
                ParseVariableDeclarations();
            }

            else if (symbol == LexicalAnalyzer.beginsy)
            {
                NextSym();
                ParseStatementSequence();
                
                Expect(LexicalAnalyzer.semicolon);
            }
            else
            {
                Error(201, tokenPos); // ожидалось 'begin'
            }
        }

        private void ParseParameterList()
        {
            do
            {
                if (symbol == LexicalAnalyzer.ident)
                {
                    NextSym();
                    while (symbol == LexicalAnalyzer.comma)
                    {
                        NextSym();
                        if (symbol == LexicalAnalyzer.ident)
                            NextSym();
                        else
                        {
                            Error(106, tokenPos); //ожидался идентификатор
                            Recover();
                        }
                    }
                    Expect(LexicalAnalyzer.colon);
                    ParseType();
                }
                else
                {
                    Error(106, tokenPos); //ожидался идентификатор
                    Recover();
                }

                if (symbol != LexicalAnalyzer.semicolon)
                    break;
                NextSym();
            } while (true);
        }

        private void ParseStatementSequence()
        {
            while (symbol != LexicalAnalyzer.endsy)
            {
                ParseStatement();
                if (symbol == LexicalAnalyzer.semicolon)
                    NextSym();
                
                else break;
            }
            NextSym();
        }


        private void ParseStatement()
        {
            if (symbol == LexicalAnalyzer.ident)
            {
                NextSym();

                // Индексированная переменная (массив)
                bool isIndexed = false;
                if (symbol == LexicalAnalyzer.lbracket)
                {
                    isIndexed = true;
                    NextSym();
                    ParseExpression();
                    while (symbol == LexicalAnalyzer.comma)
                    {
                        NextSym();
                        ParseExpression();
                    }
                    Expect(LexicalAnalyzer.rbracket);
                }

                // Присваивание
                if (symbol == LexicalAnalyzer.assign)
                {
                    NextSym();
                    ParseExpression();
                }
                // Вызов процедуры с параметрами
                else if (!isIndexed && symbol == LexicalAnalyzer.leftpar)
                {
                    NextSym();
                    ParseExpressionList();
                    Expect(LexicalAnalyzer.rightpar);
                }
                
               
            }
            else if (symbol == LexicalAnalyzer.beginsy)
            {
                NextSym();
                ParseStatementSequence();
                if (symbol != LexicalAnalyzer.endsy)
                {
                    Error(104, tokenPos); // ожидалось 'end'
                    Recover();
                }
                else
                {
                    NextSym();
                }
            }
            else if (symbol == LexicalAnalyzer.endsy)
            {
                
            }
            else if (symbol == LexicalAnalyzer.semicolon)
            {
                
                NextSym();
            }
            else
            {
                Error(110, tokenPos); // неизвестный или пустой оператор
                Recover();
            }
        }


        private void ParseExpressionList()
        {
            ParseExpression();
            while (symbol == LexicalAnalyzer.comma)
            {
                NextSym();
                ParseExpression();
            }
        }


        private void ParseExpression()
        {
            if (symbol == LexicalAnalyzer.plus || symbol == LexicalAnalyzer.minus)
            {
                NextSym();
            }
            ParseTerm();
            while (symbol == LexicalAnalyzer.plus || symbol == LexicalAnalyzer.minus || symbol == LexicalAnalyzer.orsy || symbol == LexicalAnalyzer.xorsy)
            {
                NextSym();
                ParseTerm();
            }
        }

        private void ParseTerm()
        {
            ParseFactor();
            while (symbol == LexicalAnalyzer.star || symbol == LexicalAnalyzer.slash || symbol == LexicalAnalyzer.divsy || symbol == LexicalAnalyzer.modsy || symbol == LexicalAnalyzer.andsy)
            {
                NextSym();
                ParseFactor();
            }
        }

        private void ParseFactor()
        {
            if (symbol == LexicalAnalyzer.ident)
            {
                NextSym();
                if (symbol == LexicalAnalyzer.leftpar)
                {
                    NextSym();
                    ParseExpressionList();
                    Expect(LexicalAnalyzer.rightpar);
                }
                else if (symbol == LexicalAnalyzer.lbracket)
                {
                    NextSym();
                    ParseExpression();
                    while (symbol == LexicalAnalyzer.comma)
                    {
                        NextSym();
                        ParseExpression();
                    }
                    Expect(LexicalAnalyzer.rbracket);
                }
            }
            else if (symbol == LexicalAnalyzer.intc || symbol == LexicalAnalyzer.floatc)
            {
                NextSym();
            }
            else if (symbol == LexicalAnalyzer.leftpar)
            {
                NextSym();
                ParseExpression();
                Expect(LexicalAnalyzer.rightpar);
            }
            else if (symbol == LexicalAnalyzer.notsy)
            {
                NextSym();
                ParseFactor();
            }
            else
            {
                Error(201, tokenPos);
                NextSym();
            }
        }
    }


}

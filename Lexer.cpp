#include "Lexer.h"
#include <cctype>
#include <climits>
#include <cstdlib>
#include <cerrno>

Lexer::Lexer(InputModule& inputModule, ErrorTable& errorTable)
    : input(inputModule), errors(errorTable)
{
    initKeywords();
    advance();
}

void Lexer::initKeywords() 
{
    keywords = 
    {
        {"program", TokenType::KEYWORD},
        {"var", TokenType::KEYWORD},
        {"begin", TokenType::KEYWORD},
        {"end", TokenType::KEYWORD},
        {"if", TokenType::KEYWORD},
        {"then", TokenType::KEYWORD},
        {"else", TokenType::KEYWORD},
        {"while", TokenType::KEYWORD},
        {"do", TokenType::KEYWORD},
        {"for", TokenType::KEYWORD},
        {"to", TokenType::KEYWORD},
        {"function", TokenType::KEYWORD},
        {"procedure", TokenType::KEYWORD},
        {"const", TokenType::KEYWORD},
        {"type", TokenType::KEYWORD},
        {"repeat", TokenType::KEYWORD},
        {"until", TokenType::KEYWORD},
        {"case", TokenType::KEYWORD},
        {"of", TokenType::KEYWORD},
        {"with", TokenType::KEYWORD},
        {"div", TokenType::KEYWORD},
        {"mod", TokenType::KEYWORD},
        {"and", TokenType::KEYWORD},
        {"or", TokenType::KEYWORD},
        {"not", TokenType::KEYWORD},
        {"xor", TokenType::KEYWORD},
        {"integer", TokenType::KEYWORD},
        {"real", TokenType::KEYWORD},
        {"char", TokenType::KEYWORD},
        {"boolean", TokenType::KEYWORD},
        {"array", TokenType::KEYWORD},
        {"writeln", TokenType::KEYWORD}
    };
}


void Lexer::advance() 
{
    if (input.isEOF()) 
    {
        eofReached = true;
        currentChar = '\0';
    }
    else 
    {
        currentChar = input.nextch();
        if (input.isEOF()) 
        {
            eofReached = true;
        }
    }
}


void Lexer::skipWhitespace() 
{
    while (!eofReached && std::isspace(static_cast<unsigned char>(currentChar))) 
    {
        advance();
    }
}

Token Lexer::getNextToken() 
{
    if (hasBufferedToken) 
    {
        hasBufferedToken = false;
        return bufferedToken;
    }

    skipWhitespace();

    if (eofReached) 
    {
        return Token(TokenType::END_OF_FILE, "", input.getLine(), input.getColumn());
    }

    int tokenLine = input.getLine();
    int tokenColumn = input.getColumn();

    if (std::isdigit(static_cast<unsigned char>(currentChar))) 
    {
        return readNumber();
    }
    
    if (std::isalpha(static_cast<unsigned char>(currentChar)) || currentChar == '_') 
    {
        return readIdentifier();
    }

    switch (currentChar) 
    {
    case '.':
    {
        advance();
        if (currentChar == '.') 
        {
            advance();
            return Token(TokenType::DOTDOT, "..", tokenLine, tokenColumn);
        }
        else 
        {
            return Token(TokenType::DOT, ".", tokenLine, tokenColumn);
        }
    }
    case ',':
        advance();
        return Token(TokenType::COMMA, ",", tokenLine, tokenColumn);
    case ';':
        advance();
        return Token(TokenType::SEMICOLON, ";", tokenLine, tokenColumn);
    case '(':
        advance();
        return Token(TokenType::LPAREN, "(", tokenLine, tokenColumn);
    case ')':
        advance();
        return Token(TokenType::RPAREN, ")", tokenLine, tokenColumn);
    case '[':
        advance();
        return Token(TokenType::LBRACKET, "[", tokenLine, tokenColumn);
    case ']':
        advance();
        return Token(TokenType::RBRACKET, "]", tokenLine, tokenColumn);
    case '+':
        advance();
        return Token(TokenType::PLUS, "+", tokenLine, tokenColumn);
    case '-':
        advance();
        return Token(TokenType::MINUS, "-", tokenLine, tokenColumn);
    case '*':
        advance();
        return Token(TokenType::MUL, "*", tokenLine, tokenColumn);
    case '/':
        advance();
        return Token(TokenType::DIV, "/", tokenLine, tokenColumn);
    case '=':
            advance();
            return Token(TokenType::EQUAL, "=", tokenLine, tokenColumn);
    case '<':
    {
        advance();
        if (currentChar == '=') 
        {
            advance();
            return Token(TokenType::LESS_EQUAL, "<=", tokenLine, tokenColumn);
        }
        else if (currentChar == '>') 
        {
            advance();
            return Token(TokenType::NOT_EQUAL, "<>", tokenLine, tokenColumn);
        }
        else 
        {
            return Token(TokenType::LESS, "<", tokenLine, tokenColumn);
        }
    }
    case '>':
    {
        advance();
        if (currentChar == '=') 
        {
            advance();
            return Token(TokenType::GREATER_EQUAL, ">=", tokenLine, tokenColumn);
        }
        else 
        {
            return Token(TokenType::GREATER, ">", tokenLine, tokenColumn);
        }
    }
    case ':':
    {
        advance();
        if (currentChar == '=') 
        { 
            advance();
            return Token(TokenType::ASSIGN, ":=", tokenLine, tokenColumn);
        }
        else 
        {
            return Token(TokenType::COLON, ":", tokenLine, tokenColumn);
        }
    }
    default:
        std::string unknownLexeme(1, currentChar);
        advance();
        return Token(TokenType::UNKNOWN, unknownLexeme, tokenLine, tokenColumn);
    }
}


void Lexer::ungetToken(const Token& token) 
{
    if (hasBufferedToken) 
    {
        errors.addError(token.line, token.column, "Ошибка: попытка повторного возврата токена в буфер лексера");
        return;
    }
    bufferedToken = token;
    hasBufferedToken = true;
}


Token Lexer::readNumber() 
{
    std::string numberStr;
    int tokenLine = input.getLine();
    int tokenColumn = input.getColumn();

    while (!eofReached && std::isdigit(static_cast<unsigned char>(currentChar))) 
    {
        numberStr += currentChar;
        advance();
    }

    checkNumberRange(numberStr, tokenLine, tokenColumn);

    return Token(TokenType::NUMBER, numberStr, tokenLine, tokenColumn);
}

Token Lexer::readIdentifier() 
{
    std::string identStr;
    int tokenLine = input.getLine();
    int tokenColumn = input.getColumn();

    while (!eofReached && (std::isalnum(static_cast<unsigned char>(currentChar)) || currentChar == '_')) 
    {
        identStr += currentChar;
        advance();
    }

    // Приводим к нижнему регистру для поиска ключевых слов (Pascal нечувствителен к регистру)
    for (auto& c : identStr) 
    {
        c = static_cast<char>(std::tolower(static_cast<unsigned char>(c)));
    }

    auto it = keywords.find(identStr);
    if (it != keywords.end()) 
    {
        return Token(it->second, identStr, tokenLine, tokenColumn);
    }
    else 
    {
        return Token(TokenType::IDENTIFIER, identStr, tokenLine, tokenColumn);
    }
}


bool Lexer::checkNumberRange(const std::string& numStr, int line, int column) 
{
    char* endptr = nullptr;
    errno = 0;
    long val = strtol(numStr.c_str(), &endptr, 10);

    if (*endptr != '\0' || errno == ERANGE || val < INT_MIN || val > INT_MAX) 
    {
        errors.addError(line, column, "Число выходит за пределы допустимого диапазона");
        return false;
    }
    return true;
}

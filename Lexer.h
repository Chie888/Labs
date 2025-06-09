#pragma once
#include <string>
#include <unordered_map>
#include "ErrorTable.h"
#include "nextch.h"


enum class TokenType 
{
    IDENTIFIER,
    NUMBER,
    KEYWORD,
    DOT,            // '.'
    COMMA,          // ','
    COLON,          // ':'
    SEMICOLON,      // ';'
    LPAREN,         // '('
    RPAREN,         // ')'
    LBRACKET,       // '['
    RBRACKET,       // ']'
    ASSIGN,         // ':='
    PLUS,           // '+'
    MINUS,          // '-'
    MUL,            // '*'
    DIV,            // '/'
    LESS,           // '<'
    LESS_EQUAL,     // '<='
    GREATER,        // '>'
    GREATER_EQUAL,  // '>='
    EQUAL,          // '='
    NOT_EQUAL,      // '<>'
    DOTDOT,         // '..'
    END_OF_FILE,
    UNKNOWN
};


struct Token 
{
    TokenType type;
    std::string lexeme;
    int line;
    int column;

    Token(TokenType t = TokenType::UNKNOWN, const std::string& lex = "", int l = 0, int c = 0)
        : type(t), lexeme(lex), line(l), column(c) {
    }
};


class Lexer 
{
public:
    Lexer(InputModule& input, ErrorTable& errors);
    Token getNextToken();
    void ungetToken(const Token& token);  

private:
    InputModule& input;
    ErrorTable& errors;

    std::unordered_map<std::string, TokenType> keywords;

    char currentChar = '\0';
    bool eofReached = false;

    bool hasBufferedToken = false;  
    Token bufferedToken;            

    void initKeywords();
    void advance();
    void skipWhitespace();
    Token readNumber();
    Token readIdentifier();
    bool checkNumberRange(const std::string& numStr, int line, int column);
};
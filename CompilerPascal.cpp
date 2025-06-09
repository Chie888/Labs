#include <iostream>
#include "ErrorTable.h"
#include "nextch.h"
#include "Lexer.h"
#include "Parser.h"

int main() {
    setlocale(LC_ALL, "Russian");
    system("chcp 1251");

    ErrorTable errors;
    InputModule input("test3.1.txt", errors);
    Lexer lexer(input, errors);
    Parser parser(lexer, errors);

    parser.parseProgram();

    std::cout << "Лексический анализ:" << std::endl;
    while (true) {
        Token token = lexer.getNextToken();
        if (token.type == TokenType::END_OF_FILE)
            break;

        std::cout << "Токен: " << token.lexeme << " (Тип: ";
        switch (token.type) {
        case TokenType::IDENTIFIER: std::cout << "IDENTIFIER"; break;
        case TokenType::NUMBER: std::cout << "NUMBER"; break;
        case TokenType::KEYWORD: std::cout << "KEYWORD"; break;
        case TokenType::DOT: std::cout << "DOT"; break;
        case TokenType::COMMA: std::cout << "COMMA"; break;
        case TokenType::COLON: std::cout << "COLON"; break;
        case TokenType::SEMICOLON: std::cout << "SEMICOLON"; break;
        case TokenType::LPAREN: std::cout << "LPAREN"; break;
        case TokenType::RPAREN: std::cout << "RPAREN"; break;
        case TokenType::LBRACKET: std::cout << "LBRACKET"; break;
        case TokenType::RBRACKET: std::cout << "RBRACKET"; break;
        case TokenType::ASSIGN: std::cout << "ASSIGN"; break;
        case TokenType::PLUS: std::cout << "PLUS"; break;
        case TokenType::MINUS: std::cout << "MINUS"; break;
        case TokenType::MUL: std::cout << "MUL"; break;
        case TokenType::DIV: std::cout << "DIV"; break;
        case TokenType::LESS: std::cout << "LESS"; break;
        case TokenType::LESS_EQUAL: std::cout << "LESS_EQUAL"; break;
        case TokenType::GREATER: std::cout << "GREATER"; break;
        case TokenType::GREATER_EQUAL: std::cout << "GREATER_EQUAL"; break;
        case TokenType::EQUAL: std::cout << "EQUAL"; break;
        case TokenType::NOT_EQUAL: std::cout << "NOT_EQUAL"; break;
        case TokenType::UNKNOWN: std::cout << "UNKNOWN"; break;
        default: std::cout << "OTHER"; break;
        }
        std::cout << ") в строке " << token.line << ", столбце " << token.column << std::endl;
    }

    if (errors.hasErrors()) 
    {
        errors.printErrors();
    }
    else 
    {
        std::cout << "Ошибок не обнаружено." << std::endl;
    }

    return 0;
}

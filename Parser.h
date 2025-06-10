#pragma once
#include "Lexer.h"
#include "ErrorTable.h"
#include "SemanticAnalyzer.h"
#include <initializer_list>
#include <string>

class Parser 
{
    SemanticAnalyzer semanticAnalyzer;

public:
    Parser(Lexer& lexer, ErrorTable& errors);

    void parseProgram();

private:
    Lexer& lexer;
    ErrorTable& errors;
    Token currentToken;

    void advance();
    void expect(TokenType type, const std::string& errMsg);
    void error(const std::string& message);

    void parseVariableDeclarations();
    void parseVariableDeclaration();
    std::string parseType();
    std::string parseArrayType();
  
    void parseProcedure();
    void parseParameterList();
    void parseStatementSequence();
    void parseStatement();
    void parseAssignment();
    void parseProcedureCall();
    void parseCompoundStatement();
    void parseVariable();

    void parseExpression();
   

    void synchronize(const std::initializer_list<TokenType>& syncTokens);
};

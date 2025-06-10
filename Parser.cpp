#include "Parser.h"
#include <iostream>
#include <string>

Parser::Parser(Lexer& lex, ErrorTable& err) : lexer(lex), errors(err), semanticAnalyzer(err)
{
    advance();
}

void Parser::advance() 
{
    currentToken = lexer.getNextToken();
}

void Parser::error(const std::string& message) 
{
    std::cerr << "Ошибка в строке " << currentToken.line
        << ", столбце " << currentToken.column
        << ": " << message
        << " (Текущий токен: '" << currentToken.lexeme
        << "', Тип: " << static_cast<int>(currentToken.type) << ")"
        << std::endl;
    errors.addError(currentToken.line, currentToken.column, message);
}


void Parser::synchronize(const std::initializer_list<TokenType>& syncTokens) 
{
    while (currentToken.type != TokenType::END_OF_FILE)
    {
        for (auto t : syncTokens) 
        {
            if (currentToken.type == t) return;
        }
        advance();
    }
}


void Parser::expect(TokenType type, const std::string& errMsg) 
{
    if (currentToken.type == type) 
    {
        advance();
    }
    else 
    {
        error(errMsg);
        synchronize({ TokenType::SEMICOLON, TokenType::KEYWORD, TokenType::IDENTIFIER, TokenType::DOT });
    }
}


// Точка входа
void Parser::parseProgram() 
{
    if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "program") 
    {
        advance();
        expect(TokenType::IDENTIFIER, "Ожидалось имя программы");
        expect(TokenType::SEMICOLON, "Ожидалась точка с запятой после имени программы");
        parseVariableDeclarations();
        if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "procedure")
        {
            while (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "procedure")
            {
                parseProcedure();
            }
        }
        else if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "begin")
        {
            advance();
            parseStatementSequence();
            expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'end' (конец программы)");
            expect(TokenType::DOT, "Ожидалась точка в конце программы");
        }
    }
    else {
        error("Ожидалось ключевое слово 'program'");
        synchronize({ TokenType::DOT, TokenType::END_OF_FILE });
    }
}


void Parser::parseVariableDeclarations() 
{
    if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "var") 
    {
        advance();
        while (currentToken.type == TokenType::IDENTIFIER) 
        {
            parseVariableDeclaration();
            //advance();
            //expect(TokenType::SEMICOLON, "Ожидалась точка с запятой после объявления переменной");
        }
    }
}


void Parser::parseVariableDeclaration() 
{
    std::vector<std::string> varNames;
    
    if (currentToken.type == TokenType::IDENTIFIER) 
    {
        varNames.push_back(currentToken.lexeme);  // сохраняем лексему, только если это идентификатор
        advance();
        // используем identLexeme дальше
    }
    else 
    {
        error("Ожидался идентификатор");
        advance();
        
    }    

    while (currentToken.type == TokenType::COMMA) 
    {
        advance();
        if (currentToken.type == TokenType::IDENTIFIER) 
        {
            varNames.push_back(currentToken.lexeme);
            advance();
        }
        else 
        {
            error("Ожидался идентификатор");
            advance();
        }
    }

        
    expect(TokenType::COLON, "Ожидался двоеточие после списка идентификаторов");

    std::string typeName = parseType();

    expect(TokenType::SEMICOLON, "Ожидалась точка с запятой после объявления переменной");
    
    for (const auto& name : varNames) 
    {
        if (!semanticAnalyzer.declareVariable(name, typeName, std::nullopt, currentToken.line, currentToken.column)) 
        {
            error("Повторное объявление переменной: " + name);
        }
    }
}



std::string Parser::parseType() 
{
    if (currentToken.type == TokenType::KEYWORD) 
    {
        std::string typeName = currentToken.lexeme;

        if (typeName == "integer" || typeName == "real" || typeName == "char" 
            || typeName == "boolean") 
        {
            advance();
            return typeName;
        }
        else if (typeName == "array") 
        {
            advance();
            return parseArrayType();
        }
        else 
        {
            error("Неизвестный тип: " + typeName);
            advance();
            return "unknown";
        }
    }
    else 
    {
        error("Ожидался тип");
        advance();
        return "unknown";
    }
}


std::string Parser::parseArrayType() 
{
    expect(TokenType::LBRACKET, "Ожидался символ '[' после 'array'");
    int lowerBound, upperBound;
    if (currentToken.type == TokenType::NUMBER) 
    {
        std::string numberLexeme = currentToken.lexeme;  
        lowerBound = std::stoi(numberLexeme);
        advance();
    }
    else 
    {
        expect(TokenType::NUMBER, "Ожидалось число  в диапазоне массива");
    }

    expect(TokenType::DOTDOT, "Ожидались две точки '..' в диапазоне массива");
    

    if (currentToken.type == TokenType::NUMBER) 
    {
        std::string numberLexeme = currentToken.lexeme;  
        upperBound = std::stoi(numberLexeme);
        advance();
    }
    else 
    {
        expect(TokenType::NUMBER, "Ожидалось число  в диапазоне массива");
    }
    expect(TokenType::RBRACKET, "Ожидался символ ']' после диапазона массива");

    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'of' после диапазона массива");
    
    std::string elementType = parseType();

    //ArrayTypeInfo arrayInfo{ lowerBound, upperBound, elementType };
    //semanticAnalyzer.declareVariable(arrayName, "array", arrayInfo, line, column);
    
    return "array";
}





void Parser::parseProcedure() 
{
    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'procedure'");
    expect(TokenType::IDENTIFIER, "Ожидалось имя процедуры");

    if (currentToken.type == TokenType::LPAREN) 
    {
        advance();
        parseParameterList();
        expect(TokenType::RPAREN, "Ожидалась ')' после списка параметров");
    }

    expect(TokenType::SEMICOLON, "Ожидалась точка с запятой после объявления процедуры");
    parseVariableDeclarations();

    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'begin' (начало тела процедуры)");
    parseStatementSequence();
    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'end' (конец тела процедуры)");
    expect(TokenType::SEMICOLON, "Ожидалась точка с запятой после тела процедуры");
}

void Parser::parseParameterList() 
{
    
    if (currentToken.type == TokenType::IDENTIFIER) 
    {
        

        while (true) 
        {
            while (true) 
            {
                expect(TokenType::IDENTIFIER, "Ожидался идентификатор параметра");
                if (currentToken.type == TokenType::COMMA) 
                {
                    advance();
                }
                else
                {
                    break;
                }
            }
            expect(TokenType::COLON, "Ожидался двоеточие после списка параметров");
            parseType();

            if (currentToken.type == TokenType::SEMICOLON) 
            {
                advance();
            }
            else 
            {
                break;
            }
        }
    }
}


void Parser::parseStatementSequence() 
{

    parseStatement();
    while (currentToken.type == TokenType::SEMICOLON) 
    {
        advance();
        parseStatement();
    }
}


void Parser::parseStatement() 
{
    if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "end") 
    {
        return;
    }


    if (currentToken.type == TokenType::IDENTIFIER) 
    {
        Token lookahead = lexer.getNextToken();
        lexer.ungetToken(lookahead);

        if (lookahead.type == TokenType::ASSIGN || lookahead.type == TokenType::LBRACKET) 
        {
            parseAssignment();
        }
        else if (lookahead.type == TokenType::LPAREN || lookahead.type == TokenType::SEMICOLON) 
        {
            parseProcedureCall();
        }
        else 
        {
            error("Неизвестный оператор");
            synchronize({ TokenType::SEMICOLON, TokenType::KEYWORD, TokenType::DOT });
        }
    }
    else if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "begin") 
    {
        parseCompoundStatement();
    }
    else 
    {
        error("Ожидалось начало оператора");
        synchronize({ TokenType::SEMICOLON, TokenType::KEYWORD, TokenType::DOT });
    }
}

void Parser::parseAssignment() 
{
    parseVariable();
    expect(TokenType::ASSIGN, "Ожидался оператор присваивания ':='");
    parseExpression();
}

void Parser::parseProcedureCall() 
{
    expect(TokenType::IDENTIFIER, "Ожидалось имя процедуры");
    if (currentToken.type == TokenType::LPAREN) 
    {
        advance();
        while (currentToken.type != TokenType::RPAREN && currentToken.type != TokenType::END_OF_FILE) {
            advance();
        }
        expect(TokenType::RPAREN, "Ожидалась ')' после списка параметров");
    }
}

void Parser::parseCompoundStatement() 
{
    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'begin'");
    parseStatementSequence();
    expect(TokenType::KEYWORD, "Ожидалось ключевое слово 'end'");
}

void Parser::parseVariable() 
{
    expect(TokenType::IDENTIFIER, "Ожидался идентификатор переменной");
    while (currentToken.type == TokenType::LBRACKET) 
    {
        advance();
        parseExpression();
        expect(TokenType::RBRACKET, "Ожидалась ']' после индекса массива");
    }
}

void Parser::parseExpression() 
{
    while (currentToken.type != TokenType::SEMICOLON && currentToken.type != TokenType::RPAREN &&
        currentToken.type != TokenType::RBRACKET && currentToken.type != TokenType::END_OF_FILE) 
    {
        advance();
    }
}

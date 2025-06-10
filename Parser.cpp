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
    std::cerr << "������ � ������ " << currentToken.line
        << ", ������� " << currentToken.column
        << ": " << message
        << " (������� �����: '" << currentToken.lexeme
        << "', ���: " << static_cast<int>(currentToken.type) << ")"
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


// ����� �����
void Parser::parseProgram() 
{
    if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "program") 
    {
        advance();
        expect(TokenType::IDENTIFIER, "��������� ��� ���������");
        expect(TokenType::SEMICOLON, "��������� ����� � ������� ����� ����� ���������");
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
            expect(TokenType::KEYWORD, "��������� �������� ����� 'end' (����� ���������)");
            expect(TokenType::DOT, "��������� ����� � ����� ���������");
        }
    }
    else {
        error("��������� �������� ����� 'program'");
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
            //expect(TokenType::SEMICOLON, "��������� ����� � ������� ����� ���������� ����������");
        }
    }
}


void Parser::parseVariableDeclaration() 
{
    std::vector<std::string> varNames;
    
    if (currentToken.type == TokenType::IDENTIFIER) 
    {
        varNames.push_back(currentToken.lexeme);  // ��������� �������, ������ ���� ��� �������������
        advance();
        // ���������� identLexeme ������
    }
    else 
    {
        error("�������� �������������");
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
            error("�������� �������������");
            advance();
        }
    }

        
    expect(TokenType::COLON, "�������� ��������� ����� ������ ���������������");

    std::string typeName = parseType();

    expect(TokenType::SEMICOLON, "��������� ����� � ������� ����� ���������� ����������");
    
    for (const auto& name : varNames) 
    {
        if (!semanticAnalyzer.declareVariable(name, typeName, std::nullopt, currentToken.line, currentToken.column)) 
        {
            error("��������� ���������� ����������: " + name);
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
            error("����������� ���: " + typeName);
            advance();
            return "unknown";
        }
    }
    else 
    {
        error("�������� ���");
        advance();
        return "unknown";
    }
}


std::string Parser::parseArrayType() 
{
    expect(TokenType::LBRACKET, "�������� ������ '[' ����� 'array'");
    int lowerBound, upperBound;
    if (currentToken.type == TokenType::NUMBER) 
    {
        std::string numberLexeme = currentToken.lexeme;  
        lowerBound = std::stoi(numberLexeme);
        advance();
    }
    else 
    {
        expect(TokenType::NUMBER, "��������� �����  � ��������� �������");
    }

    expect(TokenType::DOTDOT, "��������� ��� ����� '..' � ��������� �������");
    

    if (currentToken.type == TokenType::NUMBER) 
    {
        std::string numberLexeme = currentToken.lexeme;  
        upperBound = std::stoi(numberLexeme);
        advance();
    }
    else 
    {
        expect(TokenType::NUMBER, "��������� �����  � ��������� �������");
    }
    expect(TokenType::RBRACKET, "�������� ������ ']' ����� ��������� �������");

    expect(TokenType::KEYWORD, "��������� �������� ����� 'of' ����� ��������� �������");
    
    std::string elementType = parseType();

    //ArrayTypeInfo arrayInfo{ lowerBound, upperBound, elementType };
    //semanticAnalyzer.declareVariable(arrayName, "array", arrayInfo, line, column);
    
    return "array";
}





void Parser::parseProcedure() 
{
    expect(TokenType::KEYWORD, "��������� �������� ����� 'procedure'");
    expect(TokenType::IDENTIFIER, "��������� ��� ���������");

    if (currentToken.type == TokenType::LPAREN) 
    {
        advance();
        parseParameterList();
        expect(TokenType::RPAREN, "��������� ')' ����� ������ ����������");
    }

    expect(TokenType::SEMICOLON, "��������� ����� � ������� ����� ���������� ���������");
    parseVariableDeclarations();

    expect(TokenType::KEYWORD, "��������� �������� ����� 'begin' (������ ���� ���������)");
    parseStatementSequence();
    expect(TokenType::KEYWORD, "��������� �������� ����� 'end' (����� ���� ���������)");
    expect(TokenType::SEMICOLON, "��������� ����� � ������� ����� ���� ���������");
}

void Parser::parseParameterList() 
{
    
    if (currentToken.type == TokenType::IDENTIFIER) 
    {
        

        while (true) 
        {
            while (true) 
            {
                expect(TokenType::IDENTIFIER, "�������� ������������� ���������");
                if (currentToken.type == TokenType::COMMA) 
                {
                    advance();
                }
                else
                {
                    break;
                }
            }
            expect(TokenType::COLON, "�������� ��������� ����� ������ ����������");
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
            error("����������� ��������");
            synchronize({ TokenType::SEMICOLON, TokenType::KEYWORD, TokenType::DOT });
        }
    }
    else if (currentToken.type == TokenType::KEYWORD && currentToken.lexeme == "begin") 
    {
        parseCompoundStatement();
    }
    else 
    {
        error("��������� ������ ���������");
        synchronize({ TokenType::SEMICOLON, TokenType::KEYWORD, TokenType::DOT });
    }
}

void Parser::parseAssignment() 
{
    parseVariable();
    expect(TokenType::ASSIGN, "�������� �������� ������������ ':='");
    parseExpression();
}

void Parser::parseProcedureCall() 
{
    expect(TokenType::IDENTIFIER, "��������� ��� ���������");
    if (currentToken.type == TokenType::LPAREN) 
    {
        advance();
        while (currentToken.type != TokenType::RPAREN && currentToken.type != TokenType::END_OF_FILE) {
            advance();
        }
        expect(TokenType::RPAREN, "��������� ')' ����� ������ ����������");
    }
}

void Parser::parseCompoundStatement() 
{
    expect(TokenType::KEYWORD, "��������� �������� ����� 'begin'");
    parseStatementSequence();
    expect(TokenType::KEYWORD, "��������� �������� ����� 'end'");
}

void Parser::parseVariable() 
{
    expect(TokenType::IDENTIFIER, "�������� ������������� ����������");
    while (currentToken.type == TokenType::LBRACKET) 
    {
        advance();
        parseExpression();
        expect(TokenType::RBRACKET, "��������� ']' ����� ������� �������");
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

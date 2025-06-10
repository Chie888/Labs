#ifndef SYMBOLTABLE_H
#define SYMBOLTABLE_H

#include <string>
#include <unordered_map>
#include <vector>
#include <optional>

enum class SymbolKind 
{
    Variable,
    Procedure,
    Type,
    Parameter
};

struct ArrayTypeInfo 
{
    int lowerBound;
    int upperBound;
    std::string elementType;
};

struct ParameterInfo
{
    int parameterPosotion;
    std::string parameterName;
    std::string parameterType;
};

struct Symbol 
{
    std::string name;
    SymbolKind kind{};
    std::string typeName;
    std::optional<ArrayTypeInfo> arrayInfo; 
    std::vector<ParameterInfo> parameterInfo; 
    int scopeLevel{};
};

using SymbolTable = std::unordered_map<std::string, Symbol>;

class SymbolTableStack 
{
private:
    std::vector<SymbolTable> stack;

public:
    void enterScope();
    void exitScope();
    bool addSymbol(const Symbol& sym);
    std::optional<Symbol> findSymbol(const std::string& name) const;
    int getScopeLevel() const;
};

#endif 

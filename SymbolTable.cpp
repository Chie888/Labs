#include "SymbolTable.h"

void SymbolTableStack::enterScope() 
{
    stack.emplace_back();  
}

void SymbolTableStack::exitScope() 
{
    if (!stack.empty()) 
    {
        stack.pop_back();
    }
}

bool SymbolTableStack::addSymbol(const Symbol& sym) 
{
    if (stack.empty()) 
    {
        enterScope();
    }
    SymbolTable& current = stack.back();
    if (current.find(sym.name) != current.end()) 
    {
        return false;
    }
    Symbol symCopy = sym;
    symCopy.scopeLevel = getScopeLevel();
    current[sym.name] = symCopy;
    return true;
}

std::optional<Symbol> SymbolTableStack::findSymbol(const std::string& name) const 
{
    for (auto it = stack.rbegin(); it != stack.rend(); ++it) 
    {
        auto found = it->find(name);
        if (found != it->end()) 
        {
            return found->second;
        }
    }
    return std::nullopt; 
}

int SymbolTableStack::getScopeLevel() const 
{
    return static_cast<int>(stack.size());
}

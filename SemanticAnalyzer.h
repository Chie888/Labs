#ifndef SEMANTICANALYZER_H
#define SEMANTICANALYZER_H

#include "SymbolTable.h"
#include "ErrorTable.h"
#include <optional>
#include <vector>
#include <string>

class SemanticAnalyzer 
{
private:
    SymbolTableStack symbolTables;
    ErrorTable& errorTable;

public:
    explicit SemanticAnalyzer(ErrorTable& errors);

    void enterScope();
    void exitScope();

    bool declareVariable(const std::string& name, const std::string& typeName,
        const std::optional<ArrayTypeInfo>& arrayInfo = std::nullopt,
        int line = 0, int column = 0);

    bool declareProcedure(const std::string& name,
        const std::vector<ParameterInfo>& parameters = {},
        int line = 0, int column = 0);

    bool checkIdentifier(const std::string& name, int line = 0, int column = 0);

    bool checkTypeCompatibility(const std::string& leftType,
        const std::string& rightType, const std::string& operation, int line, int column);

    bool checkProcedureCall(const std::string& name,
        const std::vector<std::string>& argumentTypes, int line, int column);

    bool checkArrayIndex(const std::string& arrayName,
        int index, int line, int column);
};


#endif 

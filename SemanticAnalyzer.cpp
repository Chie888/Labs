#include "SemanticAnalyzer.h"
#include "ErrorTable.h"


SemanticAnalyzer::SemanticAnalyzer(ErrorTable& errors) : errorTable(errors) 
{
    symbolTables.enterScope(); // ���������� ������� ���������
}


void SemanticAnalyzer::enterScope() 
{
    symbolTables.enterScope();
}


void SemanticAnalyzer::exitScope() 
{
    symbolTables.exitScope();
}


bool SemanticAnalyzer::declareVariable(const std::string& name, const std::string& typeName,
    const std::optional<ArrayTypeInfo>& arrayInfo, int line, int column)
{
    Symbol sym{ name, SymbolKind::Variable, typeName, arrayInfo, {},
        symbolTables.getScopeLevel()};
    if (!symbolTables.addSymbol(sym)) 
    {
        errorTable.addError(line, column, "���������� '" + name + 
            "' ��� ��������� � ������� ������� ���������");
        return false;
    }

    return true;
}


bool SemanticAnalyzer::declareProcedure(const std::string& name, 
    const std::vector<ParameterInfo>& parameters, int line, int column)
{
    Symbol sym{ name, SymbolKind::Procedure, "", std::nullopt, {},
        symbolTables.getScopeLevel()};
    if (!symbolTables.addSymbol(sym)) 
    {
        errorTable.addError(line, column, "��������� '" + name + 
            "' ��� ��������� � ������� ������� ���������");
        return false;
    }

    return true;
}


bool SemanticAnalyzer::checkIdentifier(const std::string& name, int line, int column)
{
    auto sym = symbolTables.findSymbol(name);
    if (!sym.has_value()) 
    {
        errorTable.addError(line, column, "������������� '" + name + "' �� ��������");
        return false;
    }
    return true;
}


bool SemanticAnalyzer::checkTypeCompatibility(const std::string& leftType, const std::string& rightType,
    const std::string& operation, int line, int column)
{
    // ������ ������� ��������: ���� ������ ���������
    if (leftType != rightType) 
    {
        errorTable.addError(line, column, "������������� ���� ��� �������� '" + operation + 
            "': " + leftType + " � " + rightType);
        return false;
    }
    return true;
}


bool SemanticAnalyzer::checkProcedureCall(const std::string& name,
    const std::vector<std::string>& argumentTypes, int line, int column)
{
    auto symOpt = symbolTables.findSymbol(name);
    if (!symOpt.has_value()) 
    {
        errorTable.addError(line, column, "��������� '" + name + "' �� ���������");
        return false;
    }

    const Symbol& sym = symOpt.value();
    if (sym.kind != SymbolKind::Procedure) 
    {
        errorTable.addError(line, column, "'" + name + "' �� �������� ����������");
        return false;
    }

    if (sym.parameterInfo.size() != argumentTypes.size()) 
    {
        errorTable.addError(line, column, "�������� ���������� ���������� � ������ ��������� '" + name + "'");
        return false;
    }

    for (size_t i = 0; i < argumentTypes.size(); ++i) 
    {
        if (sym.parameterInfo[i].parameterType != argumentTypes[i]) 
        {
            errorTable.addError(line, column, "������������� ��� ��������� " + std::to_string(i + 1) 
                + " � ������ ��������� '" + name + "'");
            return false;
        }
    }

    return true;
}


bool SemanticAnalyzer::checkArrayIndex(const std::string& arrayName, 
    int index, int line, int column)
{
    auto symOpt = symbolTables.findSymbol(arrayName);
    if (!symOpt.has_value()) 
    {
        errorTable.addError(line, column, "������ '" + arrayName + "' �� ��������");
        return false;
    }

    const Symbol& sym = symOpt.value();
    if (!sym.arrayInfo.has_value()) 
    {
        errorTable.addError(line, column, "'" + arrayName + "' �� �������� ��������");
        return false;
    }

    const ArrayTypeInfo& arrInfo = sym.arrayInfo.value();
    if (index < arrInfo.lowerBound || index > arrInfo.upperBound) 
    {
        errorTable.addError(line, column, "������ " + std::to_string(index) 
            + " ������� �� ������� ������� '" + arrayName + "'");
        return false;
    }

    return true;
}

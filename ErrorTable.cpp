#include "ErrorTable.h"
#include <iostream>


void ErrorTable::addError(int line, int column, const std::string& message) 
{
    if (!message.empty()) 
    {  
        errors.emplace_back(line, column, message);
    }
    else 
    {
        errors.emplace_back(line, column, "����������� ������");
    }
}


void ErrorTable::printErrors() const 
{
    if (errors.empty()) 
    {
        std::cout << "������ ���." << std::endl;
        return;
    }
    for (const auto& err : errors) 
    {
        std::cout << "������ � ������ " << err.line
            << ", ������� " << err.column
            << ": " << err.message << std::endl;
    }
}


bool ErrorTable::hasErrors() const 
{
    return !errors.empty();
}
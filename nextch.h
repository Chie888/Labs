#pragma once
#include <fstream>
#include <string>


class ErrorTable;


class InputModule 
{
public:
	InputModule(const std::string& filename, ErrorTable& errorTable);
	char nextch();
	int getLine() const;
	int getColumn() const;
	bool isEOF() const;
	~InputModule();

private:
	std::ifstream inputFile;
	std::ofstream outputCodesFile;
	int line;
	int column;
	bool eof;
	ErrorTable& errors;
};
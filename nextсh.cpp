#include "nextch.h"
#include "ErrorTable.h"


InputModule::InputModule(const std::string& filename, ErrorTable& errorTable)
	: inputFile(filename, std::ios::binary), errors(errorTable), line(1), column(0), eof(false)
{
	if (!inputFile.is_open())
	{
		errors.addError(0, 0, "Не удалось открыть файл");
		eof = true;
	}

	outputCodesFile.open("codes.txt", std::ios::out);
	if (!outputCodesFile.is_open())
	{
		errors.addError(0, 0, "Не удалось открыть файл для записи кодов символов");
	}
}


InputModule::~InputModule()
{
	if (inputFile.is_open()) inputFile.close();
	if (outputCodesFile.is_open()) outputCodesFile.close();
}


char InputModule::nextch() 
{
	char ch;
	if (!inputFile.get(ch)) 
	{
		eof = true;
		return '\0';
	}

	unsigned char uch = static_cast<unsigned char>(ch);
	if (uch > 127 && (uch < 0xC0 || uch > 0xFF) && ch != '\0') 
	{
		errors.addError(line, column, "Недопустимый символ");
	}

	if (outputCodesFile.is_open())
	{
		unsigned char uch = static_cast<unsigned char>(ch);
		outputCodesFile << static_cast<int>(uch) << "\n";
	}

	if (ch == '\n') 
	{
		line++;
		column = 0;
	}
	else 
	{
		column++;
	}

	return ch;
}


int InputModule::getLine() const 
{
	return line;
}


int InputModule::getColumn() const 
{
	return column;
}


bool InputModule::isEOF() const 
{
	return eof;
}
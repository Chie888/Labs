#pragma once
#include <string>
#include <vector>

struct ErrorRecord 
{
	int line;
	int column;
	std::string message;


	ErrorRecord(int l, int c, const std::string& msg): line(l), column(c), message(msg) 
	{
	}
};


class ErrorTable 
{
public:
	void addError(int line, int column, const std::string& message);
	void printErrors() const;
	bool hasErrors() const;

private:
	std::vector<ErrorRecord> errors;
};
#ifndef ENTRY_H
#define ENTRY_H

#include <string>
#include <utils.h>

using namespace std;

class Entry
{
public:
    Entry(int line, int indexInLine, string text, string path);
    int getLine();
    int getIndexInLine();
    string getText();
    string toString();
    string getPath();

private:
    int line;
    int indexInLine;
    string text;
    string path;
};

#endif // ENTRY_H

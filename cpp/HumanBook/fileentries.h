#ifndef FILEENTRIES_H
#define FILEENTRIES_H

#include <vector>
#include <entry.h>
#include <string>
#include <utils.h>

using namespace std;

class FileEntries
{
public:
    FileEntries(string path);
    ~FileEntries();
    vector<Entry> *getEntries();
    void addEntry(int line, int indexInLine, const string &text);
    string toString();
    string getPath();

private:
    vector<Entry> *entries;
    string path;
};

#endif // FILEENTRIES_H

#ifndef URLENTRIES_H
#define URLENTRIES_H

#include <vector>
#include <fileentries.h>
#include <string>

class UrlEntries
{
public:
    UrlEntries();
    ~UrlEntries();
    vector<FileEntries> *getEntries();
    void addEntry(FileEntries &entries);
    string getUrl();
    void setUrl(string &url);

private:
    vector<FileEntries> *entries;
    string url;
};

#endif // URLENTRIES_H

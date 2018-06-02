#include "urlentries.h"

UrlEntries::UrlEntries()
{
    entries = new vector<FileEntries>();
}

UrlEntries::~UrlEntries() {
    delete entries;
}

vector<FileEntries> *UrlEntries::getEntries() {
    return entries;
}

void UrlEntries::addEntry(FileEntries &entries) {
    this->entries->push_back(entries);
}

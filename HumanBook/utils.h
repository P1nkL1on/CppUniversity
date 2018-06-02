#ifndef UTILS_H
#define UTILS_H

#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <stdarg.h>

using namespace std;

class Utils
{
public:
    Utils();
    static void stringTokenizer(string* line, char delimiter, vector<string> *tokens);
    static string readFile(const string &path);
    static void replaceAll(string &str, const string &from, const string &to);
    static string format(const string fmt, ...);
};

#endif // UTILS_H

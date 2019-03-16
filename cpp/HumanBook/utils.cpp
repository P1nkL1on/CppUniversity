#include "utils.h"
#include <bits/stdc++.h>

Utils::Utils()
{

}

void Utils::stringTokenizer(string *line, char delimiter, vector<string> *tokens) {
    stringstream stream(*line);
    string temp;
    while (getline(stream, temp, delimiter)) {
        tokens->push_back(temp);
    }
}

string Utils::readFile(const string &path) {
    ifstream file(path);
    stringstream buffer;
    buffer << file.rdbuf();
    return buffer.str();
}

void Utils::replaceAll(string &str, const string &from, const string &to) {
    size_t index = 0;
    while (true) {
        index = str.find(from, index);
        if (index == string::npos) break;

        str.replace(index, from.size(), to);

        index += to.size();
    }
}

string Utils::format(const std::string fmt, ...) {
    int size = ((int)fmt.size()) * 2 + 50;   // Use a rubric appropriate for your code
    std::string str;
    va_list ap;
    while (1) {     // Maximum two passes on a POSIX system...
        str.resize(size);
        va_start(ap, fmt);
        int n = vsnprintf((char *)str.data(), size, fmt.c_str(), ap);
        va_end(ap);
        if (n > -1 && n < size) {  // Everything worked
            str.resize(n);
            return str;
        }
        if (n > -1)  // Needed size returned
            size = n + 1;   // For null char
        else
            size *= 2;      // Guess at a larger size (OS specific)
    }
    return str;
}

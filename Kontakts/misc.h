#ifndef MISC_H
#define MISC_H

#include "QString"
#include "QVector"
class Misc
{
public:
    static QString viewTag (QString line, QString& text);
    static QVector<QString> readFile(const QString& path);
};

#endif // MISC_H

#ifndef WRITABLE_H
#define WRITABLE_H
#include "QString"
#include "QVector"

class Writable
{
    virtual QVector<QString> WriteToFileStrings () = 0;
    virtual QString Trace () = 0;
};

#endif // WRITABLE_H

#ifndef GROUP_H
#define GROUP_H

#include "writable.h"
#include "QVector"



class Contact
{
private:
    QString fio, phone;
public:
    Contact();
    Contact (QString fio, QString phone);
    QString ToLine();
    QString GetSaveFIO();
    QString GetSavePhone();
};

class Group : public Writable
{
public:
    Group(QVector<QString> fromLines);
    ~Group();
    QVector<QString> WriteToFileStrings () override;
    QString Trace() override;
private:
    QString groupName;
    QVector<Contact*> contacts;
};

#endif // GROUP_H

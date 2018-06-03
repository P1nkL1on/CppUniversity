#ifndef EVENT_H
#define EVENT_H

#include "group.h"
//class Event : public Writable
//{
//public:
//    Event();
//};


class Event : public Writable
{
public:
    Event(QVector<QString> fromLines, bool fromFile);
    ~Event();
    QVector<QString> WriteToFileStrings () override;
    QString Trace() override;
private:
    QString eventName, date, description;
    QVector<QString> pointedMembers;
};
#endif // EVENT_H

#include "event.h"
#include "misc.h"
#include "qdebug.h"


//Event::Event(QVector<QString> fromLines)
//{

//}

//Event::~Event()
//{
//}

Event::Event(QVector<QString> fromLines, bool fromFile)
{
    if (fromFile){
        QString tag = "none", text="", n = "", t = "";
        //int state = 0;

        for (int i = 0; i < fromLines.length(); i++){
            tag = Misc::viewTag(fromLines[i], text);
            qDebug () << tag << text;
            pointedMembers = QVector<QString>();

            if (tag == "event_name")
                eventName = text;
            if (tag == "date")
                date = text;
            if (tag == "description")
                description = text;
            if (tag == "member_depend" || tag == "group_depend")
                pointedMembers << text;
        }
    }else{
//        groupName = fromLines[0].remove(fromLines[0].length() - 1); // <group_name>
//        for (int i = 1; i < fromLines.length(); i++){
//            contacts << new Contact(fromLines[i].mid(0, fromLines[i].indexOf(", тел. ")).trimmed(),
//                                    fromLines[i].remove(0, fromLines[i].indexOf(", тел. ") + 7).trimmed());
//        }
    }
    int X = 10;
}

Event::~Event()
{

}

QVector<QString> Event::WriteToFileStrings()
{
    QVector<QString> res = QVector<QString>();
    res << "<event_name>" + eventName << "<date>" + date << "<description>" + description;
    for (int i  = 0; i < pointedMembers.length(); i++)
        res << ((pointedMembers[i].indexOf(", тел.")>0)?("<member_depend>"):("<group_depend>"))+ pointedMembers[i];
    return res;
}

QString Event::Trace()
{
    QString res = eventName + ":\r\n" + date + "\r\n" + description + "\r\nУчастники:\r\n";
    for (int i  = 0; i < pointedMembers.length(); i++)
        res += pointedMembers[i] + "\r\n";
    return res;
}

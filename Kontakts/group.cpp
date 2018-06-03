#include "group.h"

#include "misc.h"
#include "qdebug.h"

Group::Group(QVector<QString> fromLines)
{
    QString tag = "none", text="", n = "", t = "";
    int state = 0;

    for (int i = 0; i < fromLines.length(); i++){
        tag = Misc::viewTag(fromLines[i], text);
        qDebug () << tag << text;

        if (tag == "group_name")
            groupName = text;
        if (tag == "group_member")
        {if (state == 2){state = 0; contacts << new Contact(n,t);}}
        if (tag == "name")
        { n = text; state++;}
        if (tag == "phone")
        { t = text; state++;}
    }
    if (state == 2)
        contacts << new Contact(n,t);
}

Group::~Group()
{
    for (int i = 0; i < contacts.length(); i++)
        delete contacts[i];
}

QVector<QString> Group::WriteToFileStrings ()
{
    QVector<QString> res = QVector<QString>();
    res << "<group_name>" + groupName;
    for (int i  = 0; i < contacts.length(); i++)
        res << "<group_member>" << contacts[i]->GetSaveFIO() << contacts[i]->GetSavePhone();
}

QString Group::Trace()
{
    QString res = groupName + ":\r\n";
    for (int i  = 0; i < contacts.length(); i++)
        res += contacts[i]->ToLine() + "\r\n";
    return res;
}

Contact::Contact(QString fio, QString phone)
{
    this->fio = fio;
    this->phone = phone;
}

QString Contact::ToLine()
{
    return QString(fio + ", тел. " + phone);
}

QString Contact::GetSaveFIO()
{
    return "<name>" + fio;
}

QString Contact::GetSavePhone()
{
    return "<phone>" + phone;
}

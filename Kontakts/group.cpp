#include "group.h"

#include "misc.h"
#include "qdebug.h"

Group::Group(QVector<QString> fromLines, bool fromFile)
{
    if (fromFile){
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
    }else{
        groupName = fromLines[0].remove(fromLines[0].length() - 1); // <group_name>
        for (int i = 1; i < fromLines.length(); i++){
            contacts << new Contact(fromLines[i].mid(0, fromLines[i].indexOf(", тел. ")).trimmed(),
                                    fromLines[i].remove(0, fromLines[i].indexOf(", тел. ") + 7).trimmed());
        }
    }
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
    return res;
}

QString Group::Trace()
{
    QString res = groupName + ":\r\n";
    for (int i  = 0; i < contacts.length(); i++)
        res += contacts[i]->ToLine() + "\r\n";
    return res;
}

QVector<QString> Group::contacNames()
{
    QVector<QString> res = QVector<QString>();
    for (int i = 0; i < contacts.length(); i++)
        res << contacts[i]->ToLine();
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

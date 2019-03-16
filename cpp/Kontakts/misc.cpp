#include "misc.h"

#include "QFile"
#include "QTextStream"
#include "QDebug"
#include "QVector"

QString Misc::viewTag(QString line, QString &text)
{
    //<name>Димон из гаражей
    if (line.indexOf('<') != 0) return "ERROR";
    QString line2 = line;
    text = line2.remove(0, line.indexOf('>') + 1);
    return line.mid(1, line.indexOf('>') - 1);
}

QVector<QString> Misc::readFile(const QString &path) {


    QFile file(path);
    if (!file.open(QIODevice::ReadOnly))
        return QVector<QString>();


    // reading file
    QTextStream stream(&file);
    QVector<QString> lines = QVector<QString>();

    while (!stream.atEnd())
        lines <<stream.readLine();

    return lines;
}

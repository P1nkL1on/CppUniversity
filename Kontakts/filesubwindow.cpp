#include "filesubwindow.h"
#include "QColor"
#include "qdebug.h"

FileSubWindow::FileSubWindow(QWidget *parent, const QString &text): QMdiSubWindow(parent)
{
    textEdit = new QTextEdit();
    textEdit->setPlainText(text);
    setWidget(textEdit);
}

FileSubWindow::~FileSubWindow() {
     textEdit->~QTextEdit();
}

QString FileSubWindow::getTextForSave() {
    Group g = Group(textEdit->toPlainText().split(QRegExp("[\n]"),QString::SkipEmptyParts).toVector(), false);
    QVector<QString> qs= g.WriteToFileStrings(); QString res = "";
    for (int i = 0; i < qs.length(); i++)res += qs[i]+"\n";
    return res;
}

void FileSubWindow::setType(const bool isEvent)
{
    isevent = isEvent;
    qDebug() << ((isEvent)? "The file is event;" : "The file is group");
    textEdit->setTextBackgroundColor((isEvent)? QColor(240,200,200) : QColor(200,200,240));
}

void FileSubWindow::addToText(const QString what)
{
    textEdit->append(what);
}

bool FileSubWindow::isGroup() const
{
    return !isevent;
}

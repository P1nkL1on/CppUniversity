#include "filesubwindow.h"

FileSubWindow::FileSubWindow(QWidget *parent, const QString &text): QMdiSubWindow(parent)
{
    textEdit = new QTextEdit();
    textEdit->setPlainText(text);
    setWidget(textEdit);
    g = nullptr;
    e = nullptr;
}

FileSubWindow::~FileSubWindow() {
     textEdit->~QTextEdit();
}

QString FileSubWindow::getTextForSave() {
    return textEdit->toPlainText();
}

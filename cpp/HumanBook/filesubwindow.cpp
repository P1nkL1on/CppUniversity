#include "filesubwindow.h"

FileSubWindow::FileSubWindow(QWidget *parent, const QString &text): QMdiSubWindow(parent)
{
    textEdit = new QTextEdit();
    textEdit->setPlainText(text);
    setWidget(textEdit);
    isEvent = false;
}

FileSubWindow::~FileSubWindow() {
     textEdit->~QTextEdit();
}

QString FileSubWindow::getText() const {
    return textEdit->toPlainText();
}

void FileSubWindow::SetType(const bool isEvent)
{
    this->isEvent = isEvent;
}

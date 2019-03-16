#ifndef FILESUBWINDOW_H
#define FILESUBWINDOW_H

#include <QObject>
#include <QWidget>
#include <QMdiSubWindow>
#include <QTextEdit>
#include <QString>
#include "group.h"
#include "event.h"

class FileSubWindow : public QMdiSubWindow
{
    Q_OBJECT

public:
    FileSubWindow(QWidget *parent, const QString &text = nullptr);
    ~FileSubWindow();
    QString getTextForSave();
    QString getTextEventForSave();
    void setType (const bool isEvent);
    void addToText(const QString what);
    bool isGroup() const;
private:
    QTextEdit *textEdit;
    bool isevent;
};

#endif // FILESUBWINDOW_H

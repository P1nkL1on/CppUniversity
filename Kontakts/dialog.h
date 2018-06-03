#ifndef DIALOG_H
#define DIALOG_H

#include "group.h"
#include <QDialog>

namespace Ui {
class Dialog;
}

class Dialog : public QDialog
{
    Q_OBJECT

public:
    explicit Dialog(QWidget *parent = 0);
    ~Dialog();
    static QString getMember();

private slots:
    void on_pushButton_clicked();

private:
    Ui::Dialog *ui;
    QString getMemberPath();
    QString filePath;
    Group* currentG;
};

#endif // DIALOG_H

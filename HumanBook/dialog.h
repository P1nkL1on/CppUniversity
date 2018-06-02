#ifndef DIALOG_H
#define DIALOG_H

#include <QDialog>
#include <QString>
#include <QVector>

using namespace std;

namespace Ui {
class Dialog;
}

class Dialog : public QDialog
{
    Q_OBJECT

public:
    explicit Dialog(QWidget *parent = 0);
    ~Dialog();
    static QString getUrl();

private:
    Ui::Dialog *ui;
    QVector<QString> protocols;
    QString assembleResult();
};

#endif // DIALOG_H

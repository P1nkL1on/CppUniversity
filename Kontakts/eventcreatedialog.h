#ifndef EVENTCREATEDIALOG_H
#define EVENTCREATEDIALOG_H

#include <QDialog>
#include <QString>
#include <QVector>

using namespace std;

namespace Ui {
class EventCreateDialog;
}
class EventCreateDialog : public QDialog
{
public:
    explicit EventCreateDialog(QWidget *parent = 0);
    ~EventCreateDialog();
private:
    Ui::Dialog *ui;

};

#endif // EVENTCREATEDIALOG_H

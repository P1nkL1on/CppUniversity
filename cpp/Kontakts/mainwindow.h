#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private:
    Ui::MainWindow *ui;
    void showFileSubWindow(const QString &text = nullptr, const QString &title = nullptr, bool isGroup = false);
    void showErrorDialog(const QString &text);

private slots:
//    void createNewGroup();
//    void createNewEvent();
//    void save();
//    void saveAs();
    void on_actionOpen_triggered();
    void on_actionSave_triggered();
    void on_actionSave_as_triggered();
    void on_actionNew_group_triggered();
    void on_actionNew_event_triggered();
    void on_actionAdd_member_dependence_triggered();
};

#endif // MAINWINDOW_H

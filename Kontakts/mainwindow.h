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

//private slots:
//    void createNewGroup();
//    void createNewEvent();
//    void openFile();
//    void save();
//    void saveAs();
};

#endif // MAINWINDOW_H

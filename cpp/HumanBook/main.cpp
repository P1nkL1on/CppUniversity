#include <QApplication>
#include <QQmlApplicationEngine>
#include <QMdiArea>
#include <QMdiSubWindow>
#include <QMainWindow>
#include <mainwindow.h>
#include <QTreeWidget>
#include <QList>
#include <QTreeWidgetItem>
#include <QStringList>
#include <QLineEdit>
#include <QSizePolicy>
#include <QBrush>
#include <QColor>
#include <QHBoxLayout>
#include <QPushButton>
#include <QWidget>
#include <mainwindow.h>

void constructLayout();
void onFindBtnClick();

int main(int argc, char *argv[])
{
    QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);

    QApplication app(argc, argv);

    constructLayout();

    return app.exec();
}

void constructLayout() {
    MainWindow *mainWindow = new MainWindow();
    mainWindow->show();
}

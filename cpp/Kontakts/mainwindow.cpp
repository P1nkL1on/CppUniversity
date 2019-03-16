#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "QFileDialog"
#include "qdebug.h"
#include <filesubwindow.h>
#include "misc.h"
#include "group.h"
#include "event.h"
#include "QErrorMessage"
#include "dialog.h"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::showFileSubWindow(const QString &text, const QString &title, bool isGroup)
{
    FileSubWindow *fileSubWindow = new FileSubWindow(ui->mdi, text);
    fileSubWindow->setAttribute(Qt::WA_DeleteOnClose);
    fileSubWindow->setWindowTitle(title);
    fileSubWindow->show();
    fileSubWindow->setType(!isGroup);
}

void MainWindow::showErrorDialog(const QString &text)
{
    QErrorMessage *errorDialog = new QErrorMessage(this);
    errorDialog->setAttribute(Qt::WA_DeleteOnClose);
    errorDialog->showMessage(text);
}



void MainWindow::on_actionOpen_triggered()
{
    QString fileName = QFileDialog::getOpenFileName(this, "Open file", "/", "Text files (*.txt)");
    qDebug() << "Selected file: " << fileName;
    if (fileName.trimmed().length() != 0) {
        QVector<QString> geted = Misc::readFile(fileName);

        if (geted.length() > 0){
            if (geted[0].indexOf("group_name") == 1){
                Group g = Group(geted, true);
                showFileSubWindow(g.Trace(), fileName, true);
                return;
            }
            if (geted[0].indexOf("event_name") == 1){
                Event e = Event(geted, true);
                showFileSubWindow(e.Trace(), fileName, false);
                return;
            }
        }
        showErrorDialog("Incorrect file format!");
        return;
    }
}

void MainWindow::on_actionSave_triggered()
{
    FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdi->activeSubWindow());
    if (activeWindow == nullptr) {
        showErrorDialog("No file selected");
        return;
    }
    QFileInfo fileInfo(activeWindow->windowTitle());
    if (!(fileInfo.exists() && fileInfo.isFile())) {
        on_actionSave_as_triggered();
    } else {

        QString text ="";
        if (!activeWindow->isGroup())
        {
            text= activeWindow->getTextEventForSave();
        }else{
            text= activeWindow->getTextForSave();
        }
        QFile file(fileInfo.absoluteFilePath());
        if (file.open(QIODevice::ReadWrite | QIODevice::Truncate | QIODevice::Text)) {
            QTextStream textStram(&file);
            textStram << text;
        }
    }
}

void MainWindow::on_actionSave_as_triggered()
{
    FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdi->activeSubWindow());
    if (activeWindow == nullptr) {
        showErrorDialog("No file selected");
        return;
    }
    QString text ="";
    if (!activeWindow->isGroup())
    {
        text= activeWindow->getTextEventForSave();
    }else{
        text= activeWindow->getTextForSave();
    }
    QString path = QFileDialog::getSaveFileName(this, "Save as", "/", "All Files (*)");
    if (path.trimmed().size() == 0) return;
    QFileInfo fileInfo(path);
    if (!(fileInfo.exists() && fileInfo.isFile())) {
        QFile file(fileInfo.absoluteFilePath());
        if (file.open(QIODevice::ReadWrite | QIODevice::Truncate | QIODevice::Text)) {
            QTextStream textStram(&file);
            textStram << text;
        }
    }
    activeWindow->setWindowTitle(path);
}

void MainWindow::on_actionNew_group_triggered()
{
    showFileSubWindow("Новая группа контактов:\nКонтакт №1, тел. 1-111-222-33-33\nКонтакт №2, тел. 1-444-555-33-33\nКонтакт №3, тел. 2-33-44-55", "New group", true);
}

void MainWindow::on_actionNew_event_triggered()
{
    showFileSubWindow("Новое событие:\nВстреча нового года\n1 января сего года\nВстречаемся, кушаем оливье и смотрим обращение президента, идём на салют.\nУчастники:\n", "New event", false);
}

void MainWindow::on_actionAdd_member_dependence_triggered()
{
    FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdi->activeSubWindow());
    if (activeWindow == nullptr) {
        showErrorDialog("No file selected");
        return;
    }
    if (activeWindow->isGroup()) {
        showErrorDialog("Depencdences can only be added to event structure!");
        return;
    }
    activeWindow->addToText(Dialog::getMember());
}

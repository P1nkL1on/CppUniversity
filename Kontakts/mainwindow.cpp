#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "QFileDialog"
#include "qdebug.h"
#include <filesubwindow.h>
#include "misc.h"
#include "group.h"
#include "event.h"
#include "QErrorMessage"


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

void MainWindow::showFileSubWindow(const QString &text, const QString &title)
{
    FileSubWindow *fileSubWindow = new FileSubWindow(ui->mdi, text);
    fileSubWindow->setAttribute(Qt::WA_DeleteOnClose);
    fileSubWindow->setWindowTitle(title);
    fileSubWindow->show();
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

        Group g = Group(geted);
        showFileSubWindow(g.Trace(), fileName);
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
        QString text = activeWindow->getTextForSave();
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
    QString text = activeWindow->getTextForSave();
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

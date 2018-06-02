#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QMenu>
#include <QAction>
#include <QtGui>
#include <QMdiArea>
#include <QString>
#include <QFileDialog>
#include <iostream>
#include <utils.h>
#include <QVector>
#include <QFile>
#include <QMdiSubWindow>
#include <QErrorMessage>
#include <vector>
#include <regex>
#include <string>
#include <QFileInfo>
#include <fileentries.h>
#include <entry.h>
#include <sstream>
#include <urlentries.h>
#include <map>

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
    void showFileSubWindow(const QString &text = nullptr, const QString &title = nullptr);
    void findStringInFiles(const QString &text, const QVector<QFileInfo*> *files, vector<FileEntries> *result);
    void findStringInFile(const QString &text, QFileInfo &fileInfo, FileEntries *entries);
    void findStringInText(const QString &str, QString &text, FileEntries *entries);
    void findStringInLine(const QString &text, string &line, FileEntries *entries, int lineIndex);
    void parseFilesPaths(const QString &filesListStr, QVector<QFileInfo*> *filesList);
    bool validateFilesPathsStr(const QString &filesList);
    void showErrorDialog(const QString &text);
    void findUrlsInFiles(const QVector<QFileInfo*> *files, map<string, vector<FileEntries*>> *result);
    void findUrlsInLine(const string &line, int lineIndex, const string &path, vector<Entry> *result);
    int findFileEntriesByPath(const string &path, vector<FileEntries*> *entries);

private slots:
    void createNewDocument();
    void openFile();
    void findInFiles();
    void findInFile();
    void save();
    void saveAs();
    void findAllUrls();
};

#endif // MAINWINDOW_H

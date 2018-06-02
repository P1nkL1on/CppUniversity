#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QKeySequence>
#include <filesubwindow.h>
#include <dialog.h>

using namespace std;

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    ui->mdiArea->setViewMode(QMdiArea::TabbedView);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::showFileSubWindow(const QString &text, const QString &title) {
    FileSubWindow *fileSubWindow = new FileSubWindow(ui->mdiArea, text);
    fileSubWindow->setAttribute(Qt::WA_DeleteOnClose);
    fileSubWindow->setWindowTitle(title);
    fileSubWindow->show();
}

void MainWindow::findStringInFiles(const QString &text, const QVector<QFileInfo*> *files, vector<FileEntries> *result) {
    for (QFileInfo *fileInfo: *files) {
        FileEntries *temp = new FileEntries(fileInfo->absoluteFilePath().toStdString());
        findStringInFile(text, *fileInfo, temp);
        result->push_back(*temp);
    }
}

void MainWindow::findUrlsInFiles(const QVector<QFileInfo *> *files, map<string, vector<FileEntries*>> *result) {
    vector<Entry> *entries = new vector<Entry>();
    for (QFileInfo *fileInfo : *files) {
        QString fileText = QString::fromStdString(Utils::readFile(fileInfo->absoluteFilePath().toStdString()));

        istringstream stream(fileText.toStdString());
        int lineIndex = 0;
        string line;
        while (getline(stream, line)) {
            findUrlsInLine(line, lineIndex, fileInfo->absoluteFilePath().toStdString(), entries);
            lineIndex++;
        }


    }

    for (Entry entry : *entries) {
        if (result->find(entry.getText()) != result->end()) {
            vector<FileEntries*> fileEntries = result->at(entry.getText());
            int fileEntriesIndex = findFileEntriesByPath(entry.getPath(), &fileEntries);
            if (fileEntriesIndex != -1) {
                fileEntries[fileEntriesIndex]->addEntry(entry.getLine(), entry.getIndexInLine(), entry.getText());
            } else {
                FileEntries *newFileEntries = new FileEntries(entry.getPath());
                newFileEntries->addEntry(entry.getLine(), entry.getIndexInLine(), entry.getText());
                fileEntries.push_back(newFileEntries);
            }
        } else {
            result->insert(pair<string, vector<FileEntries*>>(entry.getText(), vector<FileEntries*>()));
            FileEntries *newFileEntries = new FileEntries(entry.getPath());
            newFileEntries->addEntry(entry.getLine(), entry.getIndexInLine(), entry.getText());
            result->at(entry.getText()).push_back(newFileEntries);
        }
    }
}

int MainWindow::findFileEntriesByPath(const string &path, vector<FileEntries*> *entries) {
    return 0;
}

void MainWindow::findUrlsInLine(const string &line, int lineIndex, const string &path, vector<Entry> *result) {
    string protocols[3] = {"ftp://", "http://", "telnet://"};
    for (string protocol : protocols) {
        int index = 0;
        int spaceIndex = 0;
        while (true) {
            index = line.find(protocol, index);
            if (index == string::npos) break;

            spaceIndex = line.find(" ", index + protocol.size());
            if (spaceIndex == string::npos) {
                spaceIndex = line.size();
            }
            string text = line.substr(index, spaceIndex - index);

            result->push_back(Entry(lineIndex, index, text, path));

            index += 6;
        }
    }
}

void MainWindow::findStringInFile(const QString &text, QFileInfo &fileInfo, FileEntries *entries) {
    QString fileText = QString::fromStdString(Utils::readFile(fileInfo.absoluteFilePath().toStdString()));
    findStringInText(text, fileText, entries);
}

void MainWindow::findStringInText(const QString &str, QString &text, FileEntries *entries) {
    istringstream stream(text.toStdString());
    int lineIndex = 0;
    string line;
    while (getline(stream, line)) {
        findStringInLine(str, line, entries, lineIndex);
        lineIndex++;
    }
}

void MainWindow::findStringInLine(const QString &text, string &line, FileEntries *entries, int lineIndex) {
    int index = 0;
    int spaceIndex = 0;
    while (true) {
        index = line.find(text.toStdString(), index);
        if (index == string::npos) break;

        spaceIndex = line.find(" ", index + text.size());
        if (spaceIndex == string::npos) {
            spaceIndex = line.size() - 1;
        }
        string text = line.substr(index, spaceIndex - index + 1);

        entries->addEntry(lineIndex, index, text);

        index += text.size();
    }
}

void MainWindow::parseFilesPaths(const QString &filesListStr, QVector<QFileInfo*> *filesList) {
    string filesListStdStr = filesListStr.toStdString();
    Utils::replaceAll(filesListStdStr, "\n", "");
    Utils::replaceAll(filesListStdStr, "\r", "");
    if (!validateFilesPathsStr(filesListStr)) return;
    vector<string> *tokens = new vector<string>();
    Utils::stringTokenizer(&filesListStdStr, ',', tokens);
    QFileInfo *temp;
    string invalidFilesStr = "Invalid files paths: ";
    int invalidFilesCount = 0;
    for (string path: *tokens) {
        temp = new QFileInfo(QString::fromStdString(path));
        if (temp->exists() && temp->isFile()) {
            filesList->push_back(temp);
        } else {
            invalidFilesCount++;
            invalidFilesStr += path + ", ";
        }
    }
    if (invalidFilesCount != 0)
        showErrorDialog(QString::fromStdString(invalidFilesStr));
}

bool MainWindow::validateFilesPathsStr(const QString &filesList) {
    if (filesList.trimmed().size() == 0) {
        showErrorDialog("Files list is blank");
        return false;
    }
    return true;
}

void MainWindow::showErrorDialog(const QString &text) {
    QErrorMessage *errorDialog = new QErrorMessage(this);
    errorDialog->setAttribute(Qt::WA_DeleteOnClose);
    errorDialog->showMessage(text);
}

//--------------------------- SLOTS ------------------------
void MainWindow::createNewDocument() {
    showFileSubWindow();
}

void MainWindow::openFile() {
    QString fileName = QFileDialog::getOpenFileName(this, "Open file", "/", "Text files (*.txt)");
    cout << "Selected file: " << fileName.toStdString() << endl;
    if (fileName.trimmed().length() != 0) {
        showFileSubWindow(QString::fromStdString(Utils::readFile(fileName.toStdString())), fileName);
    }
}

void MainWindow::findInFiles() {
    QString url = Dialog::getUrl();
    cout << "Url entered: " << url.toStdString() << endl;
    if (url.trimmed().length() != 0) {
        FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdiArea->activeSubWindow());
        if (activeWindow == nullptr) {
            showErrorDialog("No file selected");
            return;
        }
        QString filesListStr = activeWindow->getText();
        QVector<QFileInfo*> *filesList = new QVector<QFileInfo*>();
        parseFilesPaths(filesListStr, filesList);
        if (filesList->size() == 0) return;
        vector<FileEntries> *entries = new vector<FileEntries>();
        findStringInFiles(url, filesList, entries);
        string entriesStr = "";
        for (FileEntries fileEntries: *entries) {
            entriesStr += fileEntries.toString() + "\n";
        }
        showFileSubWindow(QString::fromStdString(entriesStr),
                          QString::fromStdString(Utils::format("%s_search_results", url.toStdString().c_str())));
    }
}

void MainWindow::findInFile() {
    QString url = Dialog::getUrl();
    cout << "Url entered: " << url.toStdString() << endl;
    if (url.trimmed().length() != 0) {
        FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdiArea->activeSubWindow());
        if (activeWindow == nullptr) {
            showErrorDialog("No file selected");
            return;
        }
        QString text = activeWindow->getText();
        FileEntries *entries = new FileEntries("");
        findStringInText(url, text, entries);
        string entriesStr = entries->toString() + "\n";
        showFileSubWindow(QString::fromStdString(entriesStr),
                          QString::fromStdString(Utils::format("%s_search_results", url.toStdString().c_str())));
    }
}

void MainWindow::save() {
    FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdiArea->activeSubWindow());
    if (activeWindow == nullptr) {
        showErrorDialog("No file selected");
        return;
    }
    QFileInfo fileInfo(activeWindow->windowTitle());
    if (!(fileInfo.exists() && fileInfo.isFile())) {
        saveAs();
    } else {
        QString text = activeWindow->getText();
        QFile file(fileInfo.absoluteFilePath());
        if (file.open(QIODevice::ReadWrite | QIODevice::Truncate | QIODevice::Text)) {
            QTextStream textStram(&file);
            textStram << text;
        }
    }
}

void MainWindow::saveAs() {
    FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdiArea->activeSubWindow());
    if (activeWindow == nullptr) {
        showErrorDialog("No file selected");
        return;
    }
    QString text = activeWindow->getText();
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

void MainWindow::findAllUrls() {
        FileSubWindow *activeWindow = dynamic_cast<FileSubWindow*>(ui->mdiArea->activeSubWindow());
        if (activeWindow == nullptr) {
            showErrorDialog("No file selected");
            return;
        }
        QString filesListStr = activeWindow->getText();
        QVector<QFileInfo*> *filesList = new QVector<QFileInfo*>();
        parseFilesPaths(filesListStr, filesList);
        if (filesList->size() == 0) return;
        map<string, vector<FileEntries*>> *entries = new map<string, vector<FileEntries*>>();
        findUrlsInFiles(filesList, entries);
        string resultStr = "";
        for (pair<string, vector<FileEntries*>> url : *entries) {
            resultStr += url.first + ":\n";
            for (FileEntries* entries : url.second) {
                resultStr += entries->toString();
            }
        }
        showFileSubWindow(QString::fromStdString(resultStr), "shit");
}

#include "dialog.h"
#include "ui_dialog.h"
#include "QFileDialog"
#include "QStringList"
#include "QDebug"
#include "misc.h"
#include "QComboBox"

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    ui->setupUi(this);
    currentG = nullptr;
}

Dialog::~Dialog()
{
    delete ui;
    if (currentG) delete currentG;
}

QString Dialog::getMember()
{
    Dialog dialog;
    if (dialog.exec()) {
        return dialog.getMemberPath();
    } else {
        return "";
    }
}

void Dialog::on_pushButton_clicked()
{
    QString fileName = QFileDialog::getOpenFileName(this, "Open file", "/", "Text files (*.txt)");
    qDebug() << "Load persons from file: " << fileName;
    if (fileName.trimmed().length() != 0) {
        QVector<QString> geted = Misc::readFile(fileName);
        currentG = new Group(geted, true);

        ui->comboBox->clear();
        ui->comboBox->addItems(QStringList::fromVector(currentG->contacNames()));
    }
}

QString Dialog::getMemberPath()
{
    return currentG->contacNames()[ui->comboBox->currentIndex()];
}

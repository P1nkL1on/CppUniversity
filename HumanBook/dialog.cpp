#include "dialog.h"
#include "ui_dialog.h"
#include <iostream>
#include <QComboBox>
#include <QStringList>

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    ui->setupUi(this);
    protocols.push_back("ftp://");
    protocols.push_back("http://");
    protocols.push_back("telnet://");
    protocols.push_back("file://");

    ui->comboBox->addItems(QStringList::fromVector(protocols));
}

Dialog::~Dialog()
{
    delete ui;
}

QString Dialog::getUrl() {
    Dialog dialog;
    if (dialog.exec()) {
        return dialog.assembleResult();
    } else {
        return "";
    }
}

QString Dialog::assembleResult() {
    return protocols[ui->comboBox->currentIndex()] + ui->lineEdit->text();
}

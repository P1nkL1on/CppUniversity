/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.9.1
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMdiArea>
#include <QtWidgets/QMenu>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QAction *actionNew_group;
    QAction *actionNew_event;
    QAction *actionSave;
    QAction *actionSave_as;
    QAction *actionOpen;
    QAction *actionShow_all_groups;
    QAction *actionShow_all_events;
    QAction *actionAdd_member_dependence;
    QWidget *centralWidget;
    QMdiArea *mdi;
    QMenuBar *menuBar;
    QMenu *menuFile;
    QMenu *menuShow;
    QToolBar *mainToolBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QStringLiteral("MainWindow"));
        MainWindow->resize(431, 609);
        actionNew_group = new QAction(MainWindow);
        actionNew_group->setObjectName(QStringLiteral("actionNew_group"));
        actionNew_event = new QAction(MainWindow);
        actionNew_event->setObjectName(QStringLiteral("actionNew_event"));
        actionSave = new QAction(MainWindow);
        actionSave->setObjectName(QStringLiteral("actionSave"));
        actionSave_as = new QAction(MainWindow);
        actionSave_as->setObjectName(QStringLiteral("actionSave_as"));
        actionOpen = new QAction(MainWindow);
        actionOpen->setObjectName(QStringLiteral("actionOpen"));
        actionShow_all_groups = new QAction(MainWindow);
        actionShow_all_groups->setObjectName(QStringLiteral("actionShow_all_groups"));
        actionShow_all_events = new QAction(MainWindow);
        actionShow_all_events->setObjectName(QStringLiteral("actionShow_all_events"));
        actionAdd_member_dependence = new QAction(MainWindow);
        actionAdd_member_dependence->setObjectName(QStringLiteral("actionAdd_member_dependence"));
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        mdi = new QMdiArea(centralWidget);
        mdi->setObjectName(QStringLiteral("mdi"));
        mdi->setGeometry(QRect(10, 10, 411, 541));
        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QStringLiteral("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 431, 21));
        menuFile = new QMenu(menuBar);
        menuFile->setObjectName(QStringLiteral("menuFile"));
        menuShow = new QMenu(menuBar);
        menuShow->setObjectName(QStringLiteral("menuShow"));
        MainWindow->setMenuBar(menuBar);
        mainToolBar = new QToolBar(MainWindow);
        mainToolBar->setObjectName(QStringLiteral("mainToolBar"));
        MainWindow->addToolBar(Qt::TopToolBarArea, mainToolBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QStringLiteral("statusBar"));
        MainWindow->setStatusBar(statusBar);

        menuBar->addAction(menuFile->menuAction());
        menuBar->addAction(menuShow->menuAction());
        menuFile->addAction(actionNew_group);
        menuFile->addAction(actionNew_event);
        menuFile->addAction(actionOpen);
        menuFile->addAction(actionSave);
        menuFile->addAction(actionSave_as);
        menuShow->addAction(actionAdd_member_dependence);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "MainWindow", Q_NULLPTR));
        actionNew_group->setText(QApplication::translate("MainWindow", "New group", Q_NULLPTR));
        actionNew_event->setText(QApplication::translate("MainWindow", "New event", Q_NULLPTR));
        actionSave->setText(QApplication::translate("MainWindow", "Save", Q_NULLPTR));
#ifndef QT_NO_SHORTCUT
        actionSave->setShortcut(QApplication::translate("MainWindow", "Ctrl+S", Q_NULLPTR));
#endif // QT_NO_SHORTCUT
        actionSave_as->setText(QApplication::translate("MainWindow", "Save as", Q_NULLPTR));
#ifndef QT_NO_SHORTCUT
        actionSave_as->setShortcut(QApplication::translate("MainWindow", "Ctrl+Shift+S", Q_NULLPTR));
#endif // QT_NO_SHORTCUT
        actionOpen->setText(QApplication::translate("MainWindow", "Open", Q_NULLPTR));
#ifndef QT_NO_SHORTCUT
        actionOpen->setShortcut(QApplication::translate("MainWindow", "Ctrl+O", Q_NULLPTR));
#endif // QT_NO_SHORTCUT
        actionShow_all_groups->setText(QApplication::translate("MainWindow", "Show all groups", Q_NULLPTR));
        actionShow_all_events->setText(QApplication::translate("MainWindow", "Show all events", Q_NULLPTR));
        actionAdd_member_dependence->setText(QApplication::translate("MainWindow", "Add member dependence", Q_NULLPTR));
#ifndef QT_NO_SHORTCUT
        actionAdd_member_dependence->setShortcut(QApplication::translate("MainWindow", "Ctrl+D", Q_NULLPTR));
#endif // QT_NO_SHORTCUT
        menuFile->setTitle(QApplication::translate("MainWindow", "File", Q_NULLPTR));
        menuShow->setTitle(QApplication::translate("MainWindow", "Edit", Q_NULLPTR));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H

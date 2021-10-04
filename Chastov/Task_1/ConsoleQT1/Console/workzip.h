#ifndef WORKZIP_H
#define WORKZIP_H
#include <QCoreApplication>
#include <QDir>
#include <QDebug>
#include "src/quazip/quazip.h"
#include "src/quazip/quazipfile.h"


class WorkZip
{
public:
    WorkZip();
    static void recurseAddDir(QDir d, QStringList & list);
    static bool archive(const QString & filePath, const QDir & dir, const QString & comment = QString(""));
    static bool extract(const QString & filePath, const QString & extDirPath, const QString & singleFileName = QString(""));
};

#endif // WORKZIP_H

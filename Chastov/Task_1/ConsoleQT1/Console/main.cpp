#include <QCoreApplication>
#include <QDir>
#include <QDebug>
#include <QStorageInfo>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonArray>
//#include <QtXml>
#include <QDomNode>
#include "workzip.h"
//QTextStream cout(stdout);
QTextStream Console(stdin);

void Task1(void)
{
    foreach (const QStorageInfo &storage, QStorageInfo::mountedVolumes())
    {
        if (storage.isValid() && storage.isReady())
        {
            if (!storage.isReadOnly())
            {
                // ...
                //storage.device();
                qDebug() << storage.rootPath();
              if (storage.isReadOnly())
                  qDebug() << "isReadOnly:" << storage.isReadOnly();

              qDebug() << "name:" << storage.name();
              qDebug() << "fileSystemType:" << storage.fileSystemType();
              qDebug() << "size:" << storage.bytesTotal()/1000/1000 << "MB";
              qDebug() << "availableSize:" << storage.bytesAvailable()/1000/1000 << "MB";
            }
        }
    }
}
void outputData(QString fileName)
{
    if (QFile::exists(fileName))
    {
        QFile file(fileName); file.open(file.ReadOnly);
        if (file.isOpen())
        {
            QString result = file.readAll();
            qDebug()<<"File data: " << result;
            file.close();
            file.remove();
        }
    }
}
void inputData(QString fileName, QString message)
{
    QFile file(fileName); file.open(file.WriteOnly);
    if (file.isOpen())
    {
        QTextStream in(&file);
        in<<message;
        file.close();
    }
}
void Task2(void)
{
    QString fileName="", message="";
    qDebug()<<"Filename: ";
    fileName=Console.readLine();
    qDebug()<<("Your message: ");
    message = Console.readLine();
    inputData(fileName, message);
    outputData(fileName);
}

void Task3(void)
{
    QString fileName = "";
    qDebug()<<("Filename: ");
    fileName = Console.readLine();
    //User obj; obj.Age = 22; obj.Company = "Your Company"; obj.Name = "Vadim";
    QJsonDocument doc;
    QJsonArray arr;
    QJsonObject Age{{"Age", "22"}};
    QJsonObject Company{{"Company","IBM"}};
    QJsonObject Name{{"Name","Vadim"}};
    arr.append(Age); arr.append(Company); arr.append(Name);
    doc.setArray(arr);
    inputData(fileName, doc.toJson());
    outputData(fileName);
}

void Task4(void)
{
     qDebug()<<("Your file: ");
     QString fileName=Console.readLine();

     QDomDocument xdoc;
     QDomElement studentNode = xdoc.createElement("User");
     studentNode.setAttribute("name", "Vadim");

     QDomElement company = xdoc.createElement("Company");
     company.appendChild(xdoc.createTextNode("Samsung"));
     QDomElement age = xdoc.createElement("age");
     age.appendChild(xdoc.createTextNode("22"));

     studentNode.appendChild(company);
     studentNode.appendChild(age);

     xdoc.appendChild(studentNode);
     inputData(fileName, xdoc.toString());
     outputData(fileName);

}


void Task5(void)
{
   qDebug()<<("Enter file name");
   QString fileName=Console.readLine();
   const QString dir = "temp"; QString fileFullName = dir + "\\" + fileName;
   QString archivePath = dir + ".zip";
   if (QFile::exists(archivePath)) QFile::remove(archivePath);
   QDir directory;
   directory.mkdir(dir);
   if (directory.exists(dir))
   {
       inputData(fileFullName, "test message");
       //QFile::CR(archivePath);
       if (WorkZip::archive(archivePath, dir,fileName))
       {
           QString tempFile= fileFullName + ".temp";
           if (WorkZip::extract(archivePath,tempFile,fileName))
           {
               outputData(tempFile);

               QFileInfo zip(archivePath);
               qDebug()<<("Length: ")<<zip.size()<<" bytes; creation time: "<<zip.birthTime();
               QFile::remove(archivePath);
               directory.removeRecursively();
           }
       }
   }
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    Task1();
    Task2();
    Task3();
    Task4();
    Task5();
    return a.exec();
}

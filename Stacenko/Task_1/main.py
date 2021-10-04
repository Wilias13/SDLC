import zipfile
import json
import psutil
import shutil
import pathlib
import xml.etree.ElementTree as ET
from xml.dom import minidom

def first():
    x = 0
    disks = psutil.disk_partitions()
    items = len(disks)
    print("Количество дисков: ", items)
    print("Выберите диск")
    count = 0
    for item in disks:
        count += 1
        print(str(count) + ") ", item.device)
    print("Чтобы вернуться назад нажмите 0")
    try:
        x = int(input())
    except:
        print("Вы ввели не цифру")

    finally:
        if (x > items):
            print("Такого диска не существует")
        elif (x == 0):
            start()
        else:
            disc(disks, x)


def disc(disks, x):
    count = 0
    z = 0
    for disk in disks:
        count += 1
        if (count == x):
            print(f"Метка тома: {disk.mountpoint}")
            total, used, free = shutil.disk_usage(disk.device)
            print("Информация о размере, занятом и свободном месте диска:")
            print("Total: %d GiB" % (total // (2 ** 30)))
            print("Used: %d GiB" % (used // (2 ** 30)))
            print("Free: %d GiB" % (free // (2 ** 30)))
            print(f"Тип файловой системы: {disk.fstype}")
            print("Чтобы вернуться назад нажмите 0")
            z = int(input())
    if (z == 0):
        first()


def File():
    print("     ")
    value = str(input())
    print("Введите текст")
    file = open(value+".txt", "w")
    x = str(input())
    count = 0
    file.write(x)
    file.close()
    file = open(value+".txt", "r")
    info = ""
    for item in file:
        info+=item
    print("Введеный текст:\n" + info+"\n")
    file.close()
    try:
        count = int(input("Для удаления файла нажмите: 1\nГлавное меню: 0\n"))
    except:
        print("Данные введены неправильно")
        start()
    finally:
        if count == 1:
            info = pathlib.Path(value + ".txt")
            info.unlink()
            print("Файл удален")
            start()
        elif count == 0:
            start()
        else:
            print("Такого значения не существет")
            info = pathlib.Path(value + ".txt")
            info.unlink()
            print("Файл удален")
            File()


def Json():
    print("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь")
    value = str(input())
    print("Запишите данные")
    count = 0
    x = str(input())
    info = {
        "String": x
    }
    file = open(value+".json", "w", encoding="utf - 8")
    json.dump(info, file, ensure_ascii=False)
    file.close()
    file = open(value+".json", "r", encoding="utf-8")
    data = json.load(file)
    print("Введенные данные:\n"+data["String"])
    file.close()
    try:
        count = int(input("Для удаления файла нажмите: 1\nГлавное меню: 0\n"))
    except:
        print("Данные введены неправильно")
        start()
    finally:
        if count == 1:
            info = pathlib.Path(value + ".json")
            info.unlink()
            print("Файл удален")
            start()
        elif count == 0:
            start()
        else:
            print("Такого значения не существет")
            Json()


def xml():
    print("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь")
    value = str(input())
    print("Введите строку")
    st = str(input())
    data = ET.Element('data')
    data.set("name", st)
    count = 0
    mydata = ET.tostring(data)
    myfile = open(value+".xml", "w")
    text = str(mydata).replace("\'", "")
    myfile.write(text[1:])
    myfile.close()
    print("Запись прошла успешно")
    mydoc = minidom.parse(value+'.xml')
    item = mydoc.getElementsByTagName('data')
    print("Введеные данные:\n"+item[0].attributes["name"].value)
    try:
        count = int(input("Для удаления файла нажмите: 1\nГлавное меню: 0\n"))
    except:
        print("Данные введены неправильно")
        start()
    finally:
        if count == 1:
            info = pathlib.Path(value + ".xml")
            info.unlink()
            print("Файл удален")
            start()
        elif count == 0:
            start()
        else:
            print("Такого значения не существет")
            xml()


def zip():
    print("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь")
    value = str(input())
    print("Укажите полный путь к файлу, который вы хотите заархивировать")
    file = str(input())
    count = 0
    archive = zipfile.ZipFile(value, mode="w")
    try:
        archive.write(file)
        print("Файл добавлен")
    finally:
        archive.close()

    zip_arc = zipfile.ZipFile(value, mode="r")

    for item in zip_arc.infolist():
        print("Название файла ", item.filename, "\nДата создания ", item.date_time, "\nРазмер сжатого файла ",
              item.compress_size, "\nРазмер не сжатого файла ", item.file_size, "\nТип сжатия", item.compress_type)
    zip_arc.close()
    print("Введите папку куда разархивировать")
    new_directory = str(input())
    archive = zipfile.ZipFile(value, "r")
    archive.extractall(new_directory)
    archive.close()
    try:
        count = int(input("Для удаления файлов нажмите: 1\nГлавное меню: 0\n"))
    except:
        print("Данные введены неправильно")
        start()
    finally:
        if count == 1:
            info = pathlib.Path(value)
            info.unlink()
            info = pathlib.Path(file)
            info.unlink()
            info = pathlib.Path(new_directory)
            info.unlink()
            print("Файлы удалены")
            start()
        elif count == 0:
            start()
        else:
            print("Такого значения не существет")
            zip()



def start():
    value = 0
    try:
        value = int(input("Выберите номер задания:\n1. Файловая система.\n2. Работа с файлом.\n3. Работа с JSON\n4. Работа с XML\n5. Создание архива\n0. Выход из программы\n"))
    except:
        print("Вы ввели не то значение")
    finally:
        if (value == 1):
            first()
        elif (value == 2):
            File()
        elif (value == 3):
            Json()
        elif value == 4:
            xml()
        elif value == 5:
            zip()
        elif value == 0:
            exit()
        else:
            print("Неверно введено значение")
            start()

start()
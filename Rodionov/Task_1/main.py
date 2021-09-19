import psutil
import json
import zipfile
import datetime
import os
from psutil._common import bytes2human
from xml.dom import minidom
import xml.etree.ElementTree as ET


def logicalinfo():
    print("Информация о логических дисках")
    template = "%-10s %15s %15s %15s %5s%% %10s"
    print(template % ("Диск", "Всего места", "Использовано", "Свободно", "Занято ", "Тип  "))
    for part in psutil.disk_partitions(all=False):
        if os.name == 'nt':
            if 'cdrom' in part.opts or part.fstype == '':
                continue
        usage = psutil.disk_usage(part.mountpoint)
        print(template % (
            part.device,
            bytes2human(usage.total),
            bytes2human(usage.used),
            bytes2human(usage.free),
            int(usage.percent),
            part.fstype))


def createfile(file_name):
    open(file_name, "w")


def writeinfile(file_name):
    print("Работа с файлом")
    print("Введите строку = ", end="")
    string = input()
    try:
        fd = open(file_name, 'w')
        fd.write(string)
    except IOError:
        print("Файл не существует")


def readfile(file_name):
    try:
        print("Содержание файла:")
        fd = open(file_name, 'r')
        print(fd.read())
        fd.close()
    except IOError:
        print("Файл не существует")


def filedelete(file_name):
    try:
        os.remove(file_name)
    except IOError:
        print("Файл не существует")


def serialazejson(file_name):
    print("Работа с Json")
    print("Введите название компании = ", end="")
    Company=input()
    print("Введите имя сотрудника = ", end="")
    data = {'Компания': Company, 'Имя': input()}
    try:
        with open(file_name, "w") as write_file:
            json.dump(data, write_file)
    except IOError:
        print("Файл не существует")


def deserialazejson(file_name):
    try:
        print("Содержание файла Json:")
        with open(file_name, "r") as rf:
            decoded_data = json.load(rf)
        print(decoded_data)
    except IOError:
        print("Файл не существует")


def serialazexml(file_name):
    print("Работа с XML")
    print("Введите название родительского элемента = ", end="")
    data = ET.Element(input())
    print("Введите название дочернего элемента = ", end="")
    ET.SubElement(data, input())
    try:
        my_data = ET.tostring(data)
        with open(file_name, "wb") as binary_file:
            binary_file.write(my_data)
    except IOError:
        print("Файл не существует")


def deserialazexml(file_name):
    try:
        print("Содержание файла XML:")
        fd = open(file_name, 'r')
        print(fd.read())
        fd.close()
    except IOError:
        print("Файл не существует")


def createzip(zip_name):
    try:
        print("Работа с ZIP")
        zipfile.ZipFile(zip_name, mode='w', compression=zipfile.ZIP_DEFLATED)
    except Exception:
        print("Файл не существует")


def compresszip(file_name, zip_name):
    try:
        with zipfile.ZipFile(zip_name, mode='w', compression=zipfile.ZIP_DEFLATED) as zf:
            zf.write(file_name)
    except Exception:
        print("Файл не существует")


def decompresszip(file_name):
    try:
        print("Содержание ZIP-файла:")
        with zipfile.ZipFile(file_name, mode='a') as zf:
            for file in zf.infolist():
                date = datetime.datetime(*file.date_time)
                name = os.path.basename(file.filename)
                print(f"{name},\t{file.file_size},\t{file.compress_size},\t \
                                       {date.strftime('%H:%M %d.%m.%Y')}")
    except Exception:
        print("Файл не существует")


logicalinfo()
createfile("temp.txt")
writeinfile("temp.txt")
readfile("temp.txt")
createzip("temp.zip")
compresszip("temp.txt", "temp.zip")
decompresszip("temp.zip")
filedelete("temp.txt")
filedelete("temp.zip")
createfile("temp.json")
serialazejson("temp.json")
deserialazejson("temp.json")
filedelete("temp.json")
createfile("temp.xml")
serialazexml("temp.xml")
deserialazexml("temp.xml")
filedelete("temp.xml")

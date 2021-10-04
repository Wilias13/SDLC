import os, ctypes, shutil, json
import xml.etree.ElementTree as ET
from zipfile import ZipFile

letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
drives = ['{0}:'.format(d) for d in letters if os.path.exists('{0}:'.format(d))]

def GetName(dl):

    kernel32 = ctypes.windll.kernel32
    volumeNameBuffer = ctypes.create_unicode_buffer(1024)
    fileSystemNameBuffer = ctypes.create_unicode_buffer(1024)
    serial_number = None
    max_component_length = None
    file_system_flags = None

    rc = kernel32.GetVolumeInformationW(
        ctypes.c_wchar_p(dl + '\\'),
        volumeNameBuffer,
        ctypes.sizeof(volumeNameBuffer),
        serial_number,
        max_component_length,
        file_system_flags,
        fileSystemNameBuffer,
        ctypes.sizeof(fileSystemNameBuffer)
    )

    print ("Имя: " + 
    volumeNameBuffer.value + 
    ", Метка тома:" +
    " " + 
    dl + 
    ", тип: " +
    fileSystemNameBuffer.value + 
    ", размер: " + 
    str(shutil.disk_usage(dl + '\\').total//1024//1024//1024) + 
    'gb')

for dl in drives:

    GetName(dl)

def File():

    name = 'file.txt'
    with open(name, 'w') as file:
        file.write("Запись строки в файл")
        file.close()
    with open('file.txt', 'r') as file:
        print(file.read())
        file.close()
        os.remove(name)

def FileJson():

    name = "file.json"
    data = {
        "text": "Запись строки в файл"
    }
    with open(name, "w") as file:
        json.dump(data, file)
        file.close()
    with open(name, "r") as file:
        print(json.load(file))
        file.close()
        os.remove(name)

def FileXML():

    name = "file.xml"
    root = ET.Element("data")
    tree = ET.ElementTree(root)
    tree.write("%s" %(name))
    with open("%s" % (name), "r") as file:
        tree = ET.fromstring(file.read())
    print(ET.tostring(tree))
    os.remove(name)


def ZipArchive():
    name = "file.txt"
    name_zip = "file.zip"
    with open(name, 'w') as file:
        file.write("Запись строки в файл")
        file.close()
    with ZipFile(name_zip, "w") as zip:
            zip.write(name)
            os.remove(name)
            zip.close()
    z = ZipFile(name_zip, 'r')
    z.extractall()
    z.close()
    file = open(name)
    print("Содержимое файла: " + file.read())
    file.close()
    print("Данные о файле:")
    print("     Размер: " + str(os.path.getsize(name)) + " байт")
    print("     Дата последнего изменения: " + str(os.path.getmtime(name)))
    print("     Дата создания файла: " + str(os.path.getctime(name)))
    os.remove(name)
    os.remove(name_zip)


File()
FileJson()
FileXML()
ZipArchive()
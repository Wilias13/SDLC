import os
import json
import gzip
import os
import ctypes
import shutil
import xml.etree.cElementTree as ET

from types import SimpleNamespace

class User:

    Name = None
    Age = None

    def __init__(self, name, age):
        self.Name = name
        self.Age = age

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True)

letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
drives = ['{0}:'.format(d) for d in letters if os.path.exists('{0}:'.format(d))]

def get_name(dl):

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

def task_0():
    for dl in drives:
        get_name(dl)

def writeAsJSON(filename, data):
    with open(filename, "w") as outfile:
        json.dump(data, outfile)
    print("Текст записан в файл %s" % (filename))

def readAsJSON(filename):
    with open("%s" % (filename), "r") as json_file:
        jason = json.loads(json.load(json_file), object_hook=lambda d: SimpleNamespace(**d))
        print("Name: {%s} Age: {%s}" % (jason.Name, jason.Age))

def write_to_file(filename, data):
    f = open(filename, "w")
    f.write(data)
    f.close()
    print("Текст записан в файл: %s" % filename)

def read_from_file(filename):
    f = open(filename, "r")
    result = f.read()
    print("Текст из файла: %s" % result)
    return result

def delete_file(filename):
    os.remove(filename)
    print("Файл %s удален" % filename)

def task_1():
    print("Задание №1")
    filename = "C:\\SomeDir2\\note.txt"
    text = input("Введите строку для записи в файл:")
    write_to_file(filename, text)
    read_from_file(filename)

def task_2():
    print("Задание №2")
    filename = "C:\\SomeDir2\\user.json"
    user = User("Tom", 35)
    writeAsJSON(filename, user.toJSON())
    readAsJSON(filename)
    delete_file(filename)

def task_3():
    print("Задание №3")
    filename = "C:\\SomeDir2\\users.xml"
    users = [User("Bill Gates", 48), User("Larry Page", 42)]
    
    root = ET.Element("users")
    
    for user in users:
        userElem = ET.SubElement(root, "user")
        ET.SubElement(userElem, "Username", name=user.Name)
        ET.SubElement(userElem, "Age").text = str(user.Age)

    tree = ET.ElementTree(root)
    tree.write("%s" % (filename))
    print("XML записан в файл %s" % (filename))
    
    with open("%s" % (filename), "r") as xml_file:
        tree = ET.fromstring(xml_file.read())

        for el in tree.findall('user'):
            for ch in list(el):
                if ch.attrib is not None and ch.attrib.get("name") is not None:
                    print("%s %s" % (ch.tag, ch.attrib.get("name")))
                if ch.text is not None:
                    print("%s %s" % (ch.tag, ch.text))
    delete_file(filename)

def task_4():
    print("Задание №4")
    source_file = "C:\\SomeDir2\\book.pdf"
    compressed_file = "C:\\SomeDir2\\book.gz"
    target_file = "C:\\SomeDir2\\book_new.pdf"

    input = open(source_file, 'rb')
    s = input.read()
    input.close()

    output = gzip.GzipFile(compressed_file, 'wb')
    output.write(s)
    output.close()
    print("Сжатие файла %s завершено. Исходный размер %d, сжатый размер %d." % (source_file, os.path.getsize(source_file), os.path.getsize(compressed_file)))

    input = gzip.GzipFile(compressed_file, 'rb')
    s = input.read()
    input.close()

    output = open(target_file, 'wb')
    output.write(s)
    output.close()
    print("Восстановлен файл %s" % (target_file))

    delete_file(target_file)
    delete_file(compressed_file)

if __name__ == "__main__":
    task_0()
    task_1()
    task_2()
    task_3()
    task_4()
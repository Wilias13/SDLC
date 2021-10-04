import psutil
import json
import os
import xml.etree.ElementTree as xml
import zipfile

def fileinfo():
    human = dict(name="Арп гшж", age=23, weight=78.5)
    filename = "08_01_02_data.txt"

    fh = None
    try:
        fh = open(filename, "wb")
        fh.write(bytes(human["name"].encode("utf-8")))
        fh.write(bytes(str(human["age"]).encode("utf-8")))
        fh.write(bytes(str(human["weight"]).encode("utf-8")))
    finally:
        if fh:
            fh.close()

    fh = None
    try:
        fh = open(filename, "rb")
        name = fh.read(13).decode("utf-8")
        age = int(fh.read(2).decode("utf-8"))
        weight = float(fh.read(4).decode("utf-8"))
        print(name, age, weight)
    finally:
        if fh:
            fh.close()
    z = zipfile.ZipFile('spam.zip', 'w')
    z.write('08_01_02_data.txt')
    z.close()
    z = zipfile.ZipFile('spam.zip', 'r')
    z.printdir()
    z.close()
    path = os.path.join(os.path.abspath(os.path.dirname(__file__)), 'spam.zip')
    os.remove(path)
    path = os.path.join(os.path.abspath(os.path.dirname(__file__)), '08_01_02_data.txt')
    os.remove(path)


def jsonf():
    filename = "08_01_03_data.json"

    info = {
        "ФИО": "Некрасова Инна Олеговна",
        "ЕГЭ": {
            "Математика": 76,
            "Русский язык": 94,
            "Информатика": 75
        },
        "Хобби": ["Рисование", "Плавание"],
        "Возраст": 25.5,
        "ДомЖивотные": None
    }

    with open(filename, "w", encoding="utf-8") as fh:
        fh.write(json.dumps(info, ensure_ascii=False, indent=4))

    info_2 = []
    with open(filename, encoding="utf-8") as fh:
        info_2 = json.loads(fh.read())

    print(info_2)
    path = os.path.join(os.path.abspath(os.path.dirname(__file__)), '08_01_03_data.json')
    os.remove(path)


def createxml():
    rootXML = xml.Element("settings")

    text = xml.Element("text")
    text.text = "Text"
    rootXML.append(text)

    file = open("xml1.xml", "w")
    file.write(xml.tostring(rootXML, encoding="utf-8", method="xml").decode(encoding="utf-8"))
    file.close()


def readxml():
    with open("xml1.xml", "r") as file:
        for line in file:
            print(line)
    path = os.path.join(os.path.abspath(os.path.dirname(__file__)), 'xml1.xml')
    os.remove(path)


print(psutil.disk_partitions())
fileinfo()
jsonf()
createxml()
readxml()




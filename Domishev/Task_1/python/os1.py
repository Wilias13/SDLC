import os
from xml.etree import ElementTree as ET
import json
import psutil
import zipfile
import argparse


def disks():
    for disk in psutil.disk_partitions():
        usage = psutil.disk_usage(disk.mountpoint)
        total_space = usage.total
        free_space = usage.free
        free_space_percentage = free_space / total_space * 100
        print(f'Disk:        {disk.mountpoint}')
        print(f'Filesystem:  {disk.fstype}')
        print(f'Total space: {total_space}')
        print(f'Free space:  {free_space} ({free_space_percentage:.2f}%)', )


def do_file(p):
    # Создать файл
    with open(p, 'w') as f:
        # Записать в файл строку
        f.write(input('file string'))

    # Прочитать файл в консоль
    with open(p, 'r') as f:
        print(f.read())
    # Удалить файл
    os.remove(p)


class SampleObject:
    def __init__(self, name, age, location):
        self.name = name
        self.age = age
        self.location = location


def do_json(example, output):
    with open(example) as f:
        print(json.load(f))
    o = SampleObject(input('name'), int(input('age')), input('location'))
    with open(output, 'w') as f:
        json.dump(o.__dict__, f, ensure_ascii=False)
    with open(output) as f:
        print(f.read())
    os.remove(output)


def dict2xml(d, root_node=None):
    wrap = False if None == root_node or isinstance(d, list) else True
    root = 'objects' if None == root_node else root_node
    root_singular = root[:-1] if 's' == root[-1] and None == root_node else root
    xml = ''
    attr = ''
    children = []

    if isinstance(d, dict):
        # print(d)
        for key, value in dict.items(d):
            if isinstance(value, dict):
                children.append(dict2xml(value, key))
            elif isinstance(value, list):
                children.append(dict2xml(value, key))
            elif key[0] == '@':
                attr = attr + ' ' + key[1::] + '="' + str(value) + '"'
            else:
                xml = '<' + key + ">" + str(value) + '</' + key + '>'
                children.append(xml)

    else:
        # if list
        for value in d:
            children.append(dict2xml(value, root_singular))

    end_tag = '>' if 0 < len(children) else '/>'

    if wrap or isinstance(d, dict):
        xml = '<' + root + attr + end_tag

    if 0 < len(children):
        for child in children:
            xml = xml + child

        if wrap or isinstance(d, dict):
            xml = xml + '</' + root + '>'

    return xml


def do_xml(example, output):
    with open(example) as f:
        print(ET.tostring(ET.fromstring(f.read())))
    o = SampleObject(input('name'), int(input('age')), input('location'))
    with open(output, 'w') as f:
        f.write(dict2xml(o.__dict__))
    with open(output) as f:
        print(f.read())
    os.remove(output)


def do_zip():
    file = input("file to zip")

    with zipfile.ZipFile('archive.zip', 'w') as f:
        f.write(file)


def do_unzip():
    with zipfile.ZipFile('archive.zip') as f:
        f.printdir()
        f.extractall()


if __name__ == '__main__':
    disks()
    do_file('test.txt')
    do_json('example.json', 'output.json')
    do_xml('example.xml', 'example.xml')
    do_zip()
    do_unzip()

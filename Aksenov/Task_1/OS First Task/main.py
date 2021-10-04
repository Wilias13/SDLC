import os
from xml.etree import ElementTree
import json
import psutil
import zipfile


class Object:
    def __init__(self, name, age, location, institute, stipend):
        self.name = name
        self.age = age
        self.location = location
        self.institute = institute
        self.stipend = stipend


def work_with_file(p):
    with open(p, 'w') as file:
        file.write(input('Write string into file'))

    with open(p, 'r') as file:
        print(file.read())

    os.remove(p)


def work_with_json(example, output):
    with open(example) as file:
        print(json.load(file))

    obj = Object(input('Name: '),
                 int(input('Age: ')),
                 input('Location: '),
                 input('Institute: '),
                 int(input('Stipend: ')))

    with open(output, 'w') as file:
        json.dump(obj.__dict__, file, ensure_ascii=False)

    with open(output) as file:
        print(file.read())

    os.remove(output)


def dict2xml(d, root_node=None):
    wrap = False if None == root_node or isinstance(d, list) else True
    root = 'objects' if None == root_node else root_node
    root_singular = root[:-1] if 's' == root[-1] and None == root_node else root
    xml = ''
    attr = ''
    children = []

    if isinstance(d, dict):
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


def work_with_xml(example, output):
    with open(example) as file:
        print(ElementTree.tostring(ElementTree.fromstring(file.read())))

    obj = Object(input('Name: '),
                 int(input('Age: ')),
                 input('Location: '),
                 input('Institute: '),
                 int(input('Stipend: ')))

    with open(output, 'w') as file:
        file.write(dict2xml(obj.__dict__))

    with open(output) as file:
        print(file.read())

    os.remove(output)


def zip_archive():
    file = input("file to zip: ")

    with zipfile.ZipFile('archive.zip', 'w') as f:
        f.write(file)


def unzip_archive():
    with zipfile.ZipFile('archive.zip') as file:
        file.printdir()
        file.extractall()


def disks_information():
    for disk in psutil.disk_partitions():
        usage = psutil.disk_usage(disk.mountpoint)
        print(f'Disk name:             {disk.mountpoint}')
        print(f'Disk file system type: {disk.fstype}')
        total_space = usage.total
        print(f'Disk capacity:         {total_space}')
        free_space = usage.free
        free_space_percentage = free_space / total_space * 100
        print(f'Disk free space:       {free_space} ({free_space_percentage:.2f}%)', )


if __name__ == '__main__':
    print('Information about your file system:')
    disks_information()
    print('Work with files: ')
    work_with_file('kek1.txt')
    print('Work with jsons: ')
    work_with_json('kek2.json', 'kek3.json')
    print('Work with xmls: ')
    work_with_xml('kek4.xml', 'kek5.xml')
    print('Work with archives: ')
    zip_archive()
    print('Work with extraction from archive: ')
    unzip_archive()

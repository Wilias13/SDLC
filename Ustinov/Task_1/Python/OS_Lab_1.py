from zipfile import ZipFile
import datetime
import pandas as pd
import numpy as np
import json
import os
import psutil

## 1
drives = psutil.disk_partitions()
for d in drives:
    try:
        du = psutil.disk_usage(d.device)
        print(
f"""
Device: {d.device}
Mountpont: {d.mountpoint}
File System: {d.fstype}
Disk Usage:
    total: {du.total} bytes
    used: {du.used} bytes
    free: {du.free} bytes"""
        )
        
    except Exception as ex:
        print("\n", d.device)
        print(ex)
    

## 2
file_name = "test.txt"
s = input()
with open(file_name, "w") as f:
    f.write(s)
with open(file_name, "r") as f:
    print(f.read())
try:
    os.remove(file_name)
except Exception as ex:
        print(str(ex))
else:
    print("success!")


## 3
file_name = "test.json"
d = {
    "a": 1,
    "b": [1],
    "c": "1"
}

with open(file_name, "w") as f:
    json.dump(d, f)
with open(file_name, "r") as f:
    print(json.load(f))
try:
    os.remove(file_name)
except Exception as ex:
        print(str(ex))
else:
    print("success!")


## 4
file_name = "test.xml"
df = pd.DataFrame(np.random.randn(4, 3), columns = ["a", "b", "c"])
print(df)

df.to_xml(file_name, index=None)
df = pd.read_xml(file_name)

row = []
for col in df.columns:
    row.append(input(col))

df = df.append(pd.DataFrame([row], columns=df.columns, index=[df.index.stop]), ignore_index=False)
df.to_xml(file_name, index=None)
print(df)

try:
    os.remove(file_name)
except Exception as ex:
        print(str(ex))
else:
    print("success!")


## 5
file_name = "files.zip"
file_paths = ["1.txt", "2.txt", "3.txt"]

for file in file_paths:
    with open(file, "w") as f:
        f.write(file)
with ZipFile(file_name,'w') as zip:
        for file in file_paths:
            zip.write(file)
file_name = file_name

with ZipFile(file_name, 'r') as zip:
	for info in zip.infolist():
			print(info.filename)
			print('\tModified:\t' + str(datetime.datetime(*info.date_time)))
			print('\tSystem:\t\t' + "Windows" if str(info.create_system) else "Unix")
			print('\tZIP version:\t' + str(info.create_version))
			print('\tCompressed:\t' + str(info.compress_size) + ' bytes')
			print('\tUncompressed:\t' + str(info.file_size) + ' bytes')
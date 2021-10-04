use anyhow::{bail, Context, Result};
use pico_args::Arguments;
use serde_derive::{Deserialize, Serialize};
use serde_json::Value;
use std::fs::{read as read_file, read_to_string, remove_file, write as write_file, File};
use std::io::{Write, Read};
use std::path::Path;
use sysinfo::{DiskExt, System, SystemExt};
use zip::{ZipArchive, ZipWriter};

fn disks() -> Result<()> {
    // Вывести информацию в консоль о логических дисках, именах, метке тома,
    // размере типе файловой системы.

    let mut sys = System::new_all();
    sys.refresh_all();

    for disk in sys.disks() {
        let total_space = disk.total_space();
        let free_space = disk.available_space();
        let free_space_percentage = free_space as f64 / total_space as f64 * 100.0;

        println!("Disk:        {}", disk.mount_point().to_string_lossy());
        println!("Name:        {}", disk.name().to_string_lossy());
        println!("Filesystem:  {}", std::str::from_utf8(disk.file_system())?);
        println!("Type:        {:?}", disk.type_());
        println!("Total space: {}", total_space);
        println!(
            "Free space:  {} ({:.2}%)",
            free_space, free_space_percentage
        );
        println!();
    }

    Ok(())
}

fn file(mut args: Arguments) -> Result<()> {
    let create = args.contains("--create");
    let write = args.contains("--write");
    let read = args.contains("--read");
    let remove = args.contains("--remove");
    let filename: String = args.free_from_str()?;

    println!("filename: {}", filename);

    // Создать файл
    if create || write {
        let mut f = File::create(&filename)?;

        // Записать в файл строку
        if write {
            f.write_all(args.free_from_str::<String>()?.as_bytes())?;
        }
    };

    // Прочитать файл в консоль
    if read {
        println!("{}", read_to_string(&filename)?);
    }

    // Удалить файл
    if remove {
        remove_file(&filename)?;
    }

    Ok(())
}

#[derive(Debug, Serialize, Deserialize)]
pub struct SampleObject {
    name: String,
    age: u32,
    location: String,
}

fn parse_json(mut args: Arguments) -> Result<()> {
    // Создать файл формате JSON из редактора в
    let path = args.free_from_str::<String>()?;
    let json = read_to_string(path)?;
    let json: Value = serde_json::from_str(&json)?;
    println!("{:#?}", json);
    Ok(())
}

fn write_json(mut args: Arguments) -> Result<()> {
    let write = args.contains("--write");
    let read = args.contains("--read");
    let remove = args.contains("--remove");

    // Создать новый объект. Выполнить сериализацию объекта в формате JSON и
    // записать в файл.
    let output = if write {
        let name = args.opt_value_from_str("--name")?.unwrap_or("".to_string());
        let age = args
            .opt_value_from_str("--age")?
            .unwrap_or("0".to_string())
            .parse::<u32>()?;
        let location = args
            .opt_value_from_str("--location")?
            .unwrap_or("".to_string());

        let output = args.free_from_str::<String>()?;

        let object = SampleObject {
            name,
            age,
            location,
        };
        write_file(&output, serde_json::to_string(&object)?)?;

        output
    } else {
        args.free_from_str::<String>()?
    };

    // Прочитать файл в консоль
    if read {
        println!("{}", read_to_string(&output)?);
    }

    // Удалить файл
    if remove {
        remove_file(&output)?;
    }

    Ok(())
}

fn parse_xml(mut args: Arguments) -> Result<()> {
    // Создать файл формате JSON из редактора в
    let path = args.free_from_str::<String>()?;
    let xml = read_to_string(path)?;
    let object: SampleObject = serde_xml_rs::from_str(&xml)?;
    println!("{:#?}", object);
    Ok(())
}

fn write_xml(mut args: Arguments) -> Result<()> {
    let write = args.contains("--write");
    let read = args.contains("--read");
    let remove = args.contains("--remove");

    // Записать в файл новые данные из консоли.
    let output = if write {
        let name = args.opt_value_from_str("--name")?.unwrap_or("".to_string());
        let age = args
            .opt_value_from_str("--age")?
            .unwrap_or("0".to_string())
            .parse::<u32>()?;
        let location = args
            .opt_value_from_str("--location")?
            .unwrap_or("".to_string());

        let output = args.free_from_str::<String>()?;

        let object = SampleObject {
            name,
            age,
            location,
        };
        write_file(&output, serde_xml_rs::to_string(&object)?)?;

        output
    } else {
        args.free_from_str::<String>()?
    };

    // Прочитать файл в консоль
    if read {
        println!("{}", read_to_string(&output)?);
    }

    // Удалить файл
    if remove {
        remove_file(&output)?;
    }

    Ok(())
}

fn zip(mut args: Arguments) -> Result<()> {
    let create = args.contains("--create");
    let remove = args.contains("--remove");
    let add_files = args.values_from_str::<_, String>("--add")?;
    let output = args.free_from_str::<String>()?;

    println!("abc");

    if create || add_files.len() != 0 {
        // Создать архив в форматер zip
        let mut zip = ZipWriter::new(File::create(&output)?);

        if add_files.len() != 0 {
            for file in add_files {
                println!("adding file: {}", file);
                // Добавить файл в архив
                let name = Path::new(&file)
                    .file_name()
                    .context("no file name specified")?
                    .to_string_lossy();

                zip.start_file(name, Default::default())?;
                zip.write_all(&read_file(file)?)?;
            }
        }
        zip.finish()?;
    };

    // Удалить файл и архив
    if remove {
        remove_file(&output)?;
    }

    Ok(())
}

fn unzip(mut args: Arguments) -> Result<()> {
    let output = args.free_from_str::<String>()?;

    // Разархивировать файл и вывести данные о нем
    let mut zip = ZipArchive::new(File::open(output)?)?;

    for i in 0..zip.len() {
        let mut file = zip.by_index(i)?;
        let name = file
            .enclosed_name()
            .context("bad filename")?
            .to_path_buf();

        println!("Unpacking {} (size={})", name.to_string_lossy(), file.size());

        let mut buf = Vec::new();
        file.read_to_end(&mut buf)?;

        write_file(name, &buf)?;
    }

    Ok(())
}

fn do_things() -> Result<()> {
    let mut args = pico_args::Arguments::from_env();
    let subcommand = args.subcommand()?.context("subcommand is required")?;

    match subcommand.as_str() {
        "disks" => disks(),
        "file" => file(args),
        "parse-json" => parse_json(args),
        "write-json" => write_json(args),
        "parse-xml" => parse_xml(args),
        "write-xml" => write_xml(args),
        "zip" => zip(args),
        "unzip" => unzip(args),
        _ => bail!("unknown subcommand: {}", subcommand),
    }
}

fn main() {
    match do_things() {
        Ok(_) => (),
        Err(e) => eprintln!("Error: {}", e),
    }
}

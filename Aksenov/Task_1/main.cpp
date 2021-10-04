#define WIN32_LEAN_AND_MEAN
#define NOMINMAX

#include <zip.h>
#include <fstream>
#include <iostream>
#include <exception>
#include <Windows.h>
#include <filesystem>
#include <nlohmann/json.hpp>


namespace fs = std::filesystem;

void file_system_volumes_information() {
    int sz = GetLogicalDriveStrings(NULL, 0);
    auto* sz_logical_drives = new TCHAR [sz];
    GetLogicalDriveStrings(sz, sz_logical_drives);

    while(*sz_logical_drives) {
        TCHAR sz_volume[MAX_PATH];
        lstrcpy(sz_volume, sz_logical_drives);

        if (GetDriveType(sz_volume) == DRIVE_FIXED) {
            DWORD dw_system_flags;
            char c_name_buffer[MAX_PATH], c_system_name_buffer[MAX_PATH];

            GetVolumeInformation(sz_volume,
                                 c_name_buffer,
                                 sizeof(c_name_buffer),
                                 NULL,
                                 0,
                                 &dw_system_flags,
                                 c_system_name_buffer,
                                 sizeof(c_system_name_buffer));

            __int64 uli_total_bytes;
            GetDiskFreeSpaceEx(sz_volume, NULL, (PULARGE_INTEGER) &uli_total_bytes, NULL);

            std::wcout << L"File system directory: " <<  sz_volume << '\n'
                       << L"Disk name: " << c_name_buffer << '\n'
                       << L"Disk file system type: " << c_system_name_buffer << '\n'
                       << L"Disk file system flags: " << dw_system_flags << '\n'
                       << L"Disk capacity: " << uli_total_bytes / (1024 * 1024)  << L" mb\n\n";
        }

        while (*sz_logical_drives) {
            sz_logical_drives++;
        }

        sz_logical_drives++;
    }
}

int on_extract_entry(const char* filename, void* arg) {
    static int i = 0;
    int n = *(int *)arg;
    printf("Extracted: %s (%d of %d)\n", filename, ++i, n);

    return 0;
}

class File {
public:
    explicit File(fs::path& path)
    : m_path(path)
    , m_file()
    {
        m_file.open(m_path, std::ios::out | std::ios::app);

        if (!m_file.is_open()) {
            throw std::runtime_error("Cannot create a file!");
        }

        m_file.close();
    }

    void Delete() {
        remove(m_path);
    }

    std::string Read() {
        m_file.open(m_path, std::ios::in);

        if (m_file.is_open()) {
            std::stringstream buffer;
            buffer << m_file.rdbuf();
            m_file.close();

            return buffer.str();
        } else {
            throw std::runtime_error("File does not exist!");
        }
    }

    void Write(const std::string& string) {
        m_file.open(m_path, std::ios::out);

        if (m_file.is_open()) {
            m_file << '\n' << string;
            m_file.close();
        } else {
            throw std::runtime_error("File does not exist!");
        }
    }

    ~File() = default;

protected:
    fs::path m_path;
    std::fstream m_file;
};

class Json : public File {
public:
    explicit Json(fs::path& path)
    : File(path)
    , m_json()
    {}

    Json(nlohmann::json& json, fs::path& path)
    : File(path)
    , m_json(json) {
        m_file.open(path, std::ios::out | std::ios::app);

        if (m_file.is_open()) {
            m_file << m_json.dump(4);
            m_file.close();
        } else {
            throw std::runtime_error("File does not exist!");
        }
    }

    friend std::ostream& operator<<(std::ostream &out, Json& json);

    ~Json() = default;

private:
    nlohmann::json m_json;
};

std::ostream& operator<<(std::ostream &out, Json& json) {
    out << std::setw(4) << json.Read();

    return out;
}

class ZIP {
public:
    explicit ZIP(fs::path& path_to_archive)
    : m_path(path_to_archive)
    {
        struct zip_t *zip = zip_open(m_path.string().c_str(), ZIP_DEFAULT_COMPRESSION_LEVEL, 'w');
        zip_close(zip);
    }

    ZIP(fs::path& path_to_archive, fs::path& file_to_archive)
    : m_path(path_to_archive)
    {
        struct zip_t *zip = zip_open(m_path.string().c_str(), ZIP_DEFAULT_COMPRESSION_LEVEL, 'w');
        {
            zip_entry_open(zip, file_to_archive.filename().string().c_str());
            {
                File file(file_to_archive);
                char *cstr = new char[file.Read().length() + 1];
                strcpy(cstr, file.Read().c_str());
                zip_entry_write(zip, cstr, strlen(cstr));
            }
            zip_entry_close(zip);
        }
        zip_close(zip);
    }

    void Append(fs::path& file_to_append) {
        struct zip_t *zip = zip_open(m_path.string().c_str(), ZIP_DEFAULT_COMPRESSION_LEVEL, 'a');
        {
            zip_entry_open(zip, file_to_append.filename().string().c_str());
            {
                File file(file_to_append);
                char *cstr = new char[file.Read().length() + 1];
                strcpy(cstr, file.Read().c_str());
                zip_entry_write(zip, cstr, strlen(cstr));
            }
            zip_entry_close(zip);
        }
        zip_close(zip);
    };

    void Extract(fs::path& extracting_path, int arg = 2) {
        zip_extract(m_path.string().c_str(), extracting_path.string().c_str(), on_extract_entry, &arg);
    };

    void Delete() {
        remove(m_path);
    };

    ~ZIP() = default;

private:
    fs::path m_path;
};

int main() {

    size_t main_choice;

    do {
        std::cout << "Hello, this program provides the following features" << std::endl;
        std::cout << "1 - Providing information about your file system" << std::endl;
        std::cout << "2 - Work with files" << std::endl;
        std::cout << "3 - Work with jsons files" << std::endl;
        std::cout << "4 - Work with archives" << std::endl;

        std::cout << "What do you want to do?" << std::endl;

        while (std::cout << "Enter your choice: " && !(std::cin >> main_choice)) {
            if (std::cin.fail()) {
                std::cin.clear();
                std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                std::cout << "Invalid input; please re-enter.\n";
            }
        }

        switch (main_choice) {
            case 1: {
                std::cout << "Do you want to know about your file system volumes information?" << std::endl;

                int choice;

                while (std::cout << "Enter your choice: '\n' 0 - Yes, 1 - No" && !(std::cin >> choice)) {
                    if (std::cin.fail()) {
                        std::cin.clear();
                        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                        std::cout << "Invalid input; please re-enter.\n";
                    }
                }

                switch (choice) {
                    case 0: {
                        file_system_volumes_information();
                    }

                    case 1: {
                        break;
                    }

                    default: {
                        break;
                    }
                }

                break;
            }

            case 2: {
                fs::path path_to_file;
                size_t file_choice;

                do {
                    while (std::cout << "Enter path to a file (Example - C:\\example.txt): " && !(std::cin >> path_to_file)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    std::cout << "What do you want to do with this file?" << std::endl;
                    std::cout << "1: Write string into a file" << std::endl;
                    std::cout << "2: Read file into console" << std::endl;
                    std::cout << "3: Delete this file" << std::endl;

                    size_t choice;

                    while (std::cout << "Enter your choice: " && !(std::cin >> choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    switch (choice) {
                        case 1: {
                            std::string string;

                            while (std::cout << "Enter your string: " && !(std::cin >> string)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            File file(path_to_file);
                            file.Write(string);

                            std::cout << "The string was successfully written to the file " << path_to_file << std::endl;

                            break;
                        }

                        case 2: {
                            File file(path_to_file);
                            std::cout << file.Read() << std::endl;

                            break;
                        }

                        case 3: {
                            File file(path_to_file);
                            file.Delete();
                            std::cout << "File " << path_to_file << " deleted successfully" << std::endl;

                            break;
                        }

                        default: {
                            break;
                        }
                    }

                    while (std::cout << "Do you want to continue work with files?: '\n' 0 - Yes, 1 - No" && !(std::cin >> file_choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }
                } while (file_choice == 0);

                break;
            }
            case 3: {
                fs::path path_to_json;
                size_t json_choice;

                do {
                    while (std::cout << "Enter path to a file (Example - C:\\example.json): " && !(std::cin >> path_to_json)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    std::cout << "What do you want to do with this file?" << std::endl;
                    std::cout << "1: Write json object into a new json file" << std::endl;
                    std::cout << "2: Read file into console" << std::endl;
                    std::cout << "3: Delete this file" << std::endl;

                    size_t choice;

                    while (std::cout << "Enter your choice: " && !(std::cin >> choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    switch (choice) {
                        case 1: {
                            std::string string;

                            while (std::cout << R"(Enter your string (Example - {"first": "one", "second": "two"}): )" && !(std::cin >> string)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            nlohmann::json json_object = nlohmann::json::parse(string);
                            Json json(json_object, path_to_json);

                            std::cout << "The string was successfully written to the file " << path_to_json << std::endl;

                            break;
                        }

                        case 2: {
                            Json file(path_to_json);
                            std::cout << file << std::endl;

                            break;
                        }

                        case 3: {
                            Json file(path_to_json);
                            file.Delete();
                            std::cout << "File " << path_to_json << " deleted successfully" << std::endl;

                            break;
                        }

                        default: {
                            break;
                        }
                    }

                    while (std::cout << "Do you want to continue work with jsons?: '\n' 0 - Yes, 1 - No" && !(std::cin >> json_choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }
                } while (json_choice == 0);

                break;
            }

            case 4: {
                fs::path path_to_zip;
                size_t zip_choice;

                do {
                    while (std::cout << "Enter path to a archive (Example - C:\\archive.zip): " && !(std::cin >> path_to_zip)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    std::cout << "What do you want to do with this archive?" << std::endl;
                    std::cout << "1: Select the file to be archived" << std::endl;
                    std::cout << "2: Select a file that has been added to an existing archive" << std::endl;
                    std::cout << "3: Unpack the archive to the specified path" << std::endl;
                    std::cout << "4: Delete this archive" << std::endl;

                    size_t choice;

                    while (std::cout << "Enter your choice: " && !(std::cin >> choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }

                    switch (choice) {
                        case 1: {
                            fs::path path_to_file;

                            while (std::cout << "Enter path to a file (Example - C:\\example.txt): " && !(std::cin >> path_to_file)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            ZIP zip(path_to_zip, path_to_file);
                            std::cout << "Archive " << path_to_zip << " has been successfully created" << std::endl;

                            break;
                        }

                        case 2: {
                            fs::path path_to_file;

                            while (std::cout << "Enter path to a file (Example - C:\\example.txt): " && !(std::cin >> path_to_file)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            ZIP zip(path_to_zip);
                            zip.Append(path_to_file);

                            std::cout << "File " << path_to_file << " has been successfully appended to archive " << path_to_zip << std::endl;

                            break;
                        }

                        case 3: {
                            fs::path path_to_extraction;

                            while (std::cout << "Enter path for extraction (Example - C:\\tmp): " && !(std::cin >> path_to_extraction)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            int arg;

                            while (std::cout << "Enter how many files your archive have: " && !(std::cin >> arg)) {
                                if (std::cin.fail()) {
                                    std::cin.clear();
                                    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                                    std::cout << "Invalid input; please re-enter.\n";
                                }
                            }

                            ZIP zip(path_to_zip);
                            zip.Extract(path_to_extraction, arg);

                            break;
                        }

                        case 4: {
                            ZIP zip(path_to_zip);
                            zip.Delete();
                            std::cout << "Archive " << path_to_zip << " deleted successfully" << std::endl;

                            break;
                        }

                        default: {
                            break;
                        }
                    }

                    while (std::cout << "Do you want to continue work with archives?: '\n' 0 - Yes, 1 - No" && !(std::cin >> zip_choice)) {
                        if (std::cin.fail()) {
                            std::cin.clear();
                            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                            std::cout << "Invalid input; please re-enter.\n";
                        }
                    }
                } while (zip_choice == 0);

                break;
            }

            default : {
                while (std::cout << "Do you want to continue work with program?: '\n' 0 - Yes, 1 - No" && !(std::cin >> main_choice)) {
                    if (std::cin.fail()) {
                        std::cin.clear();
                        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                        std::cout << "Invalid input; please re-enter.\n";
                    }
                }
            }
        }
    } while(main_choice == 0);
}
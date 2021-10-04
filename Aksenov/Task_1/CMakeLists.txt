cmake_minimum_required(VERSION 3.20)
project(OS_FirstTask)

set(CMAKE_CXX_STANDARD 20)

add_subdirectory(ThirdPartyLibraries/json)
add_subdirectory(ThirdPartyLibraries/zip)

add_executable(OS_FirstTask main.cpp)
target_link_libraries(OS_FirstTask nlohmann_json zip)
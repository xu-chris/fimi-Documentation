cmake_minimum_required (VERSION 3.5)
project(pose-estimator-server-mock)

# Set the C++ standard to C++20
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Enable all compiler warnings
if (MSVC)
	set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /W4")
else()
	set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall")
endif()

# Under MinGW, statically link against the standard libraries
if (MINGW)
	set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -static-libgcc -static-libstdc++")
endif()

# Have a separate binary directory
IF(MSVC)
	# Yet another MSVC thingy http://stackoverflow.com/questions/3457013/visual-studio-2008-creates-a-additional-bin-debug-folder-although-i-have-no-pre
	SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin)
ELSE()
	SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin/${CMAKE_BUILD_TYPE})
ENDIF()

SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_LIBRARY_OUTPUT_DIRECTORY})


# Force WebSocket++ to use 100% C++11 mode, so that it doesn't try to look for Boost
# (Note that under MinGW, Boost.Thread is still required, due to a bug in MinGW that prevents the C++11 version from being used)
add_definitions(-D_WEBSOCKETPP_CPP11_STRICT_)

# Add the third-party headers directory to the search path
include_directories(SYSTEM "${PROJECT_SOURCE_DIR}/external")

# Build the server executable
add_executable(${PROJECT_NAME} src/core/main.cpp src/periphery/WebsocketServer.cpp)

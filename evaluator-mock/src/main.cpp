// xnect.cpp : Defines the entry point for the console application.
//

#include "CWebSocketServer.hpp"
#include "domainobject/DelayPerData.h"
#include <iostream>
#include <fstream>
#include <utility>
#include <vector>
#include <thread>
#include <chrono>

std::string recordingsFilePath = "./";

std::vector<DelayPerData> readFromFile(const std::string& fileNameWithPath) {
    std::fstream file (fileNameWithPath, std::ios::in);

    if (!file.is_open()) {
        std::cerr << "Cannot read file. Check if it exist or record a session first.";
    }

	std::vector<DelayPerData> data;

	std::string line;
	while (std::getline(file, line)) {
        DelayPerData singleData(line);
        data.push_back(singleData);
    }
	file.close();
    return data;
}


[[noreturn]] void playSimulation(const std::string& recordingFileNameWithPath) {

    std::vector<DelayPerData> data = readFromFile(recordingFileNameWithPath);

    Common::CWebSocketServer m_WSTransceiver;
    std::cout << "INFO: Attempting to started websocket server on 8080." << std::endl;
    m_WSTransceiver.Initialize();
    m_WSTransceiver.StartServer("8080", "..");

    while(true) {
        for (const DelayPerData& singleData : data) {
            std::cout << "Sending data after " + std::to_string(singleData.delay) + "ms\n";
            std::this_thread::sleep_for(std::chrono::milliseconds((int) singleData.delay));
            m_WSTransceiver.SendData(singleData.data);
        }
    }
}

int main()
{
    playSimulation(recordingsFilePath + "test.mock");

	return 0;
}


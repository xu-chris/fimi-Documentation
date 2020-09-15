#include "../periphery/WebsocketServer.h"
#include "../domainvalue/DelayPerData.h"

#include <iostream>
#include <thread>
#include <asio/io_service.hpp>
#include <utility>
#include <fstream>
#include <chrono>
#include <thread>

//The port number the WebSocket server listens on
#define PORT_NUMBER 8080

void broadcastMockMessages(WebsocketServer& server, const std::vector<DelayPerData>& data) {

    while(true) {
        // Skip sending the messages
        if (server.numConnections() == 0) continue;

        for (const auto& singleData : data) {
            if (server.numConnections() == 0) break;

            std::cout << "Sending data after " + std::to_string(singleData.delay) + "ms\n";
            std::this_thread::sleep_for(std::chrono::milliseconds((int) singleData.delay));
            server.broadcastMessage(singleData.data);
        }
    }
}

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

int main(int argc, char* argv[])
{
	//Create the event loop for the main thread, and the WebSocket server
	asio::io_service mainEventLoop;

    std::vector<DelayPerData> data = readFromFile("./test.mock");

    WebsocketServer server;

    //Start the networking thread
    std::thread serverThread([&server]() {
        server.run(PORT_NUMBER);
    });

    //Start the event loop for the main thread
    asio::io_service::work work(mainEventLoop);

    mainEventLoop.post([&server, data]{
        broadcastMockMessages(server, data);
    });

    mainEventLoop.run();

	return 0;
}

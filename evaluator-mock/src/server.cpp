#include "WebsocketServer.h"
#include "domainvalue/DelayPerData.h"

#include <iostream>
#include <thread>
#include <asio/io_service.hpp>
#include <utility>
#include <fstream>
#include <chrono>

//The port number the WebSocket server listens on
#define PORT_NUMBER 8080

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
	WebsocketServer server;
	
	//Register our network callbacks, ensuring the logic is run on the main thread's event loop
	server.connect([&mainEventLoop, &server](const ClientConnection& conn)
	{
		mainEventLoop.post([conn, &server]()
		{
			std::clog << "Connection opened." << std::endl;
			std::clog << "There are now " << server.numConnections() << " open connections." << std::endl;
			
			//Send a hello message to the client
            server.sendMessage(conn, "hello");
		});
	});
	server.disconnect([&mainEventLoop, &server](const ClientConnection& conn)
	{
		mainEventLoop.post([conn, &server]()
		{
			std::clog << "Connection closed." << std::endl;
			std::clog << "There are now " << server.numConnections() << " open connections." << std::endl;
		});
	});
	server.message("message", [&mainEventLoop, &server](const ClientConnection& conn, const Json::Value& args)
	{
		mainEventLoop.post([conn, args, &server]()
		{
			std::clog << "message handler on the main thread" << std::endl;
			std::clog << "Message payload:" << std::endl;
			for (auto key : args.getMemberNames()) {
				std::clog << "\t" << key << ": " << args[key].asString() << std::endl;
			}
			
			//Echo the message pack to the client
            server.sendMessage(conn, "message");
		});
	});
	
	//Start the networking thread
	std::thread serverThread([&server]() {
		server.run(PORT_NUMBER);
	});

    std::vector<DelayPerData> data = readFromFile("./test.mock");

    //Start the event loop for the main thread
    asio::io_service::work work(mainEventLoop);

    while(true) {
        for (const auto& singleData : data) {
            std::cout << "Sending data after " + std::to_string(singleData.delay) + "ms\n";
            std::this_thread::sleep_for(std::chrono::milliseconds((int) singleData.delay));
            server.broadcastMessage(singleData.data);
        }
    }

    mainEventLoop.run();
	
	return 0;
}

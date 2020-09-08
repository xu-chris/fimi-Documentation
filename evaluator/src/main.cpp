// xnect.cpp : Defines the entry point for the console application.
//

#include "xnect.hpp"
#include <sys/timeb.h>
#include <time.h>

#define WEB_CAM 0

std::string images_to_load = "../../data/images";
std::string videoFilePath = "../../data/videos/pepper_front_1.mp4";

void drawBones(cv::Mat &img, XNECT &xnect, int person)
{
	//int numOfJoints = xnect.getNumOf3DJoints() - 2; // don't render feet, can be unstable
	int numOfJoints = xnect.getNumOf3DJoints();

	for (int i = 0; i < numOfJoints; i++)
	{
		int parentID = xnect.getJoint3DParent(i);
		if (parentID == -1 ) continue;
		// lookup 2 connected body/hand parts
		cv::Point2f partA = xnect.ProjectWithIntrinsics(xnect.getJoint3DIK(person,i));
		cv::Point2f partB = xnect.ProjectWithIntrinsics(xnect.getJoint3DIK(person, parentID));
		
	
		if (partA.x <= 0 || partA.y <= 0 || partB.x <= 0 || partB.y <= 0)
			continue;

		line(img, partA, partB, xnect.getPersonColor(person), 4);

	}

}

void drawJoints(cv::Mat &img, XNECT &xnect, int person)
{

	//int numOfJoints = xnect.getNumOf3DJoints() - 2; // don't render feet, can be unstable
	int numOfJoints = xnect.getNumOf3DJoints();
	for (int i = 0; i < numOfJoints; i++)
	{
		int thickness = -1;
		int lineType = 8;
		cv::Point2f point2D = xnect.ProjectWithIntrinsics(xnect.getJoint3DIK(person, i));
		cv::circle(img, point2D, 6, xnect.getPersonColor(person), -1);
	
	}
}

void drawPeople(cv::Mat &img, XNECT &xnect)
{
	for (int i = 0; i < xnect.getNumOfPeople(); i++)
       if (xnect.isPersonActive(i))
	     {	     	
	     	drawBones(img, xnect,i);
	     	drawJoints(img, xnect,i);     
	     }

}

void writeFPS(cv::Mat &frame, double time)
{
	cv::Point position = cv::Point(10, 20);
	time = std::round(time * 100) / 100;
	std::string fpsText = "XNECT FPS: "+ std::to_string(time);
    writeTextOnImage(frame, fpsText, position);
}

void writeCameraFPS(cv::Mat &frame, double time)
{
	cv::Point position = cv::Point(10, frame.rows - 10);
	time = std::round(time * 100) / 100;
	std::string fpsText = "Camera FPS: " + std::to_string(time);
    writeTextOnImage(frame, fpsText, position);
}

void writeTextOnImage(cv::Mat &frame, std::string text, cv::Point position) {
    cv::putText(frame, text, cv::Point(position.x + 1, position.y + 1), cv::FONT_HERSHEY_PLAIN, 1, cv::Scalar(0, 0, 0), 1);
    cv::putText(frame, text, cv::Point(position.x, position.y), cv::FONT_HERSHEY_PLAIN, 1, cv::Scalar(255, 255, 255), 1);
}

void processImage(cv::Mat &frame, XNECT &xnect, bool showImage = true, std::string windowName = "main", bool sendToUnity = true) {
    int64 start = cv::getTickCount();

    if (frame.empty()) break;

    xnect.processImg(frame);

    if (sendToUnity) {
        xnect.sendDataToUnity();
    }
    drawPeople(frame, xnect);
    cv::resize(frame, frame, cv::Size(frame_width, frame_height), 0, 0, cv::INTER_LINEAR);
    int64 end = cv::getTickCount();
    double fps = cv::getTickFrequency() / (end - start);

    writeCameraFPS(frame, cap.get(cv::CAP_PROP_FPS));
    writeFPS(frame, fps);

    if (showImage) {
        cv::namedWindow(windowName, cv::WINDOW_NORMAL);
        imshow(windowName, frame);
    }
}

bool playLive(XNECT &xnect)
{
	cv::VideoCapture cap;

	if (!cap.open(0))
	{
		std::cout << "Can't open webcam!\n";
		cv::waitKey(0);
		return false;
	}
	if (!(cap.set(CV_CAP_PROP_FRAME_WIDTH, xnect.processWidth) && cap.set(CV_CAP_PROP_FRAME_HEIGHT, xnect.processHeight)))
	{
		
		std::cout << "[ ERROR ]: the connected webcam does not support " << xnect.processWidth << " x " << xnect.processHeight << " resolution." << std::endl;
		cv::waitKey(0);
		return false;
	}
	// open the default camera, use something different from 0 otherwise;
	// Check VideoCapture documentation.

	for (;;)
	{

		cv::Mat frame;
		cap >> frame;

		processImage(frame, xnect);

		char ch = cv::waitKey(1);

		if (ch == 27) break; // stop capturing by pressing ESC 
		
		if (ch == 'p' || ch == 'P')
		{
			xnect.rescaleSkeletons();
			std::cout << "rescaling" << std::endl;
		
		}
		
		if (ch == 'r' || ch == 'R')
		{
			xnect.resetSkeletons(); 		
			std::cout << "resetting" << std::endl;
		}
		

	}
	// the camera will be closed automatically upon exit

	return true;
}

void readImageSeq(XNECT &xnect)
{
	vector<cv::String> fn;
	cv::glob(images_to_load + "/*.jpg", fn, false);

	vector<cv::Mat> images;
	size_t count = fn.size();

	for (int i = 0; i < count; i++)
	{
		cv::Mat currentImage = imread(fn[i]);

        processImage(currentImage, xnect);

		cv::waitKey(1);
	}
}

void readVideoSeq(XNECT &xnect, std::string videoFilePath)
{
    cv::VideoCapture cap(videoFilePath);

    if (!cap.isOpened()) {
        std::cout << "Error opening video file with path " << videoFilePath << ". Probably the file is missing?";
        return;
    }

    // Default resolution of the frame is obtained.The default resolution is system dependent.
	cv::Mat frame;
	cap >> frame;
	int frame_width = frame.cols;
    int frame_height = frame.rows;

    cv::VideoWriter video("out.avi",CV_FOURCC('M','J','P','G'),10, cv::Size(frame_width,frame_height));

    while(1) {

        cv::Mat frame;
        cap >> frame;

        processImage(frame, xnect);

		video.write(frame);

        // Press  ESC on keyboard to exit
		char c = (char) cv::waitKey(25);
        if(c == 27)
            break;
    }

	cap.release();
	video.release();
	cv::destroyAllWindows();

    return;
}

int main()
{
	XNECT xnect;


	if (WEB_CAM)
	{
		if (playLive(xnect) == false)
			return 1;
	}
	else
		readVideoSeq(xnect, videoFilePath);


	xnect.save_joint_positions(".");
	xnect.save_raw_joint_positions(".");
		
	return 0;
}


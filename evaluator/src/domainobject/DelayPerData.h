//
// Created by Chris on 10.09.20.
//

#include <string>

#ifndef POSE_ESTIMATOR_DELAYPERDATA_H
#define POSE_ESTIMATOR_DELAYPERDATA_H

class DelayPerData {
public:
    const std::string& data;
    double delay;
    const std::string delimiter = " ";

    DelayPerData(data, delay) : data(data) delay(delay) {};

    const std::string toString() { return std::to_string(delay) + delimiter + data; };

    const void fromString(std::string input) {
        int position = 0;
        position = input.find(singleData.delimiter, position);
        std::string delayString = input.substr(0, position);
        std::string dataString =  input.substr(position + delimiter.length(), input.length() - 1);
        this->delay = (double) delayString;
        this->data = dataString;
    }
};

#endif //POSE_ESTIMATOR_DELAYPERDATA_H

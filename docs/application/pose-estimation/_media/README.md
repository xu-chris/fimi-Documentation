# XNECT C++ LIBRARY 
A library for real-time multi-person 3D tracking from monocular RGB. The system is described in the [SIGGRAPH'20](http://gvv.mpi-inf.mpg.de/projects/XNect/) paper of the same name. We have also included a new improved network for Stage II of the pipeline, which you can optionally select in place of the Stage II network described in the paper.

## Installation / Compilation
XNECT is provided as a library with an example application of how to use it with a pre-recorded image sequence and web-cam.
### Platform 
Windows: VC140 

Linux (GCC 6.3.0) (WoodenMan not tested on Linux, but should work)
### Dependencies
Caffe
Eigen (Included)
RtPose (CMU OpenPose, see Citation section) (Included)
OpenCV 3


### Installation
## Windows
You need to add RtPose layers (provided in extern/caffe) to your caffe version to be able to run the model.
Installing the layer in caffe:
1) copy folders extern/caffe/include and extern/caffe/src to your caffe folder accordingly.(e.g.  C:\caffe) 
2) edit caffe.proto at C:\caffe\src\caffe\proto (check example of proto in extern/caffe)
 Inside the function message LayerParameter add these layers with correct next id:
 Example:
 ```
	// LayerParameter next available layer-specific ID: 149 (last added: recurrent_param)
	message LayerParameter {

	/* some standard layers */
	...
      // CPM extra code: added layers
  optional ImResizeParameter imresize_param = 147;
  optional NmsParameter nms_param = 148;
   
   }
```
    
3)	At the end of the file caffe.proto add:
```	
	// CPM extra code: new layers description
message NmsParameter {
  optional float threshold = 1 [default = 0.5];
  optional uint32 num_peak = 2 [default = 100];
  optional uint32 max_peaks = 3 [default = 20];
  optional uint32 num_parts = 4 [default = 15];
}

message ImResizeParameter {
  optional uint32 target_spatial_width = 1 [default = 368];
  optional uint32 target_spatial_height = 2 [default = 368];
  optional float factor = 3 [default = 0];
  optional float start_scale = 4 [default = 1];
  optional float scale_gap = 5 [default = 0.1];
}
```
You need to have caffe built for Windows. Identify the path of caffe cmake cache and run cmake initialized with the caffe cache:

For instance:
```
cmake -G "Visual Studio 14 Win64" -C /c/Users/<your_usename>/.caffe/dependencies/libraries_v140_x64_py27_1.1.0/libraries/caffe-builder-config.cmake ..
```

## Linux
 Same steps as for windows. 
 Some tips which worked for us to run it on linux. 
 
 Works for cuda 8 or 9. 
 To compile with cuda 9 it takes a bit more effort.
 Example for cuda 9.
 
 Tips: 
 1) Compile boost 1.5.8. Make sure caffe finds correct version. You might change cmake in caffe in dependencies.cmake and specify rigidly this version.
 2) Compile protobuf. We used 3.1.0 version. 
 ```
      ./autogen.sh
      
      ./configure CFLAGS="-fPIC" CXXFLAGS="-fPIC"
      
      make
```
  3) make sure caffe doesn't have clip_layer. If it is there delete it following these steps:
  
      3.1) remove clip_layer.hpp and clip_layer.cpp from caffe_root/src/caffe/layers.
   
      3.2) In caffe_root/src/caffe/layers/clip_layer.cu comment out the include and the two templates and the instantiate. Comment out the `include` in `caffe_root/src/caffe/layer_factory.cpp`.
   
       3.3) in src/caffe/proto/caffe.proto comment out the ClipParameter message and the optional clipparameter.
       If there are any other errors just comment out the source line.
   
 4) finally run cmake for caffe:
 ```
 cmake -DProtobuf_LIBRARY_DEBUG=/path_to_protobuf/src/.libs/libprotobuf.so -DProtobuf_PROTOC_EXECUTABLE=/path_to_protobuf/src/.libs/protoc -DProtobuf_LIBRARY_RELEASE=/path_to_protobuf/src/.libs/libprotobuf.a  -DProtobuf_LITE_LIBRARY_RELEASE=/path_to_protobuf/src/.libs/libprotobuf-lite.a -DBOOST_INCLUDEDIR=/path_to_boost_1_58_0/  -DBOOST_LIBRARYDIR=/path_to_boost_1_58_0/stage/lib -DProtobuf_INCLUDE_DIR=/path_to_protobuf/src/ -DProtobuf_PROTOC_LIBRARY_RELEASE=/path_to_protobuf/src/.libs/libprotoc.so .. 
```
5) To compile the project you might need to compile OpenCV 3 and give the path as:
```
   cmake -DOpenCV_DIR=/path_to_install_folder_OpenCV/share/OpenCV  -DCaffe_DIR=/path_to_caffe/build ..
```

## Features
1. Build live or offline applications for Multi-Person 3D Human Pose estimation
2. Real-time performance on Pascal and later generation NVIDIA GPUs (1080, 1080-MaxQ, 1080Ti, 2070, 2080, 2080Ti etc.). Perhaps even on a 980 (Maxwell), but that has not been tested.
3. Various aspects are programmatically controllable.
4. WoodenMan (Unity character control) example is included

### Running the Example with Unity Characters
1. First start the XNECT demo, and then start the WoodenMan demo.
2. See the example code that accompanies the library.

## API Details
xnect_implementation.h and xnect.hpp along with ConfigFile.params (location: data/FullBodyTracker/ConfigFile.params) provides details of the API and various hyperparameters. Here we list some to get you started.

### Primary Functions and Hyperparameters
* processImg(cv::Mat img): Takes the image as input, gets predictions from the network, runs a skeleton fitting step on the predictions.
* sendDataToUnity(): Sends joint angles to Unity (Woodenman) for visualization
* resetSkeletons(): Resets the tracking parameters to those from the config file.
* rescaleSkeletons(): Rescale skeletons (according to the CNN predictions).
* See xnect_implementation.h and xnect.h for more functions


### Saving Predictions and Other Information to Disk
* save_joint_positions(<file_name>): Writes out the 3D joint positions of the skeletons (after skeleton fitting) for the sequence
* save_raw_joint_positions(<file_name>): Writes out the unfiltered 2D and 3D predictions from the network for each frame
* See xnect_implementation.h and xnect.h for more functions that provide access to joint positions, joint angles etc.
* See src/read_full_system_predictions.m for an example of how to parse the joint position predictions written out by the API

### Order of joint positions
* 2D joints - { 0:'head', 1:'neck',  2:'Rsho',  3:'Relb',  4:'Rwri',  5:'Lsho',  6:'Lelb', 7:'Lwri', 8:'Rhip', 9:'Rkne', 10:'Rank', 11:'Lhip', 12:'Lkne', 13:'Lank' }
* 3D joints - { 0:'head TOP', 1:'neck',  2:'Rsho',  3:'Relb',  4:'Rwri',  5:'Lsho',  6:'Lelb', 7:'Lwri', 8:'Rhip', 9:'Rkne', 10:'Rank', 11:'Lhip', 12:'Lkne', 13:'Lank', 14: Root, 15: Spine, 16:'Head', 17: 'Rhand', 18: 'LHand', 19: 'Rfoot', 20: 'Lfoot' }


### Config File Parameters
  See the config file at ./data/FullBodyTracker/XNECT.params for the configurable options and their documentation. Broadly, the configuration file controls
  * live camera vs pre-recorded sequences
  * camera calibration, horizontal flipping of input
  * the maximim number of people to track/visualize
  * the paths to Stage I and Stage II networks
  * options to toggle filtering
  * hyperparameters of skeleton fitting

## Example Code
See main.cpp for an example of how to run XNect on a pre-recorded sequence. The code can be adapted to the run XNect live, e.g., with the OpenCV webcam class.

## Other Information
See xnect_new_2ndstage_eval.xlsx for details evaluations of the paper version of Stage II, and the new version.


## License Terms
Permission is hereby granted, free of charge, to any person or company obtaining a copy of this software and associated documentation files (the "Software") from the copyright holders to use the Software for any non-commercial purpose. Publication, redistribution and (re)selling of the software, of modifications, extensions, and derivates of it, and of other software containing portions of the licensed Software, are not permitted. The Copyright holder is permitted to publically disclose and advertise the use of the software by any licensee. 

Packaging or distributing parts or whole of the provided software (including code, models and data) as is or as part of other software is prohibited. Commercial use of parts or whole of the provided software (including code, models and data) is strictly prohibited. Using the provided software for promotion of a commercial entity or product, or in any other manner which directly or indirectly results in commercial gains is strictly prohibited. 

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Citation
If the software is used, the licesnee is required to cite the use of XNect and the following publication in any documentation 
or publication that results from the work:

XNect: Real-time Multi-Person 3D Motion Capture with a Single RGB Camera (ACM Trans. on Graphics, SIGGRAPH ‘20)
D. Mehta; O. Sotnychenko; F. Mueller; W. Xu; M. Elgharib; P. Fua; H.P. Seidel; H. Rhodin; G. Pons-Moll; C. Theobalt

Since the system uses some components from the work of Cao et al. (https://github.com/CMU-Perceptual-Computing-Lab/caffe_rtpose), you are also required to cite:

Realtime Multi-person 2D Pose Estimation Using Part Affinity Fields (CVPR 2017)
Z. Cao; T. Simon; S. Wei; Y. Sheikh

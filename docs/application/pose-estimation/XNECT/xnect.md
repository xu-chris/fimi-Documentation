# XNECT 3D Multi-Person Pose Estimation

!> Status: Draft

[XNECT](http://gvv.mpi-inf.mpg.de/projects/XNect/)

<video controls>
  <source src="first_result_pepper.mp4" type="video/mp4">
Your browser does not support the video tag.
</video>

XNECT is a custom made 3D pose estimation model which tries to fit the estimated 2D pose to a 3D skeleton. For this it uses some frames beforehand to scale the skeleton correctly and use this info to fit the following pose estimations onto the skeleton.

XNECT uses a set of parameters for preprocessing and correctly fitting the skeleton. these include the camera image width and height, the first net height as well as intrinsics as matrix. It also contains a set of other parameters that extend or limit the possibilities of XNECT. An example can be seen here:

```
# Configuration file for XNect

# Number of frames to accumulate data for rescaling the length of bones
RescaleFrameNum: 2

# Maximum number of people to track and visualize. More can be tracked, but the scene becomes too clutterd
# to make out anything. Keeping it at 10 would work for most reasonable scenarios
MaxNumPeople: 1

## Live Webcam Camera Calibration
# camera type: webcam or playback
ProcessHeight: 360
ProcessWidth: 640
FirstNetHeight: 320
FirstNetWidth: 544
OrigIntrinsics:  576, 0, 320, 0, 576, 240, 0, 0, 1

FirstNet: ../../data/CNNModels/XNECT/StageI/paper/net.prototxt
FirstNetWeights: ../../data/CNNModels/XNECT/StageI/paper/snapshot.caffemodel
SecondNet: ../../data/CNNModels/XNECT/StageII/new/net.prototxt
SecondNetWeights: ../../data/CNNModels/XNECT/StageII/new/snapshot.caffemodel

# Write out 2D and 3D joint positions. It can slow processing down slightly if the joint positions are being written out.
WritePositions: 1

# Horizontally flip input to switch between a mirror interaction like setup and true view setup
FlipInput: 0

# To toggle filtering for 2D and 3D pose estimation
UseOneEuroFilter2D: 1
UseOneEuroFilter3D: 1

# Skeleton fitting Parameters. You can try tweaking these to tune the skeleton fitting step to exhibit the
# characteristics you desire from the skeletal motion
IKTermWeight: 0.85
IK2DTermWeight: 25e-6
SmoothnessTermWeight: 1e-5
JointLimitsTermWeight: 1
DepthPenaltyWeight:6e-10
```

These parameters can be tweaked to get an optimal solution with the given camera and to speed up the processing even more.
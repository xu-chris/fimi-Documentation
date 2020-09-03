# fimi - Safety First Smart Fitness Mirror
## Installation for RTX-2080

### 1. Prerequisites
1. Python 2.7
2. Visual Studio 2015 with Visual C++, Python and Git Plugin
3. NVIDIA CUDA 9.2
4. CMake
5. numpy x64

### 2. Build Caffe
This is based on https://tkcheng.wordpress.com/2019/04/11/caffe-on-windows-rtx-2080ti-cuda9-2-vs2015/

1. After you have installed all dependencies above, clone Caffe (from XNECT branch: https://github.com/creichel/caffe/tree/net/xnect).
2. Call
  ```bash
  .\scripts\build_win.cmd
  ```
  so Caffe downloads automatically the dependencies it needs.

Building Caffe will fail since we want to use a customized version of boost that supports our CUDA version and architecture.

For this, open `~\.caffe\dependencies\libraries_v140_x64_py35_1.1.0\libraries\include\boost-1_61\boost\config\compiler\nvcc.hpp` and comment out the last three lines:

```
//#if !defined(__CUDACC_VER__) || (__CUDACC_VER__ < 70500)
//#   define BOOST_NO_CXX11_VARIADIC_TEMPLATES
//#endif
```

### 3. Build XNECT
1. Clone full git repository:
```bash
git clone --recurse-submodules -j8 https://github.com/creichel/FiMi-Fitness-Smart-Mirror.git
```
2. Build it by calling
```
cd FiMi-Fitness-Smart-Mirror\evaluator\extern\xnect && mkdir build && cd build && cmake -G "Visual Studio 14 Win64" -C ~\.caffe\dependencies\libraries_v140_x64_py27_1.1.0\libraries\caffe-builder-config.cmake ..
```

### Run it

### Troubleshooting issues
#### Caffe
Please note that caffe only supports Python 3.5 for now. If you try it first with any higher version. delete the `build` folder and restart the process.

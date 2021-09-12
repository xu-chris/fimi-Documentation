# fimi - Smart Mirror Application For Workouts With Form Correction Hints

**Check out the [Full Documentation](https://xu-chris.github.io/fimi-Documentation/) of fimi.**

fimi is an application which tries to resemble a coach while you doing workout by analyzing your posture and giving you feedback about what you should watch more often. To run it, you need a **webcam**, a big screen and your smartphone (and some room space). You control the big screen with your smartphone by simply scanning the displayed QR code (being in the same wifi net is necessary).

fimi generates a 3d pose estimation based on the webcam image and checks every frame your posture based on a fixed set of rules per exercise. In the end, you receive a small summary of your results on your smartphone.

The application also compares your posture with your previous trainings while still being designed with privacy in mind. All your information is stored only temporarily while running the application on your PC or Mac, whereas the training data is stored on your smartphone directly. This also means that you could theoretically log in at your friend's house and have your profile with you.

This is the documentation for the whole project.

## The different parts
fimi has 3 parts which depend on each other:
1. The [pose estimation server](https://github.com/xu-chris/fimi-Server)
2. The [client application](https://github.com/xu-chris/fimi-Client)
3. The [controller application](https://github.com/xu-chris/fimi-Controller)

For development purposes, you can use the [pose estimation mock server](https://github.com/xu-chris/fimi-Mock-Server) during development of the [client application](https://github.com/xu-chris/fimi-Client).

## Used technology

In each repository you will find a dedicated secion called `Used technology` that resembles the used technology in each part of the system. Main driver and enabling technology is the [XNECT concept and library by the Max Planck Institute](https://gvv.mpi-inf.mpg.de/projects/XNect/) and [Unity3d](https://unity.com).

### Documentation
- [Docsify](https://docsify.js.org/#/) has been used for generating the documentation.
  - [npm](https://www.npmjs.com) for distribution of Docsify.
  - [Node.js](https://nodejs.org/) as the technical backbone.

## Install it
### 1. Install Node.js and NPM (if not given)
- Download it here: [Official Node.js website](https://nodejs.org/en/)

### 2. Install docsify via npm
Open your console and type:
```bash
npm i docsify-cli -g
```

## Start it
Open your console and type:
```bash
docsify serve docs
```

## Special Thanks
This project has been supported by the help of some people:
- [Celeste Mason](https://www.inf.uni-hamburg.de/en/inst/ab/hci/people/mason.html)
- [Prof. Frank Steinicke](https://www.inf.uni-hamburg.de/en/inst/ab/hci/people/steinicke.html)
- [Dr. Peter Konrad](https://www.physiotools.com/company/authors/peter-konrad)
- [Dushyant Mehta](https://people.mpi-inf.mpg.de/~dmetha/)

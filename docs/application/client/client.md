# Client application
!> Status: Draft

The client application contains the user interface and the [pose evaluation](pose-evaluation/pose-evaluation.md) for fimi. It has multiple states, based on the definition of a [training](../../introduction/training-definition.md):

- [Welcome screen](welcome-screen.md)
- [Calibration scene](calibration.md)
- [Pre-Training](pre-training.md)
- [In-Training](in-training.md), with the sub-states:
    - [Pre-Exercise](pre-exercise.md)
    - [In-Exercise](in-exercise.md)
    - [Post-Exercise](post-exercise.md)
- [Post-Training](post-training.md)

## Configuration
During all scenes, the application is connecting to the pose estimation Web Socket server to retrieve the data. It also instantiates dummy skeletons according to the maximum number of people that can be detected. These information are stored in the `Application.yaml` file.

Example configuration:
```yaml
webSocket:
  url: "ws://localhost:8080/"
maxNumberOfPeople: 2
```

### Parameters
- `webSocket`: Contains the information for the Web Socket client to connect to the server
	- `url`: The URL of the server (yes, the server can be also hosted elsewhere!)
- `maxNumberOfPeople`: The number of maximum people detected by the pose estimation server. Maximum 10 people is recommended.

# In-Training
Defined as the user state in which the user is performing a set of exercises to a specified training. For each exercise, [Pre-Exercise](pre-exercise.md), [In-Exercise](in-exercise.md) and [Post-Exercise](post-exercise.md) are the dedicated user interfaces.

## Configuration
During In-Training, the application is connecting to the pose estimation Web Socket server to retrieve the data. It also instantiates dummy skeletons according to the maximum number of people that can be detected. All of that can be configured by the `InTraining.yaml` YAML configuration.

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

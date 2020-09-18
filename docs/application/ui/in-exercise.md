# In-Exercise
In-Exercise is defined as the state in which the user is when performing the repetitions of a selected exercise. In fimi, an exercise is combined with other exercises during a training and can be interrupted after a certain timeframe or can be ended after a number of repetitions.

After starting the exercise, this screen shows the necessary information about the current progress of the training.

## Situation description
- Trainee is performing the exercise
- Trainee might not have an eyesight to mirror / screen
- Trainee is able to hear instructions 

## Problem statement
### Information
- Trainee needs to be notified about their mis-movement / wrong pose in real-time
- Trainee needs to know how to correct the movement / pose to prevent injuries to happen
- Trainee wants to know how long the exercise remains and how many repetitions are missing
- Trainee might want to know about their qualitative progress
- Trainee might want to get motivated to keep up or increase the current performance 

### Interaction
- Trainee needs to pause or cancel the exercise in case of fatigue.

## Possible solutions

### Motivation
1. Collision tracking based form pose goals like in [Homecourt.ai](https://www.homecourt.ai)
    - Could lead to more mirror facing activity but could also prevent trainees to move correctly
2. Scoring system for form correctness
3. Scoring system for reaching the areas with the body (like Homecourt.ai)
4. Virtual agent saying motivational things

### Correction hints

1. Audio feedback about how to correct the form. 
	- Example: "Your knee is over your ankle. Put your hip a bit more back."
	- If trainee has corrected the pose, the trainee needs to know that their form is now correct. Example: "That's better!"
2. Visual feedback about the affected body part 
3. Visual feedback in which direction the affected body part should be moved
	- Example: ![Marking of pose with arrow](_media/marking_with_arrow.png ':size=50%')
3. Visual representation of the body pose to match by using the Hull effect
	- Example: ![Hull effect](_media/hull_effect.png ':size=50%')
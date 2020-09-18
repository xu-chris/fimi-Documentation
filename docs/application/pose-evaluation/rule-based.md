# Rule based pose evaluation

## Configuration
fimi is using YAML as a format for defining and checking the different exercises and their applied different rules.

An example configuration:

```yaml
exercises:
  - type: squatArmRaise
    rules:
      - rule:
          !rangeOfMotionRule
          lowerThreshold: 0
          upperThreshold: 95
        bones:
          - LeftLowerLeg
          - LeftThigh
      - rule:
          !rangeOfMotionRule
          lowerThreshold: 0
          upperThreshold: 95
        bones:
          - RightLowerLeg
          - RightThigh
      - rule:
          !angleRule
          expectedAngle: 0
          lowerTolerance: 10
          upperTolerance: 10
        bones:
          - LeftElbow
          - LeftForearm
```

### Parameters
- `exercises`: List of named exercises, containing following fields:
	- `type`: The type of the exercise (defined in `ExerciseType.cs`. Currently, `SQUAT_ARM_RAISE` is the only parameter to be set.
	- `rules`: A list of rules for checking the performed exercise
		- `rule`: The rule definition, lead by a tag (`!`) that specifies the rule type
		- `bones`: A list of bones that are considered when checking `rule`'s `IsInvalidated()`.

### Rules
A rule is defined as a set of parameters per `rule` and the considered `bones`. 

These are the rule types supported:
- `angleRule`
- `rangeOfMotionRule`

#### Angle rule
An angle rule is considered as a rule that checks if two bones are in a specified angle to each other. The smallest angle is used for consideration and the angle itself cannot be more than 180 degrees.

**Parameters:**
- `expectedAngle`: The ideal angle to reach
- `lowerTolerance`: The lower tolerance range the angle between the bones can reach. 
- `higherTolerance`: The higher tolerance range.

Example for the lower and higher tolerance usage: a `lowerTolerance: 5` of the `expectedAngle: 90` would consider an angle of 84 degrees as invalidation of the rule.


#### Range of motion rule
This rule can be used to notify the user if they are moving two bones outside of a specified range of motion (like over-flexing the thighs and lower knees).

**Parameters:**
- `lowerThreshold`: the lower threshold of the range
- `higherThreshold`: the higher threshold of the range

It's similar to `Angle rule` but can lead to different user notifications (like marking specified bones as red / neutral color instead of red / green).
using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Rules;
using NUnit.Framework;

namespace Tests.DomainObjects
{
    public class ExerciseReportTest
    {
        private ExerciseReport exerciseReport;

        private readonly AngleRule rule1 = new AngleRule{
            priority = 1,
            bones = new List<string>{"a", "b"},
            colorize = false,
            expectedAngle = 90,
            lowerTolerance = 10,
            upperTolerance = 10,
            notificationText = "b"
        };

        private readonly LinearityRule rule2 = new LinearityRule {
            bones = new List<string>{"a", "b"},
            colorize = false,
            notificationText = "b",
            priority = 2,
            tolerance = 10
        };
        
        private readonly AngleRule rule3 = new AngleRule{
            priority = 2,
            bones = new List<string>{"a", "b"},
            colorize = false,
            expectedAngle = 90,
            lowerTolerance = 10,
            upperTolerance = 10,
            notificationText = "b"
        };

        [SetUp]
        public void SetUp()
        {
            var rulesArray = new Rule[]{rule1, rule2};
            exerciseReport = new ExerciseReport(rulesArray);
        }

        [Test]
        public void ShouldPrioritizeIfBothHaveNotNullValue()
        {
            // GIVEN
            exerciseReport.Count(rule2);
            exerciseReport.Count(rule2);
            var result1 = new ExerciseReport.Result(rule1);
            var result2 = new ExerciseReport.Result(rule2);
            result2.Increment();
            result2.Increment();
            var expectedResult = new [] {
                result1,
                result2
            };
            
            // WHEN
            var result = exerciseReport.Results();
            
            Assert.AreEqual(expectedResult[0].GetType(), result[0].GetType());
            Assert.AreEqual(expectedResult[1].GetType(), result[1].GetType());
        }

        [Test]
        public void ShouldPrioritizeNoneIfOnlyValuesDiffer()
        {
            // GIVEN
            var rulesArray = new Rule[]{rule2, rule3};
            exerciseReport = new ExerciseReport(rulesArray);
            exerciseReport.Count(rule3);
            exerciseReport.Count(rule3);
            var result2 = new ExerciseReport.Result(rule2);
            var result3 = new ExerciseReport.Result(rule3);
            result3.Increment();
            result3.Increment();
            var expectedResult = new []{
                result3,
                result2
            };
            
            // WHEN
            var result = exerciseReport.Results();
            
            Assert.AreEqual(expectedResult[0].GetType(), result[0].GetType());
            Assert.AreEqual(expectedResult[1].GetType(), result[1].GetType());
        }
    }
}
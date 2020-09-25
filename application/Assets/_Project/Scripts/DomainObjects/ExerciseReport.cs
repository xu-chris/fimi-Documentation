using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainObjects.Rules;

namespace _Project.Scripts.DomainObjects
{
    public class ExerciseReport
    {
        private Result[] results;

        public ExerciseReport(IEnumerable<Rule> rules)
        {
            results = rules.Select(rule => new Result(rule)).ToArray();
        }

        public void Count(Rule rule)
        {
            results.First(i => i.rule == rule).Increment();
        }

        public Result[] Results()
        {
            OrderResults();
            return results;
        }

        public void Reset()
        {
            foreach (var result in results)
            {
                result.count = 0;
            }
        }

        public override string ToString()
        {
            OrderResults();
            return results.Aggregate("",
                (current, result) => current + result.count + " times for " + result.rule + "\n");
        }

        private void OrderResults()
        {
            results = results.OrderBy(i => i.rule.priority).ThenByDescending(i => i.count).ToArray();
        }

        public class Result
        {
            internal Rule rule;
            internal float count;

            public Result(Rule rule)
            {
                this.rule = rule;
            }

            public void Increment()
            {
                count += 1;
            }
        }
    }
}
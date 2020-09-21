using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainObjects.Rules;
using UnityEditor.Experimental;
using UnityEngine;

namespace _Project.Scripts.DomainObjects
{
    public class ExerciseReport
    {
        public class Result
        {
            internal readonly Rule rule;
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
        private List<Result> results;

        public ExerciseReport(List<Rule> rules)
        {
            results = new List<Result>();
            foreach (var newResult in rules.Select(rule => new Result(rule)))
            {
                results.Add(newResult);
            }
        }

        public void Count(Rule rule)
        {
            if (results.Exists(i => i.rule == rule))
            {
                results.Find(i => { return i.rule == rule; }).Increment();   
            } 
            else
            {
                var result = new Result(rule);
                result.Increment();
                results.Add(result);
            }
        }

        public List<Result> Report()
        {
            results = results.OrderByDescending(i => i.count).ToList();
            return results;
        }

        public override string ToString()
        {
            results = results.OrderByDescending(i => i.count).ToList();
            return results.Aggregate("", (current, result) => current + (result.count + " times for rule: " + result.rule.ToString() + "\n"));
        }
    }
}
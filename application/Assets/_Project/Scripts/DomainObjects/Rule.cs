using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects
{
    public struct Rule
    {
        public string type;
        public List<string> bones;
        public AngleDefinition angleDefinition;
        
        RuleType RuleType()
        {
            return type.ToRuleType();
        }
    }
}
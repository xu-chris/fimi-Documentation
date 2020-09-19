using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.DomainObjects.Rules;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace _Project.Scripts.Periphery.Configurations
{
    public class ExercisesConfigurationService
    {
        public ExercisesConfiguration configuration;

        public ExercisesConfigurationService(TextAsset configurationFile)
        {
            // Start decoding the yaml file
            configuration = DecodeYaml(configurationFile.text);
        }

        private static ExercisesConfiguration DecodeYaml(string document)
        {
            var namingConvention = new CamelCaseNamingConvention();
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(namingConvention)
                .WithTagMapping("!rangeOfMotionRule", typeof(RangeOfMotionRule))
                .WithTagMapping("!angleRule", typeof(AngleRule))
                .WithTagMapping("!symmetryRule", typeof(SymmetryRule))
                .WithTagMapping("!linearityRule", typeof(LinearityRule))
                .WithTagMapping("!horizontallyRule", typeof(HorizontallyRule))
                .WithTagMapping("!verticallyRule", typeof(VerticallyRule))
                .WithTagMapping("!speedRule", typeof(SpeedRule))
                .Build();

            return deserializer.Deserialize<ExercisesConfiguration>(document);
        }
    }
}
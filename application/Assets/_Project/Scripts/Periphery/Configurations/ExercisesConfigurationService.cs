using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
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
            var deserializer = new DeserializerBuilder().WithNamingConvention(namingConvention).Build();
            
            return deserializer.Deserialize<ExercisesConfiguration>(document);
        }
    }
}
using _Project.Scripts.DomainObjects.Configurations;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace _Project.Scripts.Periphery.Configurations
{
    public class CalibrationConfigurationService
    {
        public CalibrationConfiguration configuration;

        public CalibrationConfigurationService(TextAsset configurationFile)
        {
            // Start decoding the yaml file
            configuration = DecodeYaml(configurationFile.text);
        }

        private static CalibrationConfiguration DecodeYaml(string document)
        {
            var namingConvention = new CamelCaseNamingConvention();
            var deserializer = new DeserializerBuilder().WithNamingConvention(namingConvention).Build();

            return deserializer.Deserialize<CalibrationConfiguration>(document);
        }
    }
}
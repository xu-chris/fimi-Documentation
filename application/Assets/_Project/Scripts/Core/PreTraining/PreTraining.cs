using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.PreTraining
{
    public class PreTraining : Scene
    {
        public TextAsset preTrainingConfigurationFile;
        public Text title;

        private PreTrainingConfiguration calibrationConfiguration;

        public void Start()
        {
            calibrationConfiguration = new PreTrainingConfigurationService(preTrainingConfigurationFile).configuration;
        }
    }
}
using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Calibration
{
    public class Calibration : Scene
    {
        public TextAsset calibrationConfigurationFile;
        public Text title;
        public GameObject dummy;
        public GameObject particleSystemForceField;

        private CalibrationConfiguration calibrationConfiguration;
        private SkeletonOrchestrator skeletonOrchestrator;

        public void Start()
        {
            SetUpWebSocket();
            calibrationConfiguration = new CalibrationConfigurationService(calibrationConfigurationFile).configuration;

            skeletonOrchestrator = new SkeletonOrchestrator(applicationConfiguration.maxNumberOfPeople);
        }

        public void Update()
        {
            var detectedPersons = webSocketClient.detectedPersons;
            skeletonOrchestrator?.Update(detectedPersons);

            if (IsSkeletonCollidingWithDummy()) particleSystemForceField.SetActive(true);
        }

        private bool IsSkeletonCollidingWithDummy()
        {
            return true;
        }
    }
}
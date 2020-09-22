using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Clients;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Scenes
{
    public class InTraining : MonoBehaviour
    {
        public TextAsset exercisesConfigurationFile;
        private ExercisesConfiguration exercisesConfiguration;

        public TextAsset inTrainingConfigurationFile;
        private InTrainingConfiguration inTrainingConfiguration;

        private Vector3 offset;
        private Vector3 parentCamPos;

        private Quaternion parentCamRot;
        private Vector3 parentCamScale;
        private SkeletonOrchestrator skeletonOrchestrator;

        private WebSocketClient webSocketClient;

        public Text reportingTextField;

        public void Start()
        {
            Application.runInBackground = true;

            var inTrainingConfigurationService = new InTrainingConfigurationService(inTrainingConfigurationFile);
            inTrainingConfiguration = inTrainingConfigurationService.configuration;

            var exerciseConfigurationService = new ExercisesConfigurationService(exercisesConfigurationFile);
            exercisesConfiguration = exerciseConfigurationService.configuration;

            webSocketClient = gameObject.AddComponent<WebSocketClient>();
            webSocketClient.webSocketConfiguration = inTrainingConfiguration.webSocket;
            skeletonOrchestrator = new SkeletonOrchestrator(inTrainingConfiguration.maxNumberOfPeople, reportingTextField);
            skeletonOrchestrator.SetCurrentExercise(exercisesConfiguration.exercises[0]);

            parentCamRot = transform.rotation;
            parentCamPos = transform.position;
            parentCamScale = transform.localScale;
        }

        public void Update()
        {
            var detectedPersons = webSocketClient.detectedPersons;
            skeletonOrchestrator?.Update(detectedPersons);
        }

        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.C))
            {
                Debug.Log("Before cam: " + transform.position);
                Debug.Log("Before parent cam: " + parentCamPos);
                transform.position = parentCamPos;
                transform.rotation = parentCamRot;
                transform.localScale = parentCamScale;
                Debug.Log("After: " + transform.position);

                Debug.Log("Reset camera to original position.");
            }
        }
    }
}
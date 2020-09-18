using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Clients;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;

namespace _Project.Scripts.Scenes
{
    public class InTraining : MonoBehaviour
    {
        public TextAsset inTrainingConfigurationFile;
        public TextAsset exercisesConfigurationFile;

        private WebSocketClient webSocketClient;
        private SkeletonOrchestrator skeletonOrchestrator;

        private InTrainingConfiguration inTrainingConfiguration;
        private ExercisesConfiguration exercisesConfiguration;
        
        private Vector3 offset;
        private Vector3 parentCamPos;

        private Quaternion parentCamRot;
        private Vector3 parentCamScale;

        public void Start()
        {
            Application.runInBackground = true;
            
            var inTrainingConfigurationService = new InTrainingConfigurationService(inTrainingConfigurationFile);
            inTrainingConfiguration = inTrainingConfigurationService.configuration;
            
            var exerciseConfigurationService = new ExercisesConfigurationService(exercisesConfigurationFile);
            exercisesConfiguration = exerciseConfigurationService.configuration;

            webSocketClient = gameObject.AddComponent<WebSocketClient>();
            webSocketClient.webSocketConfiguration = inTrainingConfiguration.webSocket;
            skeletonOrchestrator = new SkeletonOrchestrator(inTrainingConfiguration.maxNumberOfPeople);
            skeletonOrchestrator.SetCurrentExercise(exercisesConfiguration.exercises[0]);

            parentCamRot = transform.rotation;
            parentCamPos = transform.position;
            parentCamScale = transform.localScale;
        }

        public void Update()
        {
            var detectedPersons = webSocketClient.detectedPersons;
            var lowestY = webSocketClient.lowestY;
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
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.InTraining
{
    public class InTraining : Scene
    {
        public TextAsset exercisesConfigurationFile;
        private ExercisesConfiguration exercisesConfiguration;

        public TextAsset inTrainingConfigurationFile;
        private InTrainingConfiguration inTrainingConfiguration;

        private Vector3 offset;
        private InTrainingSkeletonOrchestrator skeletonOrchestrator;

        public Text reportingTextField;

        public GameObject notificationPanel;
        private bool notificationShown = false;

        public void Start()
        {
            SetUpWebSocket();
            Application.runInBackground = true;

            var inTrainingConfigurationService = new InTrainingConfigurationService(inTrainingConfigurationFile);
            inTrainingConfiguration = inTrainingConfigurationService.configuration;

            var exerciseConfigurationService = new ExercisesConfigurationService(exercisesConfigurationFile);
            exercisesConfiguration = exerciseConfigurationService.configuration;

            skeletonOrchestrator =
                new InTrainingSkeletonOrchestrator(applicationConfiguration.maxNumberOfPeople, exercisesConfiguration.exercises[0]);
            skeletonOrchestrator.SetCurrentExercise(exercisesConfiguration.exercises[0]);
        }

        public void Update()
        {
            var detectedPersons = webSocketClient.detectedPersons;
            
            if (skeletonOrchestrator == null) return;
            skeletonOrchestrator.Update(detectedPersons);
            var reports = skeletonOrchestrator.exerciseReports;
            if (reports != null)
                CheckReports(reports);
        }

        private void CheckReports(ExerciseReport[] reports)
        {
            ExerciseReport.Result highestInfectedRule = null; 
            foreach (var report in reports)
            {
                if (report == null) continue;
                if (highestInfectedRule == null)
                {
                    highestInfectedRule = report.Report()[0];
                    continue;
                }
                
                if (report.Report()[0].count > highestInfectedRule.count)
                {
                    highestInfectedRule = report.Report()[0];
                }
            }

            if (highestInfectedRule != null) NotifyUser(highestInfectedRule.rule.notificationText);
        }

        private void NotifyUser(string text)
        {
            var animator = notificationPanel.GetComponent<Animator>();

            if (animator == null) return; // Exists
            if (animator.GetBool("show") || notificationShown) return; // Is not already shown
            notificationPanel.GetComponentInChildren<Text>().text = text;
            animator.SetBool("show", true);
            StartCoroutine(HideNotification(animator));
            notificationShown = true;
        }

        private IEnumerator HideNotification(Animator animator)
        {
            if (!animator.GetBool("show")) yield break;
            yield return new WaitForSeconds(inTrainingConfiguration.showNotificationDurationInSeconds);
            animator.SetBool("show", false);
            yield return new WaitForSeconds(1); // Can be removed when more stable
            notificationShown = false;
        }
    }
}
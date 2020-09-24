using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.Periphery.Clients;
using _Project.Scripts.Periphery.Configurations;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class Scene : MonoBehaviour
    {
        protected WebSocketClient webSocketClient;
        public TextAsset applicationConfigurationFile;
        protected ApplicationConfiguration applicationConfiguration;

        public void SetUpWebSocket()
        {
            Application.runInBackground = true;
            
            applicationConfiguration = new ApplicationConfigurationService(applicationConfigurationFile).configuration;
            webSocketClient = gameObject.AddComponent<WebSocketClient>();
            webSocketClient.webSocketConfiguration = applicationConfiguration.webSocket;
        }
    }
}
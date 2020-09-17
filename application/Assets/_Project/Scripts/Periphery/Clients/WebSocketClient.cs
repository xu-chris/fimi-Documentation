using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using JetBrains.Annotations;
using UnityEngine;
using WebSocketSharp;

namespace _Project.Scripts.Periphery.Clients
{
    public class WebSocketClient : MonoBehaviour
    {
        private bool enableLogging = false;
        public WebSocketConfiguration webSocketConfiguration;
        public Person[] detectedPersons;

        private bool isWsConnected;

        private string message;
        private WebSocket webSocket;
        private bool reconnecting = false;

        internal float lowestY = 999.0f;

        public void Start()
        {
            Application.runInBackground = true;
            StartCheckAndReconnectLifeCycle();
        }

        public void Update()
        {
            detectedPersons = DecodeMessageData(message);
        }

        public void OnApplicationQuit()
        {
            StopCoroutine(CheckAndReconnect());

            if (webSocket != null && webSocket.ReadyState == WebSocketState.OPEN)
                webSocket.Close();
        }

        public IEnumerable<Person> GetDecodedMessage()
        {
            return DecodeMessageData(message);
        }

        /**
 * Method to limit number of coroutines for reconnecting cycle to just one.
 */
        private void StartCheckAndReconnectLifeCycle()
        {
            if (!reconnecting)
            {
                StartCoroutine(CheckAndReconnect());
                reconnecting = true;
            }
        }

        private IEnumerator CheckAndReconnect()
        {
            while (!isWsConnected)
            {
                Debug.Log("Not connected to server. Will try to connect now.");
                yield return new WaitForSeconds(1);
                if (webSocket == null)
                {
                    ConnectToWebSocketServer();
                }
                else
                {
                    webSocket.Connect();
                }
            }
            reconnecting = false;
        }

        private void ConnectToWebSocketServer()
        {
            if (isWsConnected)
                return;

            using (webSocket = new WebSocket(webSocketConfiguration.url))
            {
                if (enableLogging)
                {
                    webSocket.Log.Level = LogLevel.TRACE;
                    webSocket.Log.File = "./ws_log.txt";
                }

                webSocket.OnOpen += (sender, e) =>
                {
                    webSocket.Send("Hello server.");
                    Debug.Log("Connection opened.");
                    isWsConnected = true;
                };
                webSocket.OnMessage += (sender, e) => { message = e.Data; };
                webSocket.OnClose += (sender, e) =>
                {
                    isWsConnected = false;
                    StartCheckAndReconnectLifeCycle();
                };
                webSocket.OnError += (sender, e) =>
                {
                    isWsConnected = false;
                    StartCheckAndReconnectLifeCycle();
                };

                webSocket.Connect();
            }
        }

        [CanBeNull]
        private Person[] DecodeMessageData(string message)
        {
            if (string.IsNullOrEmpty(message))
                return null;

            // Parse message
            const int parseOffset = 0; // No offset for real-time system
            var tokens = message.Split(',');

            var maxNumberOfJoints = 22;

            if ((tokens.Length - parseOffset) / 3 % maxNumberOfJoints != 0)
            {
                Debug.Log(
                    "Number of tokens cannot be parsed. Inconsistency between number of tokens and 3D vectors detected.");
                return null;
            }

            var currentNumPeople = (tokens.Length - parseOffset) / 3 / maxNumberOfJoints;
            Debug.Log("Detected " + (tokens.Length - parseOffset) / 3 + " joints. Number of detected people " + currentNumPeople);

            var newDetection = new Person[currentNumPeople];

            for (var p = 0; p < currentNumPeople; ++p)
            {
                newDetection[p].joints = new Vector3[maxNumberOfJoints];
                newDetection[p].id = p;
                for (var i = 0; i < maxNumberOfJoints - 1; ++i)
                {
                    var tokenIndex = 3 * p * maxNumberOfJoints + 3 * i + 3;

                    newDetection[p].joints[i].x =
                        float.Parse(tokens[tokenIndex + 0]) * 0.001f; // Can be flipped here for mirroring
                    newDetection[p].joints[i].y = float.Parse(tokens[tokenIndex + 1]) * 0.001f;
                    newDetection[p].joints[i].z = -float.Parse(tokens[tokenIndex + 2]) * 0.001f;

                    if (newDetection[p].joints[i].y < lowestY)
                        lowestY = newDetection[p].joints[i].y;
                }
            }

            return newDetection;
        }
    }
}
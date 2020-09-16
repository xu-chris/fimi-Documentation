using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using WebSocketSharp;

namespace _Project.Scripts.Periphery.Clients
{
    public class WebSocketClient : MonoBehaviour
    {
        private readonly bool enableLogging = false;
        private readonly string webSocketUrl = "ws://localhost:8080/";
        private Person[] _detectedPersons;

        private bool _isWsConnected;
        private float _lowestY = 999.0f;

        private string _message;
        private Vector3 _offset;
        private Vector3 _parentCamPos;

        private Quaternion _parentCamRot;
        private Vector3 _parentCamScale;

        private SkeletonOrchestrator _skeletonOrchestrator;
        private WebSocket _webSocket;
        private bool _reconnecting = false;

        public void Start()
        {
            Application.runInBackground = true;
    
            StartCheckAndReconnectLifeCycle();

            _skeletonOrchestrator = new SkeletonOrchestrator();

            _parentCamRot = transform.rotation;
            _parentCamPos = transform.position;
            _parentCamScale = transform.localScale;
        }

        public void Update()
        {
            _skeletonOrchestrator?.Update(DecodeMessageData(_message), _lowestY);
        }

        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.C))
            {
                Debug.Log("Before cam: " + transform.position);
                Debug.Log("Before parent cam: " + _parentCamPos);
                transform.position = _parentCamPos;
                transform.rotation = _parentCamRot;
                transform.localScale = _parentCamScale;
                Debug.Log("After: " + transform.position);

                Debug.Log("Reset camera to original position.");
            }
        }

        public void OnApplicationQuit()
        {
            StopCoroutine(CheckAndReconnect());

            if (_webSocket != null && _webSocket.ReadyState == WebSocketState.OPEN)
                _webSocket.Close();
        }

        /**
 * Method to limit number of coroutines for reconnecting cycle to just one.
 */
        private void StartCheckAndReconnectLifeCycle()
        {
            if (!_reconnecting)
            {
                StartCoroutine(CheckAndReconnect());
                _reconnecting = true;
            }
        }

        private IEnumerator CheckAndReconnect()
        {
            while (!_isWsConnected)
            {
                Debug.Log("Not connected to server. Will try to connect now.");
                yield return new WaitForSeconds(1);
                if (_webSocket == null)
                {
                    ConnectToWebSocketServer();
                }
                else
                {
                    _webSocket.Connect();
                }
            }
            _reconnecting = false;
        }

        private void ConnectToWebSocketServer()
        {
            if (_isWsConnected)
                return;

            using (_webSocket = new WebSocket(webSocketUrl))
            {
                if (enableLogging)
                {
                    _webSocket.Log.Level = LogLevel.TRACE;
                    _webSocket.Log.File = "./ws_log.txt";
                }

                _webSocket.OnOpen += (sender, e) =>
                {
                    _webSocket.Send("Hello server.");
                    Debug.Log("Connection opened.");
                    _isWsConnected = true;
                };
                _webSocket.OnMessage += (sender, e) => { _message = e.Data; };
                _webSocket.OnClose += (sender, e) =>
                {
                    _isWsConnected = false;
                    StartCheckAndReconnectLifeCycle();
                };
                _webSocket.OnError += (sender, e) =>
                {
                    _isWsConnected = false;
                    StartCheckAndReconnectLifeCycle();
                };

                _webSocket.Connect();
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
                newDetection[p].Joints = new Vector3[maxNumberOfJoints];
                newDetection[p].ID = p;
                for (var i = 0; i < maxNumberOfJoints - 1; ++i)
                {
                    var tokenIndex = 3 * p * maxNumberOfJoints + 3 * i + 3;

                    newDetection[p].Joints[i].x =
                        float.Parse(tokens[tokenIndex + 0]) * 0.001f; // Can be flipped here for mirroring
                    newDetection[p].Joints[i].y = float.Parse(tokens[tokenIndex + 1]) * 0.001f;
                    newDetection[p].Joints[i].z = -float.Parse(tokens[tokenIndex + 2]) * 0.001f;

                    if (newDetection[p].Joints[i].y < _lowestY)
                        _lowestY = newDetection[p].Joints[i].y;
                }
            }

            return newDetection;
        }
    }
}
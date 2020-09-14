using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    public string m_Message;
    private readonly bool enableLogging = false;
    private readonly string webSocketUrl = "ws://localhost:8080/";

    private RunLive m_AnyMethod;

    private bool m_isRotateCamera;
    private bool m_isWSConnected;
    private Vector3 m_Offset;
    private Vector3 m_ParentCamPos;

    private Quaternion m_ParentCamRot;
    private Vector3 m_ParentCamScale;
    private WebSocket m_ws;

    public void Start()
    {
        Application.runInBackground = true;

        ConnectToWebSocketServer();

        m_AnyMethod = new RunLiveXNect();

        m_ParentCamRot = transform.rotation;
        m_ParentCamPos = transform.position;
        m_ParentCamScale = transform.localScale;

        m_AnyMethod.Start();
    }

    public void Update()
    {
        if (m_isRotateCamera && m_AnyMethod.m_JointSpheres.GetLength(0) > 0)
        {
            m_Offset = transform.position - m_AnyMethod.m_JointSpheres[0, 0].transform.position;
            m_Offset = Quaternion.AngleAxis(100.0f * Time.deltaTime, Vector3.up) * m_Offset;

            transform.position = m_AnyMethod.m_JointSpheres[0, 0].transform.position + m_Offset;
            transform.LookAt(m_AnyMethod.m_JointSpheres[0, 0].transform.position);

            m_isRotateCamera = false;
        }

        m_AnyMethod?.Update(m_Message);
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            m_isRotateCamera = !m_isRotateCamera;
            Debug.Log(m_isRotateCamera ? "Rotating camera." : "Stopping camera rotation.");
        }

        if (!Input.GetKey(KeyCode.C)) return;

        Debug.Log("Before cam: " + transform.position);
        Debug.Log("Before parent cam: " + m_ParentCamPos);
        transform.position = m_ParentCamPos;
        transform.rotation = m_ParentCamRot;
        transform.localScale = m_ParentCamScale;
        Debug.Log("After: " + transform.position);

        Debug.Log("Reset camera to original position.");
    }

    public void OnApplicationQuit()
    {
        if (m_ws != null && m_ws.ReadyState == WebSocketState.OPEN)
            m_ws.Close();
    }

    private void ConnectToWebSocketServer()
    {
        if (m_isWSConnected)
            return;

        m_Message = "No data.";

        using (m_ws = new WebSocket(webSocketUrl))
        {
            if (enableLogging)
            {
                m_ws.Log.Level = LogLevel.TRACE;
                m_ws.Log.File = "./ws_log.txt";
            }

            m_ws.OnOpen += (sender, e) =>
            {
                m_ws.Send("Hello server.");
                Debug.Log("Connection opened.");
                m_isWSConnected = true;
            };
            m_ws.OnMessage += (sender, e) =>
            {
                m_Message = e.Data;
                Debug.Log(m_Message);
            };
            m_ws.OnClose += (sender, e) =>
            {
                m_ws.Connect(); // Reopen connection
                m_isWSConnected = false;
            };
            m_ws.OnError += (sender, e) => { m_isWSConnected = false; };

            m_ws.Connect();
        }
    }
}
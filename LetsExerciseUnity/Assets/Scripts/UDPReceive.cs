using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using UnityEngine.SceneManagement;


public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;

    public ButtonEvent buttonEvent;

    public GameObject mouse;

    private float[] transformPosition = new float[3];

    public RectTransform canvasRectTransform;

    float canva_xMin;
    float canva_xMax;
    float canva_yMin;
    float canva_yMax;
    string[] parts = { "00", "00" };

    Scene m_Scene;
    Scene f_Scene;

    public void Start()
    {

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();


        transformPosition[0] = mouse.transform.position.x;
        transformPosition[1] = mouse.transform.position.y;
        transformPosition[2] = mouse.transform.position.z;

        canva_xMin = canvasRectTransform.rect.xMin;
        canva_xMax = canvasRectTransform.rect.xMax;
        canva_yMin = canvasRectTransform.rect.yMin;
        canva_yMax = canvasRectTransform.rect.yMax;

        m_Scene = SceneManager.GetActiveScene();
        f_Scene = SceneManager.GetActiveScene();
    }


    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                parts = data.Trim('[', ']').Split(',');

                float normalizedValue1 = normalize(float.Parse(parts[0]), 0, 1280, canva_xMin, canva_xMax)+430;
                float normalizedValue2 = canva_yMax - normalize(float.Parse(parts[1]), 0, 1000, canva_yMin, canva_yMax) - 50;
                //Debug.Log(normalizedValue1);
                //Debug.Log(normalizedValue2);

      
                transformPosition[0] = normalizedValue1;
                transformPosition[1] = normalizedValue2;

                //Debug.Log(worldPos);

                if (printToConsole) { print(data); }
               


            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    void Update()
    {
        m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.buildIndex != f_Scene.buildIndex)
        {

            mouse = GameObject.Find("Mouse");
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            transformPosition[0] = mouse.transform.position.x;
            transformPosition[1] = mouse.transform.position.y;
            transformPosition[2] = mouse.transform.position.z;

            canva_xMin = canvasRectTransform.rect.xMin;
            canva_xMax = canvasRectTransform.rect.xMax;
            canva_yMin = canvasRectTransform.rect.yMin;
            canva_yMax = canvasRectTransform.rect.yMax;
            buttonEvent = GetComponent<ButtonEvent>();
        }
        f_Scene = SceneManager.GetActiveScene();

        mouse.transform.position = new Vector3(transformPosition[0], transformPosition[1], transformPosition[2]);

        // 是否點擊
        if (parts.Length == 3 && buttonEvent.canClickButton == true)
        {
            buttonEvent.Check_if_button();
        }

    }

    float normalize(float value, float minFrom, float maxFrom, float minTo, float maxTo)
    {
        return (value - minFrom) / (maxFrom - minFrom) * (maxTo - minTo) + minTo;
    }


}
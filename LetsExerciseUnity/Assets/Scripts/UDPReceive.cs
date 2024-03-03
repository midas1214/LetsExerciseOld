using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using UnityEngine.SceneManagement;


public class UDPReceive : MonoBehaviour
{

    Thread receiveHandThread, receiveAngleThread;
    UdpClient clientHand, clientAngle;
    public int portHand = 5052, portAngle = 5051;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string dataHand, dataAngle;

    public ButtonEvent buttonEvent;

    public GameObject mouse;

    private float[] transformPosition = new float[3];

    public RectTransform canvasRectTransform;

    float canva_xMin;
    float canva_xMax;
    float canva_yMin;
    float canva_yMax;
    float canva_width;
    float canva_height;
    string[] parts = { "00", "00" };

    Scene m_Scene;
    Scene f_Scene;

    public void Start()
    {
        receiveHandThread = new Thread(
            new ThreadStart(ReceiveHandData));
        receiveHandThread.IsBackground = true;
        receiveHandThread.Start();

        receiveAngleThread = new Thread(
            new ThreadStart(ReceiveAngleData));
        receiveAngleThread.IsBackground = true;
        receiveAngleThread.Start();


        transformPosition[0] = mouse.transform.localPosition.x;
        transformPosition[1] = mouse.transform.localPosition.y;
        transformPosition[2] = mouse.transform.localPosition.z;

        canva_xMin = canvasRectTransform.rect.xMin;
        Debug.Log(canva_xMin);
        canva_xMax = canvasRectTransform.rect.xMax;
        Debug.Log(canva_xMax);
        canva_yMin = canvasRectTransform.rect.yMin;
        canva_yMax = canvasRectTransform.rect.yMax;
        canva_width = canvasRectTransform.rect.width;
        canva_height = canvasRectTransform.rect.height;

        m_Scene = SceneManager.GetActiveScene();
        f_Scene = SceneManager.GetActiveScene();
    }


    // receive hand thread
    private void ReceiveHandData()
    {
        clientHand = new UdpClient(portHand);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = clientHand.Receive(ref anyIP);
                dataHand = Encoding.UTF8.GetString(dataByte);

                parts = dataHand.Trim('[', ']').Split(',');

                float normalizedValue1 = normalize(float.Parse(parts[0]), 0, 640, canva_xMin, canva_xMax) + (canva_width / 2);
                float normalizedValue2 = canva_yMax - normalize(float.Parse(parts[1]), 0, 480, canva_yMin, canva_yMax);
                string s = normalizedValue1.ToString() + "," + normalizedValue2.ToString();
                Debug.Log(s);

      
                transformPosition[0] = normalizedValue1;
                transformPosition[1] = normalizedValue2;

                //Debug.Log(worldPos);

                if (printToConsole) { print(dataHand); }
               


            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // receive thread
    private void ReceiveAngleData()
    {
        clientAngle = new UdpClient(portAngle);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 1);
                byte[] dataByte = clientAngle.Receive(ref anyIP);
                dataAngle = Encoding.UTF8.GetString(dataByte);
                // Debug.Log(dataAngle);

                //parts = dataHand.Trim('[', ']').Split(',');

                //float normalizedValue1 = normalize(float.Parse(parts[0]), 0, 1280, canva_xMin, canva_xMax) + 430;
                //float normalizedValue2 = canva_yMax - normalize(float.Parse(parts[1]), 0, 1000, canva_yMin, canva_yMax) - 50;
                ////Debug.Log(normalizedValue1);
                ////Debug.Log(normalizedValue2);


                //transformPosition[0] = normalizedValue1;
                //transformPosition[1] = normalizedValue2;

                ////Debug.Log(worldPos);

                //if (printToConsole) { print(dataHand); }



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
            transformPosition[0] = mouse.transform.localPosition.x;
            transformPosition[1] = mouse.transform.localPosition.y;
            transformPosition[2] = mouse.transform.localPosition.z;

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
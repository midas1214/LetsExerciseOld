using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using System.Collections;


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
    string[] parts = {"00","00"};

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

                float normalizedValue1 = normalize(float.Parse(parts[0]), 0, 1280, canva_xMin, canva_xMax)+canva_xMax;
                float normalizedValue2 = canva_yMax - normalize(float.Parse(parts[1]), 0, 1000, canva_yMin,canva_yMax) - 10;

                transformPosition[0] = normalizedValue1;
                transformPosition[1] = normalizedValue2;

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

        mouse.transform.position = new Vector3(transformPosition[0], transformPosition[1], transformPosition[2]);

        if (parts.Length == 3 && buttonEvent.canClickButton == true)
        {
            buttonEvent.Check_if_button();
        }
    }

    float normalize (float value, float minFrom, float maxFrom, float minTo, float maxTo)
    {
        return (value - minFrom) / (maxFrom - minFrom) * (maxTo - minTo) + minTo;
    }

    


}

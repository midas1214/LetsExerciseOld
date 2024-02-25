using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageReceive : MonoBehaviour
{
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    int port;

    public RawImage img;
    byte[] imageDatas = new byte[0];
    Texture2D tex;

    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        InitTcp();
        tex = new Texture2D(1280, 720);
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitTcp()
    {
        port = 5066;
        print("TCP init");
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        listener = new TcpListener(anyIP);
        listener.Start();
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void OnDestroy()
    {
        receiveThread.Abort();
    }

    private void ReceiveData()
    {
        print("received somthing..."); //回應tcp連線
        try
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                NetworkStream stream = new NetworkStream(client.Client);
                StreamReader sr = new StreamReader(stream);
                string jsonData = sr.ReadLine();
             
                //讀成圖片
                Data _imgData = JsonUtility.FromJson<Data>(jsonData);
                imageDatas = _imgData.image;
            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    public class Data
    {
        public byte[] image;
    }

    private void FixedUpdate()
    {
        if (scene.name == "SampleScene")
        {
            tex.LoadImage(imageDatas);
            img.texture = tex;

        }
        
    }

}

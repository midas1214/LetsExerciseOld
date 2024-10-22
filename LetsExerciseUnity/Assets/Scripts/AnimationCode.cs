using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class AnimationCode : MonoBehaviour
{
    public GameObject[] Body;
    List<string> lines;
    int counter = 0;

    public GameObject avatar;

    // Start is called before the first frame update
    void Start()
    {
        lines = System.IO.File.ReadLines("Assets/PoseDataset/lmList.txt").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        string[] points = lines[counter].Split(',');

        for (int i = 0; i <= 32; i++)
        {
            // normoalize position
            //float x = float.Parse(points[0 + (i * 3)]) /20;
            //float y = float.Parse(points[1 + (i * 3)]) /20;
            //float z = float.Parse(points[2 + (i * 3)]) /20;
            float x = float.Parse(points[0 + (i * 3)]) / 20;
            float y = float.Parse(points[1 + (i * 3)]) / 20;
            float z = float.Parse(points[2 + (i * 3)]) / 80;

            Body[i].transform.localPosition = new Vector3(x, y, z);
        }
        counter += 1;
        if (counter == lines.Count)
        {
            counter = 0;
        }
        Thread.Sleep(30);
    }
}

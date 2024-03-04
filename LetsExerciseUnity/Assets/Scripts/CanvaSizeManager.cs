using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvaSizeManager : MonoBehaviour
{
    GameObject currentCanva;
    float screen_width, screen_height;

    // Start is called before the first frame update
    void Start()
    {
        screen_width = Screen.currentResolution.width;
        screen_height = Screen.currentResolution.height;
    }

    // Update is called once per frame
    void Update()
    {
        currentCanva = GameObject.Find("Canvas");
        if (currentCanva.GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution != new Vector2(screen_width, screen_height))
        {
            currentCanva.GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution = new Vector2(screen_width, screen_height);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mouse;
    public Button[] buttons;

    public bool canClickButton = true;

    void Start()
    {
        //button1.onClick.AddListener(Button1Click);

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void Button1Click()
    {
        Debug.Log("Button 1 Clicked!");
        // Add your custom logic for Button 1 click
    }

    public void Check_if_button()
    {
        int yes = 0;
        foreach (Button btn in buttons)
        {
            if (Check_touch_button(btn))
            {
                yes = 1;
                Debug.Log("RawImage is touching Button: " + btn.name);
                canClickButton = false;
                StartCoroutine(DelayedAction());

                return;
            }
        }
        if (yes == 0)
        {
            Debug.Log("Your are not clicking a button");
        }

    }


    bool Check_touch_button(Button btn)
    {
        float[] button_info = Get_button_info(btn);
        if (mouse.transform.position.x > button_info[0] - button_info[2]/2 && mouse.transform.position.x < button_info[0] +button_info[2] / 2 && mouse.transform.position.y > button_info[1] - button_info[3] / 2 && mouse.transform.position.y < button_info[1] + button_info[3] / 2)
        {
            return true;
        }
        return false;
    }

    float[] Get_button_info(Button btn)
    {
        RectTransform buttonRectTransform = btn.GetComponent<RectTransform>();
        float[] button_info = { btn.transform.position.x, btn.transform.position.y , buttonRectTransform.sizeDelta.x , buttonRectTransform.sizeDelta.y };
        return button_info;
    }

    private IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(1);

        canClickButton = true;
    }
}

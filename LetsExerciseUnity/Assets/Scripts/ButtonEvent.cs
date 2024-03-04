using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mouse;
    public Button[] buttons;

    public int forInvestigate = 1;

    public bool canClickButton = true;
    Scene m_Scene;
    Scene f_Scene;

    int numOfButton;

    void Start()
    {
        m_Scene = SceneManager.GetActiveScene();
        f_Scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.buildIndex != f_Scene.buildIndex)
        {
            SetButtonList();
        }
        f_Scene = SceneManager.GetActiveScene();
    }

    void SetButtonList()
    {
        if (m_Scene.name == "GameStart")
        {
            numOfButton = 1;
        }

        else if (m_Scene.name == "Trainer")
        {
            numOfButton = 1;
        }
        else if (m_Scene.name == "Intro")
        {
            numOfButton = 0;
        }
        else if (m_Scene.name == "SelectSex")
        {
            numOfButton = 3;
        }
        else if (m_Scene.name == "SelectPart")
        {
            numOfButton = 6;
        }
        else if (m_Scene.name == "Investigation")
        {
            numOfButton = 0;
        }

        mouse = GameObject.Find("Mouse");

        buttons = new Button[numOfButton];
        for (var i = 1; i <= numOfButton; i++)
        {
            Button btn = GameObject.Find("Btn" + i).GetComponent<Button>();

            buttons[i-1] = btn; 
        }



    }

    private void ButtonClick(Button btn)
    {
        if ( m_Scene.name == "GameStart")
        {
            if (btn.name == "Btn1")
            {
                SceneManager.LoadScene(1);
            }

        }
        
        else if (m_Scene.name == "Trainer")
        {
            if (btn.name == "Btn1")
            {
                SceneManager.LoadScene(3);
            }

        }
        else if (m_Scene.name == "SelectSex")
        {
            if(btn.name == "Btn1")
            {

            }
            else if (btn.name == "Btn2")
            {

            }
            else if (btn.name == "Btn3")
            {
                SceneManager.LoadScene(4);
            }

        }
        else if (m_Scene.name == "SelectPart")
        {
            if (btn.name == "Btn1")
            {
                forInvestigate = 1;
            }
            else if (btn.name == "Btn2")
            {
                forInvestigate = 2;
            }
            else if (btn.name == "Btn3")
            {
                forInvestigate = 3;
            }
            else if (btn.name == "Btn4")
            {
                forInvestigate = 3;
            }
            else if (btn.name == "Btn5")
            {
                forInvestigate = 4;
            }
            else if (btn.name == "Btn6")
            {
                SceneManager.LoadScene(5);
            }

        }


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
                ButtonClick(btn);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CountDownTimer : MonoBehaviour
{
    float currentTime = -1f;
    [SerializeField] float startingTime = 5f;
    [SerializeField] TextMeshProUGUI count;

    [SerializeField] AnimationCode animationCode;
    // Start is called before the first frame update
    void Start()
    {
        count.text = "";
        
    }

    public void StartCountDown()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            count.text = Mathf.RoundToInt(currentTime).ToString();
            Debug.Log(currentTime);
        }
        else if (currentTime == 0)
        {
            count.text = ""; // Or any message you want when the countdown reaches zero
            //animationCode.StartAnimate();
            currentTime = -1;
        }
    }
}

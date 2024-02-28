using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateSet : MonoBehaviour
{
    [SerializeField] ButtonEvent buttonEvent;
    public TextMeshProUGUI bm;
    public TextMeshProUGUI des;
    public TextMeshProUGUI c1;
    public TextMeshProUGUI c2;
    public TextMeshProUGUI c3;

    // Start is called before the first frame update
    void Start()
    {
        buttonEvent = GameObject.Find("WholeManager").GetComponent<ButtonEvent>();
        buttonEvent.forInvestigate = 2;

        if (buttonEvent.forInvestigate == 1)
        {
            bm.text = "push-up";
            des.text = "How many moves can you do in one minute?";

        }
        else if (buttonEvent.forInvestigate == 2)
        {
            bm.text = "plank";
            des.text = "How many seconds can you hold on before you feel tired?";

        }
        else if (buttonEvent.forInvestigate == 3)
        {
            bm.text = "squat";
            des.text = "How many moves can you do in one minute?";

        }
        else if (buttonEvent.forInvestigate == 4)
        {
            bm.text = "jumping jacks";
            des.text = "How many moves can you do in one minute?";

        }
        if (buttonEvent.forInvestigate == 1 || buttonEvent.forInvestigate == 3 || buttonEvent.forInvestigate == 4)
        {
            c1.text = "less than 20 times";
            c2.text = "20 times to 40 times";
            c3.text = "more than 40 times";
        }
        else
        {
            c1.text = "less than 30 seconds";
            c2.text = "30 times to 60 seconds";
            c3.text = "more than 60 seconds";

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

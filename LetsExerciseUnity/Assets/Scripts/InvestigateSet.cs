using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvestigateSet : MonoBehaviour
{
    [SerializeField] ButtonEvent buttonEvent;
    public TextMeshPro bm;
    public TextMeshPro des;

    // Start is called before the first frame update
    void Start()
    {
        buttonEvent = GameObject.Find("WholeManager").GetComponent<ButtonEvent>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

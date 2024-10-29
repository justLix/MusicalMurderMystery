using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//tracks the amount of found opportunities as part of the hand menu during case 3
public class Tracker : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.text.text = "opportunities found:\n"+Opportunity.found+"/5";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//tool that shows additional context (name and description of playing instruments) synched to a dialogue
public class Contexter : MonoBehaviour
{
    [SerializeField] public string[] texts;
    [SerializeField] TMP_Text display;
    [SerializeField] GameObject onButton;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.display.text = texts[i];
        this.display.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        i++;
        display.text = texts[i];
    }
    public void Prev()
    {
        i--;
        display.text = texts[i];
    }
    public void Show()
    {
        this.display.gameObject.SetActive(true);
        this.onButton.SetActive(false);
    }
}

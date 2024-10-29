using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//help tool that is part of the hand menu
public class TaskDescriber : MonoBehaviour
{
    public GameObject taskbox;
    public GameObject onButton;

    // Start is called before the first frame update
    void Start()
    {
        this.taskbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        this.taskbox.SetActive(true);
        this.onButton.SetActive(false);
    }
    public void hide()
    {
        this.taskbox.SetActive(false);
        this.onButton.SetActive(true);
    }

}

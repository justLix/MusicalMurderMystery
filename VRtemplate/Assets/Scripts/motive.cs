using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used during search challenges to reveal possible murder motives
public class motive : MonoBehaviour
{
    //interactable button for the player to press if they think the motive is correct
    [SerializeField] GameObject button;
    //player camera to align the button to
    [SerializeField] GameObject cam;
    //audiosource to play 'wrong' noice if activated incorrectly
    [SerializeField] AudioSource buzzer;
    //properties for alignment of button to camera
    [SerializeField] Transform root;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        if(this.root)
            this.pos = this.root.position;
        else
            this.pos = this.transform.position;
    }

    //align button to camera if close enough, oterwise hide it
    void Update()
    {
        if (Vector3.Distance(cam.transform.position, this.pos) < 2)
        {
            this.button.SetActive(true);
            this.button.transform.LookAt(cam.transform);
        }
        else
        {
            this.button.SetActive(false);
        }
            
    }

    public void Wrong()
    {
        LevelBehavior.score -= 50;
        this.button.SetActive(false);
        this.buzzer.Play();
    }
    public void Right()
    {
        LevelBehavior.score += 50;
    }
}

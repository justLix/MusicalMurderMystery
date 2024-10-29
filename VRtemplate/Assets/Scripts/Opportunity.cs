using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//used during case three to display possible opportunities for the victim to have been killed
public class Opportunity : MonoBehaviour
{
    //visualization of the possible course of actions
    [SerializeField] GameObject happening;
    //is this the correct solution?
    [SerializeField] bool correct = false;
    //properties for aligning the button to the camera
    [SerializeField] Transform cam;
    [SerializeField] GameObject button;
    [SerializeField] GameObject pos;
    bool hidden = true;
    //associated audiosource
    Vector3 audiopos;
    AudioSource audiosource;

    //static variable to ensure only one course of action is to be displayed at a time
    static Opportunity active;
    //statistics to track amount of opportunities the player has found
    public static int found = 0;
    int value = 50;

    // Start is called before the first frame update
    void Start()
    {
        this.happening.SetActive(false);
        this.button.SetActive(false);
        if (this.GetComponentInParent<Location>())
        {
            this.audiosource = this.transform.parent.GetComponentInChildren<AudioSource>();
            this.audiopos = this.audiosource.transform.position;
        }
        else
        {
            if (this.transform.parent.GetComponentInChildren<AudioSource>())
            {
                this.audiosource = this.transform.parent.GetComponentInChildren<AudioSource>();
                this.audiopos = this.audiosource.transform.position;
                this.audiosource.loop = true;
                this.audiosource.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //handle behaviour of button
        if (this.hidden)
        {
            if (Vector3.Distance(this.button.transform.position, cam.position) < 2)
                this.button.SetActive(true);
            else
                this.button.SetActive(false);
        }
        if(active != this)
        {
            this.hidden = true;
            this.audiosource.transform.position = this.audiopos;
            this.happening.SetActive(false);
        }
    }

    //activated by the player pressing the button. reveals the happening and updates the count
    public void Reveal()
    {
        this.happening.SetActive(true);
        this.audiosource.gameObject.transform.position = this.pos.transform.position;
        LevelBehavior.score += this.value;
        if(this.value > 0)
        {
            this.value = 0;
            found++;
        }
        this.button.SetActive(false);
        this.hidden = false;
        active = this;
    }

    //activated by the player selecting this opportunity as their guess
    public void Test()
    {
        if(correct)
            SceneManager.LoadSceneAsync(25);
        else
            SceneManager.LoadSceneAsync(26);
    }

}

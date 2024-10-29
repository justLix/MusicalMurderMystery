using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//used during briefings to show dialogues and associated items and to play music
public class Dialogue : MonoBehaviour
{
    //the individual lines of the dialogue
    [SerializeField] public string[] texts;
    //the associated items for each line of dialogue. if no item is associated, use dummy empty item instead
    [SerializeField] public GameObject[] item;
    //the associated piece of music for each line of dialogue. if no song is to be played, leave empty
    [SerializeField] public AudioClip[] songs;
    //synchronized context tool displaying instrument names and descriptions
    [SerializeField] public Contexter cont;
    //buttons to show next or previous line
    [SerializeField] public GameObject Button;
    [SerializeField] public GameObject backButton;
    //gameobjects that are only to be activated once the last line of dialogue has been displayed
    [SerializeField] public GameObject[] Dependants = { };
    //tracks the currently active line
    public int i = 0;
    //scene to be loaded upon completion of the dialogue. if no scene is to be loaded, set to negative number 
    public int nextScene = -1;
    //used to log the time spent listening to individual pieces of music
    private float logger = 0;

    //outputs
    protected TMP_Text display;
    AudioSource radio;
    


    // Start is called before the first frame update
    //load initial state
    protected void Start()
    {
        foreach(GameObject o in this.item)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in this.Dependants)
        {
            o.SetActive(false);
        }
        this.display = this.gameObject.GetComponentInChildren<TMP_Text>();
        this.radio = this.gameObject.GetComponentInChildren<AudioSource>();
        this.display.text = texts[i];
        this.radio.clip = songs[i];
        this.backButton.SetActive(false);
        if(this.radio.clip != null )
            this.radio.Play();
        this.item[i].SetActive(true);
    }

    // Update is called once per frame
    //rotate displayed items
    protected void Update()
    {
        foreach (GameObject it in item)
        {
            it.transform.Rotate(new Vector3(0, 1f, 0));
        }
    }
    //load next line
    public virtual void Next()
    {
        //log time spent listening
        if (this.radio.clip)
            LevelBehavior.logdata += "listened to " + this.radio.clip.name + " for " + (Time.timeSinceLevelLoad - this.logger) + "\n";
        this.logger = Time.timeSinceLevelLoad;
        //check whether the end of the dialogue is reached
        if (i >= texts.Length-1)
        {
            Last();
            End(nextScene);
            return;
        }
        //hide the previous item
        item[i].SetActive(false);
        i++;
        //ensure that the player can revisit the previous line
        this.backButton.SetActive(true);
        //keep the contexter synchronized
        if(this.cont)
            cont.Next();
        //show the new text, song and item
        this.radio.clip = songs[i];
        if (this.radio.clip != null)
            this.radio.Play();
        this.item[i].SetActive(true);
        this.display.text = texts[i];
    }
    //load previous line
    public virtual void Prev()
    {
        //log time spent listening
        this.logger = Time.timeSinceLevelLoad;
        //hide last item
        item[i].SetActive(false);
        i--;
        //ensure that the player can't go back further than the beginning
        if (i == 0)
            this.backButton.SetActive(false);
        //keep the contexter synchronized
        if (this.cont)
            cont.Prev();
        //ensure that the player can continue
        if (this.Button)
            this.Button.SetActive(true);
        //load the new text, song and item
        this.radio.clip = songs[i];
        if (this.radio.clip != null)
            this.radio.Play();
        this.item[i].SetActive(true);
        this.display.text = texts[i];
    }

    //called after the last line of dialogue was displyed
    protected void Last()
    {
        //hide the continue button
        if(this.Button)
            this.Button.SetActive(false);
        //show gameobjects that are revealed after the dialogue
        if (this.Dependants.Length>0)
        {
            foreach(GameObject it in this.Dependants)
                it.SetActive(true);
        }
    }

    //called when the end of the dialogue is reached, may be called externally to switch levels
    public static void End(int number)
    {
        //load no new scene 
        if (number < 0)
            return;
        //load the 'player has solved the case' scene
        if (number == 7)
            LevelBehavior.score += 100;
        //quit the game using placeholder code 42
        if(number==42)
            Application.Quit();
        SceneManager.LoadSceneAsync(number);
    }

    public void Skip()
    {
        Last();
        End(nextScene);
    }

}

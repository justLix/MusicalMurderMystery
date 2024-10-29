using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//locations in which an item can be placed during challenges
public class Location : MonoBehaviour
{
    //id to match against item id
    public string id = "";
    //audio source to play the associated theme
    public AudioSource audioSource;
    //associated theme
    [SerializeField] public AudioClip song;

    //if the corresponding item has already been found and the scene is reloaded, restore the appropriate state
    public void Pop()
    {
        if (!LevelBehavior.pieces.ContainsKey(this.id))
            return;
        this.Found();
        Destroy(this);
    }

    //create initial state
    void Start()
    {
        this.audioSource = this.gameObject.GetComponentInChildren<AudioSource>();
        this.audioSource.clip = song;
        this.audioSource.loop = true;
        this.audioSource.Play();
        //check whether the item was already found
        this.Pop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //tune down volume to help the player focus on other themes
    public void Found()
    {
        this.audioSource.volume *= 0.3f;
    }
}

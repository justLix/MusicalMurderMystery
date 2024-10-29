using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//manages the chase sequences, controls the movement of the chase target
public class Chase : MonoBehaviour
{
    //the target of the chase
    public GameObject target;
    //the player
    public GameObject chaser;
    //all corners of the path
    public List<Transform> stops = new List<Transform>();
    //amount of corners that have been passed by the player
    public static int passed = 0;
    //amount of corners that have been passed by the target
    public int found = 0;
    //status of the target to facilitate rubberbanding
    public bool waiting = false;
    public float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.GetComponent<ChaseTarget>())
            {
                this.target = child.gameObject;
                return;
            }
            
        }
        passed = 0;
        found = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //check whether the target has completed the path
        if (this.found >= this.stops.Count-1)
            return;
        //check whether the player has caught up with the target, speed it up if this is the case
        if(passed>this.found)
            this.target.transform.position += (this.stops[found+1].position - this.target.transform.position).normalized * this.speed * 2 * Time.deltaTime;
        //check whether the player is close to the target, speed it up if this is the case to avoid confusion
        if(Vector3.Distance(this.target.transform.position,this.chaser.transform.position)<2)
            this.target.transform.position += (this.stops[found + 1].position - this.target.transform.position).normalized * this.speed * 3 * Time.deltaTime;
        //check whether the target needs to wait for the player any longer
        if (this.waiting)
            this.target.transform.position += (this.stops[found + 1].position - this.target.transform.position).normalized * (this.speed / 50.0f) * Time.deltaTime;
        else
            this.target.transform.position += (this.stops[found + 1].position - this.target.transform.position).normalized * this.speed * Time.deltaTime;
    }

    //log that the player has passed another corner
    public void Pass()
    {
        passed++;
        this.waiting = false;
    }

    //log that the target has arrived at another corner
    public void Find()
    {
        this.found++;
        this.waiting = true;
    }

}

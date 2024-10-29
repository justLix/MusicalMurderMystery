using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//main component of the hand menu
//usually contains item for the player to grab and place
public class Inventory : MonoBehaviour
{
    //grabbable items
    public List<GameObject> objects = new List<GameObject>();
    //all locations in the scene that accept one of the items
    public GameObject locations;

    //components for help functionality
    public LineRenderer line;
    private int helping = 0;
    private int showing = 50;
    [SerializeField] GameObject instNames;
    [SerializeField] GameObject tooltip;
    
    // Start is called before the first frame update
    void Start()
    {
        //collect all items, register them as part of the inventory
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.GetComponent<PuzzlePiece>())
            {
                child.gameObject.GetComponent<PuzzlePiece>().inventory = this;
                objects.Add(child.gameObject);
            }
        }
        //check whether there is a saved state for objects
        //(from playing part of the scene, going back to the briefing, and returning to the scene)
        foreach (GameObject piece in this.objects)
        {
            piece.GetComponent<PuzzlePiece>().Pop();
        }
        //hide help tools
        if(this.line)
            this.line.gameObject.SetActive(false);
        if(this.instNames)
            this.instNames.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //handle helper
        if (this.helping < 0)
        {
            if(this.line)
                this.line.gameObject.SetActive(false);
            if(this.instNames)
                this.instNames.SetActive(false);
        }
        else
        {
                this.helping--;
        }
        if(this.showing <= 0)
        {
            if(this.tooltip)
                this.tooltip.gameObject.SetActive(false);
        }
        else
        {
            this.showing--;
        }
    }

    //help function invoked by player clicking the Help button
    public void Help()
    {
        //find an object to help with
        foreach (GameObject child in this.objects)
        {
            //rigidbody component is destroyed if the item is placed, therefore items with rigidbodies may be helped with
            if (child.gameObject.GetComponent<Rigidbody>())
            {
                //find the corresponding target location
                foreach (Transform loc in locations.transform)
                {
                    if (loc.gameObject.GetComponent<Location>().id == child.GetComponent<PuzzlePiece>().id)
                    {
                        //log request for help
                        LevelBehavior.helped++;
                        LevelBehavior.logdata += "Helped finding " + child.GetComponent<PuzzlePiece>().id + ".\n";
                        //show help tools
                        this.line.gameObject.SetActive(true);
                        this.line.SetPositions(new Vector3 []{ child.gameObject.transform.position, child.gameObject.transform.position+0.3f*(loc.position-child.gameObject.transform.position).normalized});
                        this.instNames.SetActive(true);
                        this.helping = 180;
                        return;
                    }
                }
            }
        }
    }

    public void End(int number)
    {
        if(number == 6)
        {
            if(LevelBehavior.score >= 390)
            {
                SceneManager.LoadSceneAsync(10);
                return;
            }else if(LevelBehavior.score >= 290)
            {
                SceneManager.LoadSceneAsync(9);
                return;
            }
                
        }
        SceneManager.LoadSceneAsync(number);
    }
}
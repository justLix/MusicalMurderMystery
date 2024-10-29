using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to help players during chase sequences.
//draws an arrow pointing towards the chased target.
public class ChaseCompass : MonoBehaviour
{
    public GameObject target;
    public LineRenderer line;
    private int duration = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.line.gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        //remove the arrow after showing it
        if(this.duration <= 0)
        {
            this.line.gameObject.SetActive(false);
            return;
        }
        this.duration--;
    }

    //invoked by pressing the help button of the hand menu
    public void Help()
    {
        //log the request for help
        LevelBehavior.helped++;
        LevelBehavior.logdata += "Helped with stop " + (Chase.passed + 1) + "\n";
        //draw the arrow
        this.line.SetPositions(new Vector3[] {this.gameObject.transform.position, this.gameObject.transform.position + 0.3f * (this.target.gameObject.transform.position - this.gameObject.transform.position).normalized});
        this.duration = 50;
        this.line.gameObject.SetActive(true);
    }

}

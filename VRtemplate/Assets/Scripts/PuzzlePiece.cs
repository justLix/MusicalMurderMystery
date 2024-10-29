using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//item for the player to place 
public class PuzzlePiece : MonoBehaviour
{
    //id to match against id of location
    [SerializeField] public string id = "";
    //inventory to which the item belongs
    public Inventory inventory;
    //variables to handle size transformations from inventory to grabbed
    public Vector3 initialscale;
    public Quaternion initialrotation;
    public Vector3 startpos;
    [SerializeField] public Vector3 intermediateScale;
    //items may only interact with locations if they are placed, i.e. let go of by the player
    //defaults to true and only is set to negative if taken out of the inventory and placed
    public bool grabbed = true;
    //frames since the item has been let go of for the sake of cleanup
    public int lost = 300;
    //rigidbody for the item, important as it behaves differently in different states
    Rigidbody r;
    //items with particle system need to be treated differently
    [SerializeField] public GameObject blood = null;
    
    //variables for statistics
    private int logger = 0;
    public static double timeAcc = 0.0f;
    public static double lastTime = 0.0f;
    public static double distAcc = 0.0f;
    public static int tries = 0;
    
    //details about a placed item that need to be saved so they can be reloaded if the level is reloaded
    public struct SaveState
    {
        public Vector3 localPosition;
        public Vector3 localScale;
        public Quaternion localRotation;
        public int lvl;
    }

    //adds the item to the list of stored items after it has been placed so it can be reloaded
    public void Store()
    {
        //no need to store it if it has been stored before, no details change
        if (LevelBehavior.pieces.ContainsKey(this.id))
            return;
        SaveState state;
        state.localPosition = this.transform.localPosition;
        state.localRotation = this.transform.localRotation;
        state.localScale = this.transform.localScale;
        //stores current level id so any falsely saved items can never appear in the wrong level
        state.lvl = LevelBehavior.lvl;
        LevelBehavior.pieces.Add(this.id, state);
    }

    //tries to restore the item after a level reload
    public void Pop()
    {
        //only attempt restore if it has been stored previously
        if (LevelBehavior.pieces.ContainsKey(this.id))
        {
            SaveState s;
            LevelBehavior.pieces.TryGetValue(this.id, out s);
            //if somehow a stored item from another level is still present, don't restore it 
            //should never be the case, unless unity's level loading system messes up and doesn't correctly invoke methods from LevelBehavior
            if (s.lvl != LevelBehavior.lvl)
                return;
            //treat the object as if it had been placed correctly by the player just now
            this.gameObject.transform.SetParent(null, true);
            Destroy(this.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>());
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            Destroy(this.gameObject.GetComponent<Collider>());
            this.grabbed = true;
            this.gameObject.transform.localPosition = s.localPosition;
            this.gameObject.transform.localScale = s.localScale;
            this.gameObject.transform.localRotation = s.localRotation;
            LevelBehavior.Find();
        }
    }

    //initial configuration as part of the inventory
    //ensures that an unplaced item follows the inventory and doesn't interact with locations
    void Start()
    {
        r = this.gameObject.GetComponent<Rigidbody>();
        r.isKinematic = true;
        this.startpos = this.transform.localPosition;
        this.initialscale = this.gameObject.transform.localScale;
        this.initialrotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //if the item has been dropped, ensure that it returns to the inventory if it has not been placed correctly
        if ((!this.grabbed))
        {
            this.lost--;
            if (this.lost < 0)
            {
                this.transform.SetParent(this.inventory.gameObject.transform, false);
                this.gameObject.transform.localScale = this.initialscale;
                this.transform.localPosition = this.startpos;
                this.gameObject.transform.localRotation = this.initialrotation;
                r.isKinematic = true;
                this.grabbed = true;
                this.lost = 300;
            }
        }
    }

    //methods that handle behavior upon being grabbed or let go of
    public void Focus()
    {
        this.gameObject.transform.localScale = intermediateScale;
    }

    public void Grab()
    {
        this.grabbed = true;
    }
    public void UnGrab()
    {
        this.logger++;
        this.transform.SetParent(null, true);
        r.WakeUp();
        r.isKinematic = false;
        r.WakeUp();
        this.grabbed = false;
    }

    //check whether the item has been placed correctly
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Location>())
        {
            //is it the correct location?
            if(other.gameObject.GetComponent<Location>().id == this.id && !this.grabbed)
            {
                float dist = Vector3.Distance(this.transform.position, other.transform.position);
                //Log the hit
                LevelBehavior.logdata += ("Found " + this.id + " with a distance of " + dist + " after " + Time.timeSinceLevelLoad + " and " + this.logger + " tries.\n");
                //write persistent statistics
                timeAcc += (Time.timeSinceLevelLoadAsDouble-lastTime);
                lastTime = Time.timeSinceLevelLoadAsDouble;
                distAcc += dist;
                LevelBehavior.tries += this.logger;
                //increase score: ((4 - distance) / tries) * 10 
                LevelBehavior.score += ((4.0f - dist) / this.logger) * 10;
                //turn down audio
                other.gameObject.GetComponent<Location>().Found();
                //activate the particle system if available
                if(this.id == "blood")
                {
                    this.blood.SetActive(true);
                    this.gameObject.transform.GetComponentInChildren<MeshFilter>().gameObject.SetActive(false);
                    
                }
                //snap to the precisely correct position
                this.gameObject.transform.localScale = other.gameObject.transform.localScale;
                this.gameObject.transform.SetPositionAndRotation(other.gameObject.transform.position, other.gameObject.transform.rotation);
                //store the new position for future reloads
                this.Store();
                //remove interactions to prevent future unnecessary checks
                Destroy(this.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>());
                Destroy(this.gameObject.GetComponent<Rigidbody>());
                Destroy(this.gameObject.GetComponent<Collider>());
                Destroy(other);
                this.grabbed = true;
                //write the hit to the gamestate
                LevelBehavior.Find();
            }
        }
    }

}

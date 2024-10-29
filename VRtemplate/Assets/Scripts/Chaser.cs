using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script attached to the player character during chase sequences
public class Chaser : MonoBehaviour
{
    public List<Collider> hits = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //check whether the player has arrived at another corner of the chase path.
    //check for duplicates to accommodate confused players walking around the same corner multiple times.
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Chase>())
        {
            if (!hits.Contains(other))
            {
                hits.Add(other);
                other.GetComponentInParent<Chase>().Pass();
            }
            
        }
            
    }
}

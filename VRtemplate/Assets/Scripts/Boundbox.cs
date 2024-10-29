using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to reset pieces that are dropped out of the level by the player
public class Boundbox : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PuzzlePiece>() != null)
        {
            LevelBehavior.oopsies++;
            collision.gameObject.transform.position = new Vector3(0,1,-0.5f);
        }
    }
}

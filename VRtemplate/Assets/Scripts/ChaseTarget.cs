using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//target of a chase sequence, only relevant to track its progression.
//actual movement handled by ChaseManager
public class ChaseTarget : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //log that the target has arrived at another corner
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Chase>())
            other.GetComponentInParent<Chase>().Find();
    }
}


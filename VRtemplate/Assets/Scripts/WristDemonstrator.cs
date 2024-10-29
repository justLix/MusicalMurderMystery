using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls the NPC during the intro scene to show the appropriate pose to activate the hand menu
public class WristDemonstrator : MonoBehaviour
{
    [SerializeField] GameObject head;
    [SerializeField] GameObject upper;
    [SerializeField] GameObject lower;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject right;
    [SerializeField] GameObject handplane;
    [SerializeField] GameObject headplane;
    [SerializeField] GameObject arrow;
    public int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.headplane.SetActive(false);
        this.handplane.SetActive(false);
        this.arrow.SetActive(false);
    }

    private void Awake()
    {
        this.headplane.SetActive(false);
        this.handplane.SetActive(false);
        this.arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StateTransformer();
    }

    void StateTransformer()
    {
        switch (state)
        {
            case 0:
                if (upper.transform.rotation.eulerAngles.z > 40)
                {
                    upper.transform.Rotate(new Vector3(0, 0, -0.6f));
                    right.transform.Rotate(new Vector3(0, 0, -4.2f));
                }                    
                else
                    state = 1;
                break;
            case 1:
                if (upper.transform.rotation.eulerAngles.y > 240)
                    upper.transform.Rotate(new Vector3(0, -0.6f, 0));
                else
                    state = 2;
                break;
            case 2:
                if (lower.transform.rotation.eulerAngles.y > 320)
                    lower.transform.Rotate(new Vector3(0, -0.6f, 0));
                else
                    state = 3;
                break;
            case 3:
                if (hand.transform.rotation.eulerAngles.x < 50)
                {
                    hand.transform.Rotate(new Vector3(0.6f, 0, -0.78f));
                    head.transform.Rotate(new Vector3(0.14f, 0.84f, 0));
                }
                else
                {
                    this.headplane.SetActive(true);
                    this.handplane.SetActive(true);
                    this.arrow.SetActive(true);
                    state = -1;
                }
                break;
            default:
                return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//used in case 2 medium ending to check the correctness of the reconstruction
public class Evaluator : MonoBehaviour
{
    [SerializeField] Songbuilder songbuilder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Evaluate(bool right)
    {
        if(right)
        {
            if ((this.songbuilder.killer.value == 5) && (this.songbuilder.victim.value == 1) && (this.songbuilder.weapon.value == 6))
                SceneManager.LoadSceneAsync(7);
            else
                SceneManager.LoadSceneAsync(11);
        }else
            SceneManager.LoadSceneAsync(8);
    }
}

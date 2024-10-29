using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//variant of dialogue used in case 2 medium difficulty ending for the song reconstruction
public class Songbuilder : Dialogue
{
    //reconstruction menu that is shown after the initial dialogue
    [SerializeField] GameObject selection;
    [SerializeField] GameObject button;
    //dropdown menus that play corresponding instrumental themes upon selection
    [SerializeField] public TMP_Dropdown killer;
    [SerializeField] public TMP_Dropdown weapon;
    [SerializeField] public TMP_Dropdown victim;
    [SerializeField] List<AudioSource> killers; 
    [SerializeField] List<AudioSource> weapons; 
    [SerializeField] List<AudioSource> victims; 

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        this.selection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();   
    }

    public override void Next()
    {
        base.Next();
        if (this.i < this.texts.Length - 1)
            return;
        this.selection.SetActive(true);
        this.button.gameObject.SetActive(false);
        this.display.gameObject.SetActive(false);
        base.Last();
    }

    public void ChangeKiller()
    {
        foreach (var k in this.killers) {
            if(k)
                k.mute = true;
        }
        if(this.killer.value != 0)
            this.killers[this.killer.value].mute = false;
    }

    public void ChangeWeapon()
    {
        foreach (var w in this.weapons)
        {
            if(w)
                w.mute = true;
        }
        if (this.weapon.value != 0)
            this.weapons[this.weapon.value].mute = false;
    }

    public void ChangeVictim() 
    {
        foreach (var v in this.victims)
        {
            if(v)
                v.mute = true;
        }
        if (this.victim.value != 0)
            this.victims[this.victim.value].mute = false;
    }
}

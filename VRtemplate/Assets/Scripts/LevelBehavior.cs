using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//class that manages the overall gamestate
public class LevelBehavior : MonoBehaviour
{
    //properties of each individual level:

    //identification of level (relevant for win conditions and music):
    //  0 = any briefing
    //  1 = first search challenge of case 2
    //  2 = second search challenge of case 2
    //  3 = any chase sequence
    //  4 = first search challenge of case 1
    //  5 = second search challenge of case 1
    //  6 = first search challenge of case 3
    //  7 = second search challenge of case 3
    [SerializeField] public int level = 0;
    //audio mixer
    [SerializeField] public AudioMixer mix;
    //is the level the final briefing of a case?
    [SerializeField] bool final = false;
    //used for song reconstruction (case 2 medium difficulty finaele), otherwise empty
    [SerializeField] Songbuilder songbuilder;
    //button that is revealed when all items have been placed during a reconstruction challenge
    [SerializeField] GameObject ender;

    //static properties of the game state

    //globally available ID of the current level (failsafe for item reloads)
    public static int lvl = 0;
    //dictionary that stores placed items for level reloading, with the id of the item as key
    public static Dictionary<string, PuzzlePiece.SaveState> pieces = new Dictionary<string, PuzzlePiece.SaveState>();
    //cumulative log data written over the course of the game
    public static string logdata = "";
    //path location to a file to write persistent logs
    static string logfile = "";
    //player ID for a given playthrough. currently, this is the time of day at which the game was started.
    //during larger trials, this might need to be extended to accurately identify participants
    private static string playerID = null;
    //variables for in-game statistics
    public static int helped = 0;
    public static int oopsies = 0;
    public static double tries = 0;
    public static double score = 0.0f;
    private static double lastscore = 0.0f;
    private static int found = 0;

    //set global information needed for restoring of items (handled in individual Start() methods)
    private void Awake()
    {
        found = 0;
        lvl = this.level;
    }

    private void Start()
    {
        //begin the log
        if (logdata == "")
        {
            playerID = null;
            score = 0;
            logdata += "-------------------------------------------------------------\n" + GetPlayerID() + ":\n";
        }
        //set starting conditions for different types of levels
        switch (level)
        {
            //activate audio mixer snapshot for briefings
            case 0:
                ChangeAudio("Briefing");
                break;
            //activate audio mixer snapshot for the first reconstruction challenge of each case
            case 1:
            case 4:
            case 6:
                ChangeAudio("MainRoom");
                //should a player replay a case without exiting the game, the placed items for later levels may still be stored
                //if this is the case, remove them.
                //the killer is never introduced in the first level of a case. if it is still stored, that data needs to be deleted.
                if (pieces.ContainsKey("killer"))
                {
                    PuzzlePiece.timeAcc = 0.0f;
                    PuzzlePiece.lastTime = 0.0f;
                    PuzzlePiece.distAcc = 0.0f;
                    pieces.Clear();
                }
                break;
            //activate audio mixer snapshot for the second reconstruction challenge of each case
            //in this snapshot, the instruments that played during the day are still audible but quieter
            case 2:
            case 5:
            case 7:
                ChangeAudio("MainRoom Night");
                //should a player replay a case without exiting the game, the placed items for some levels may still be stored
                //if this is the case, remove them.
                //the victim is always present in the first but not the second level of a case. if it is still stored, that data needs to be deleted.
                if (pieces.ContainsKey("victim"))
                {
                    PuzzlePiece.timeAcc = 0.0f;
                    PuzzlePiece.lastTime = 0.0f;
                    PuzzlePiece.distAcc = 0.0f;
                    pieces.Clear();
                }
                break;
            //activate audio mixer snapshot for the chase sequences. this includes the distracting music snippets along the chase
            case 3:
                ChangeAudio("Chase");
                break;
        }
        //hide the finish button of a level, if included in a scene
        if(this.ender)
            this.ender.SetActive(false);
    }

    //wrapper for switching audio snapshots
    public void ChangeAudio(string name)
    {
        mix.TransitionToSnapshots(new AudioMixerSnapshot[] { mix.FindSnapshot(name) }, new float[] { 1.0f }, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //check for the current level whether the final number of items has been found
        switch (level)
        {
            case 1:
                if(found == 6)
                    this.ender.SetActive(true);
                break;
            case 2:
                if (found == 5)
                    this.ender.SetActive(true);
                break;
            case 4:
                if (found == 7)
                    this.ender.SetActive(true);
                break;
            case 5:
                if (found == 4)
                    this.ender.SetActive(true);
                break;
            case 6:
                if (found == 10)
                    this.ender.SetActive(true);
                break;
            default:
                break;
        }
    }

    //generates player id if not already existing, otherwise just returns it
    public static string GetPlayerID()
    {
        if (LevelBehavior.playerID == null) 
        {
            LevelBehavior.playerID = System.DateTime.Now.ToShortTimeString();
        }
        return playerID;
    }

    //log found items
    public static void Find()
    {
        found++;
    }

    //handle individual scene transitions, primarily logging recorded data and adding scores where necessary
    private void OnDestroy()
    {
        if (level == 1)
        {
            logdata += "Average time per object was " + (PuzzlePiece.timeAcc / 6.0f) + "\n";
            logdata += "Average distance per object was " + (PuzzlePiece.distAcc / 6.0f) + "\n";
        }
        else if (level == 2)
        {
            logdata += "Average time per object was " + (PuzzlePiece.timeAcc / 5.0f) + "\n";
            logdata += "Average distance per object was " + (PuzzlePiece.distAcc / 5.0f) + "\n";
        }
        else if (level == 3)
        {
            LevelBehavior.score += (80.0f - Time.timeSinceLevelLoad);
        }
        else if (this.songbuilder)
        {
            int correct = 0;
            if (this.songbuilder.killer.value == 5) correct += 1;
            if (this.songbuilder.victim.value == 1) correct += 1;
            if (this.songbuilder.weapon.value == 6) correct += 1;
            LevelBehavior.score += correct * 100.0f;
        }
        else if (level == 4)
        {
            logdata += "Average time per object was " + (PuzzlePiece.timeAcc / 7.0f) + "\n";
            logdata += "Average distance per object was " + (PuzzlePiece.distAcc / 7.0f) + "\n";
            
        }
        else if (level == 5)
        {
            logdata += "Average time per object was " + (PuzzlePiece.timeAcc / 4.0f) + "\n";
            logdata += "Average distance per object was " + (PuzzlePiece.distAcc / 4.0f) + "\n";
            
        }
        else if (level == 6)
        {
            logdata += "Average time per object was " + (PuzzlePiece.timeAcc / 10.0f) + "\n";
            logdata += "Average distance per object was " + (PuzzlePiece.distAcc / 10.0f) + "\n";
        }
        logdata += "Gained " + (score - lastscore) + " points.\n";
        lastscore = score;
        logdata += "Player spent " + Time.timeSinceLevelLoad + " in " + SceneManager.GetActiveScene().name + "\n--------------------------------------------\n";
        //during final levels of each case, write conclusive logs
        if (this.final)
        {
            logdata += "Needed help a total of " + helped + " times.\n";
            logdata += "Dropped " + oopsies + " items out of the world.\n";
            logdata += "Needed an average of " + (tries/11.0f) + " tries per item.\n";
            logdata += "Achieved a total score of " + score + "\n";
            Debug.Log(logdata);
            if(logfile != "")
            {
                StreamWriter writer = new StreamWriter(logfile, true);
                writer.Write(logdata);
                writer.Close();
            }
            //reset case specific data
            pieces.Clear();
            score = 0;
        }
            
    }

}

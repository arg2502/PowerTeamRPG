using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MorttimerStatue : StationaryNPCControl
{
    //Used to change the statue's orientation
    public bool flip = false;
    public bool side = false;
    float mult = 1.0f;
    float distanceToSpawn = 1.5f;
    bool canPlayerMove;
    
    void Save()
    {
        if (flip) { mult = -1.0f; }
        // set the current scene variable
        ////GameControl.control.currentScene = SceneManager.GetActiveScene().name;
        // heal the heroes
        foreach (DenigenData hd in GameControl.control.heroList)
        {
            hd.hp = hd.hpMax;
            hd.pm = hd.pmMax;
            //get rid of status effects -- add later
            hd.statusState = DenigenData.Status.normal;
        }
        // set this as the saved statue
        GameControl.control.taggedStatue = true;
        if (!side)
        {
            GameControl.control.savedStatue = new Vector2(this.transform.position.x, this.transform.position.y - distanceToSpawn);
            GameControl.control.currentPosition = new Vector2(this.transform.position.x, this.transform.position.y - distanceToSpawn);
        }
        else
        {
            GameControl.control.savedStatue = new Vector2(this.transform.position.x - (distanceToSpawn * mult), this.transform.position.y);
            GameControl.control.currentPosition = new Vector2(this.transform.position.x - (distanceToSpawn * mult), this.transform.position.y);
        }

        ////GameControl.control.RecordRoom();

        // Save the game
        GameControl.control.Save();

    }

    public override void ShowInteractionNotification(string message)
    {
        message = "Pray";

        base.ShowInteractionNotification(message);
    }

}

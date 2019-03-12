using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MorttimerStatue : NPCObject
{
    //Used to change the statue's orientation
    public bool flip = false;
    public bool side = false;
    float mult = 1.0f;
    float distanceToSave = 2.5f;
    float distanceToSpawn = 1.5f;
    bool canPlayerMove;

    // Use this for initialization
    void Start()
    {
        base.Start();

        //if (flip) { mult *= -1; }
        //gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;

        //base.Start();

        //canPlayerMove = player.GetComponent<characterControl>().canMove;
    }

    void Save()
    {
        if (flip) { mult = -1.0f; }
        //if (distFromPlayer < distanceToSave && Input.GetKeyUp(GameControl.control.selectKey) && canTalk && !canPlayerMove)
        //{
            // set the current scene variable
            GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
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
            if (side)
            {
                GameControl.control.savedStatue = new Vector2(this.transform.position.x - (distanceToSpawn * mult), this.transform.position.y);
                GameControl.control.currentPosition = new Vector2(this.transform.position.x - (distanceToSpawn * mult), this.transform.position.y);
            }

            GameControl.control.RecordRoom();

            // Save the game
            GameControl.control.Save();
        //}
        
    }
    
}

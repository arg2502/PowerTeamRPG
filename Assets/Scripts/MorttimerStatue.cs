using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MorttimerStatue : NPCObject {

    //Used to change the statue's orientation
    public bool flip = false;
    public bool side = false;
    float mult = 1.0f;

	// Use this for initialization
	void Start () {
        if (flip) { mult *= -1; }
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
        base.Start();
	}

    void Update()
    {
        if (flip) { mult = -1.0f; }
        if (distFromPlayer < 150.0f && Input.GetKeyUp(KeyCode.Space) && canTalk)
        {
            // set the current scene variable
            GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            // heal the heroes
            foreach(HeroData hd in GameControl.control.heroList)
            {
                hd.hp = hd.hpMax;
                hd.pm = hd.pmMax;
                //get rid of status effects -- add later
                hd.statusState = HeroData.Status.normal;
            }
            // set this as the saved statue
            GameControl.control.taggedStatue = true;
            if (!side)
            {
                GameControl.control.savedStatue = new Vector2(this.transform.position.x, this.transform.position.y - 100.0f);
                GameControl.control.currentPosition = new Vector2(this.transform.position.x, this.transform.position.y - 100.0f);
            }
            if (side)
            {
                GameControl.control.savedStatue = new Vector2(this.transform.position.x - (100.0f * mult), this.transform.position.y);
                GameControl.control.currentPosition = new Vector2(this.transform.position.x - (100.0f * mult), this.transform.position.y);
            }

            GameControl.control.RecordRoom();

            // Save the game
            GameControl.control.Save();
        }
        base.Update();
    }

	void FixedUpdate () {
        base.FixedUpdate();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MorttimerStatue : NPCObject {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
        base.Start();
	}

    void Update()
    {
        if (distFromPlayer < 150.0f && Input.GetKeyUp(KeyCode.Space) && canTalk)
        {
            // heal the heroes
            foreach(HeroData hd in GameControl.control.heroList)
            {
                hd.hp = hd.hpMax;
                hd.pm = hd.pmMax;
                //get rid of status effects -- add later
            }
            // set this as the saved statue
            GameControl.control.savedStatue = this.gameObject.transform;
            GameControl.control.currentPosition = player.transform.position;
            // Save the game
            GameControl.control.Save();
        }
        base.Update();
    }

	void FixedUpdate () {
        base.FixedUpdate();
	}
}

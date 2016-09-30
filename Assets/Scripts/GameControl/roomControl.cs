using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class roomControl : MonoBehaviour {

    // store the default entrance of the room
    // this is where the player will be sent when they die if no statue has been tagged
    public Vector2 entrance;

    // Attributes dealing with enemies
    public int areaLevel; // the locked level of enemies in the area
    public int numOfEnemies; //number of enemies walking around
    public List<Enemy> possibleEnemies;// Array of possible enemy types
    List<enemyControl> enemies = new List<enemyControl>();
    public GameObject enemyControlPrefab; // a generic enemy control object

	// Use this for initialization
	void Start () {
        //tell the gameControl object what it needs to know
        GameControl.control.areaEntrance = entrance;

        //create the appropriate amount of enemies
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject temp = GameObject.Instantiate(enemyControlPrefab);
            temp.transform.position = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f)); // hopefully we will have a better way of placing enemies
            temp.GetComponent<enemyControl>().maxEnemies = 4; // maybe this shouldn't be here
            enemies.Add(temp.GetComponent<enemyControl>());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

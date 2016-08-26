using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class roomControl : MonoBehaviour {

    // Attributes dealing with enemies
    public int areaLevel; // the locked level of enemies in the area
    public int numOfEnemies; //number of enemies walking around
    public List<Enemy> possibleEnemies;// Array of possible enemy types
    List<enemyControl> enemies = new List<enemyControl>();
    public GameObject enemyControlPrefab; // a generic enemy control object

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject temp = GameObject.Instantiate(enemyControlPrefab);
            temp.transform.position = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f)); // hopefully we will have a better way of placing enemies
            enemies.Add(temp.GetComponent<enemyControl>());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyControl : OverworldObject {

    //attributes
    float moveSpeed = 0;
    float walkSpeed = 200;
    float runSpeed = 325;
    Vector2 speed;

    public Transform player;
    public float dist = 0;
    float safeDistance;

    //These are useful for contolling the enemy's movement
    float waitTimer;
    float walkTimer;
    float timer;
    List<Vector2> directions = new List<Vector2>() { new Vector2(1.0f, 0.0f), new Vector2(-1.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, -1.0f) };

    enum State { wait, walk, pursue };
    State state;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<characterControl>().transform;
        state = State.wait;
        walkTimer = 3.0f;
        waitTimer = 2.0f;
        safeDistance = 500.0f;
        base.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //iterate timer
        timer += Time.deltaTime;

        //check player's distance from enemy
        dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
            + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
        if(dist <= safeDistance) { state = State.pursue; }

        if (state == State.wait)
        {
            if (timer >= waitTimer)
            {
                state = State.walk;
                //pick a direction to walk
                int dir = Random.Range(0, 3);
                speed = directions[dir] * walkSpeed * Time.deltaTime;
                //reset the timer
                timer = 0.0f;
            }
        }
        else if (state == State.walk)
        {
            transform.Translate(speed);
            if (timer >= walkTimer)
            {
                state = State.wait;
                timer = 0.0f;
            }
        }
        else
        {
            speed = Vector2.zero;

            if (player.position.x < transform.position.x - 5.0f) { speed += directions[1] * runSpeed * Time.deltaTime; }
            else if (player.position.x > transform.position.x + 5.0f) { speed += directions[0] * runSpeed * Time.deltaTime; }
            if (player.position.y < transform.position.y - 5.0f) { speed += directions[3] * runSpeed * Time.deltaTime; }
            else if (player.position.y > transform.position.y + 5.0f) { speed += directions[2] * runSpeed * Time.deltaTime; }

            transform.Translate(speed);

            if (dist <= 15.0f)
            {
                player.position = GameControl.control.currentPosition; //record the player's position before entering battle
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
                // Recieve the battle info from the enemy, such as enemy types and # of enemies -- ADD LATER
                UnityEngine.SceneManagement.SceneManager.LoadScene("testMenu"); // load the battle scene
            }
        }
	}
}

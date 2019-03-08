using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyControl : OverworldObject {

    //attributes
    float moveSpeed = 0;
    float walkSpeed = 3f;
    float coolDownSpeed = 2f;
    float runSpeed = 5.5f;
    Vector2 speed;

    public Transform player;
    public float dist = 0;
    public float safeDistance;

    public int maxEnemies = 0;
    public int minEnemies = 0;

    //These are useful for contolling the enemy's movement
    float waitTimer;
    float walkTimer;
    float pursueTimer;
    float timer;
    float coolDownTimer;
    int dir; // the direction the enemy chooses
    List<Vector2> directions = new List<Vector2>() { new Vector2(1.0f, 0.0f), new Vector2(-1.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, -1.0f) };

    enum State { wait, walk, pursue, coolDown };
    State state;

    roomControl rc; // a reference to the roomControl object, which will dictate enemy specifics
    int numOfEnemies; // number of enemies this object carries
    public List<EnemyData> enemies; // the enemies this object carries
    public List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    float distance = 1.5f;

    Animator anim;

    public bool beenPlaced; // true if the enemy has been arbitrarily placed
	public bool beenBattled; // true if you battled/fleed this enemy - all normal enemies should be set back to false when player dies

	public Vector3 currentPosition;

    float sideHitFloat = 0.25f;
    float topHitFloat = 0.07f;
    float bottomHitFloat = 0.035f;

	// Use this for initialization
	void Start () {
		canMove = true;
        anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer> ();
        player = FindObjectOfType<characterControl>().transform;
        state = State.wait;
        walkTimer = 3.0f;
        waitTimer = 2.0f;
        pursueTimer = 5.0f;
        coolDownTimer = 1.5f;
        safeDistance = 8.0f;

        rc = GameObject.FindObjectOfType<roomControl>();

		// vector3 to store updated player location
		currentPosition = GameControl.control.currentPosition;

		dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - currentPosition.x) * (transform.position.x - currentPosition.x))
			+ ((transform.position.y - currentPosition.y) * (transform.position.y - currentPosition.y))));
		
		// set enabled state if you battled and close to it
		// mainly for coming out of pause sub menus
		if (beenBattled && dist <= safeDistance + 1.5f) {
            //sr.enabled = false;
            //enabled = false;
            gameObject.SetActive(false);
		} 

        //check player's distance from enemy
        if (!GameControl.control.isPaused)
        {
			if (dist <= safeDistance + 1.5f) {
				// if the enemy has been arbitrarily place, destroy it
				if (beenPlaced) {
                    //sr.enabled = false;
                    //enabled = false;
                    gameObject.SetActive(false);
				}
				// if it's a random enemy, push it somewhere else
				else {
					while (dist <= safeDistance + 1.5f) {
						transform.position = new Vector2 (Random.Range (-15.0f, 15.0f), Random.Range (-15.0f, 15.0f));
						dist = Mathf.Abs (Mathf.Sqrt (((transform.position.x - currentPosition.x) * (transform.position.x - currentPosition.x))
							+ ((transform.position.y - currentPosition.y) * (transform.position.y - currentPosition.y))));
					}
				}
			} 

			// if enemy is stuck in wall, push them out
			// only for randomly placed enemies
			if (!beenPlaced) {
				for (int i = 0; i < 2; i++) {
					raycastHits.Add (Physics2D.Raycast (transform.position, directions [i], distance, mask));
				}
				foreach (RaycastHit2D rh in raycastHits) {
					if (rh.collider != null) {
						OverworldObject owo = rh.collider.GetComponent<OverworldObject> ();
						int loopCounter = 0; // keep track of the number of loops
						while ((raycastHits [0].collider != null//owo.GetComponent<Collider2D>()
						                     || raycastHits [1].collider != null)//owo.GetComponent<Collider2D>()) 
						                     || (dist <= safeDistance + 1.5f)) {
							transform.position = new Vector2 (Random.Range (-15.0f, 15.0f), Random.Range (-15.0f, 15.0f));
							print ("hit");
							dist = Mathf.Abs (Mathf.Sqrt (((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
							+ ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));

							raycastHits = new List<RaycastHit2D> ();
							for (int i = 0; i < 2; i++) {
								raycastHits.Add (Physics2D.Raycast (transform.position, directions [i], distance, mask));
							}
							loopCounter++;
							// if looping excedes a number, just quit
							if (loopCounter >= 50) {
								Destroy (this.gameObject);
							}
						}
					}
				}
			}
        }

        numOfEnemies = Random.Range(minEnemies, maxEnemies + 1);

        base.Start();

	}
	public void UpdateDist()
	{
		dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - currentPosition.x) * (transform.position.x - currentPosition.x))
			+ ((transform.position.y - currentPosition.y) * (transform.position.y - currentPosition.y))));
	}
	// Update is called once per frame
	void FixedUpdate () {

            for (int i = 0; i < raycastHits.Count; i++)
            {
                if (raycastHits[i].collider != null)
                {
                Debug.DrawLine(raycastHits[i].transform.position, raycastHits[i].point);
                }
            }
            
        sr.sortingOrder = (int)-transform.position.y;

        // don't animate if the game is paused
        if (!canMove)
        {
            anim.speed = 0;
        }

        if (canMove)
        {
            anim.speed = 1;

            //iterate timer
            timer += Time.deltaTime;

            //check player's distance from enemy
            dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
                + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));

            if (state == State.wait)
            {
                CheckForBattle();
                if (dist <= safeDistance) { state = State.pursue; timer = 0.0f; }

                if (timer >= waitTimer)
                {
                    state = State.walk;
                    //pick a direction to walk
                    dir = Random.Range(0, 4);
                    speed = directions[dir] * walkSpeed * Time.deltaTime;
                    //reset the timer
                    timer = 0.0f;
                }
            }
            else if (state == State.walk)
            {
                CheckCollision();
                CheckForBattle();

                if (dist <= safeDistance) { state = State.pursue; timer = 0.0f; }

                if (timer >= walkTimer)
                {
                    state = State.wait;
                    timer = 0.0f;
                }
            }
            else if (state == State.coolDown)
            {
                CheckCollision();
                CheckForBattle();

                if (timer >= coolDownTimer)
                {
                    state = State.wait;
                    timer = 0.0f;
                }
            }
            else
            {
                speed = Vector2.zero;

                if (player.position.x < transform.position.x - 0.1f) { speed += directions[1] * runSpeed * Time.deltaTime; }
                else if (player.position.x > transform.position.x + 0.1f) { speed += directions[0] * runSpeed * Time.deltaTime; }
                if (player.position.y < transform.position.y - 0.1f) { speed += directions[3] * runSpeed * Time.deltaTime; }
                else if (player.position.y > transform.position.y + 0.1f) { speed += directions[2] * runSpeed * Time.deltaTime; }

                CheckCollision();
                CheckForBattle();

                if (timer >= pursueTimer)
                {
                    state = State.coolDown;
                    timer = 0.0f;
                    int prevDir = dir;
                    while (dir == prevDir) { dir = Random.Range(0, 4); }
                    speed = directions[dir] * coolDownSpeed * Time.deltaTime;
                }
            }           
        }
	}

    void CheckCollision()
    {
        RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y + topHitFloat, transform.position.z), speed, 0.5f, mask);
        RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), speed, 0.5f, mask);
                
        if((topHit.collider != null && !topHit.collider.GetComponent<characterControl>())
            || bottomHit.collider != null && !bottomHit.collider.GetComponent<characterControl>())
        {
            state = State.coolDown;
            timer = 0.0f;
            int prevDir = dir;
            while (dir == prevDir) { dir = Random.Range(0, 3); }
            speed = directions[dir] * coolDownSpeed * Time.deltaTime;
        }
        else
        {
            transform.Translate(speed);
        }
    }

    void CheckForBattle()
    {
        if (dist <= 0.25f && GameControl.control.currentCharacterState == characterControl.CharacterState.Normal)
        {
            GameControl.control.currentCharacterState = characterControl.CharacterState.Battle;
            GameControl.control.currentPosition = player.position; //record the player's position before entering battle
            GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene

            // if the enemies are not preset, set then based on the Room Control obj here
            if (enemies.Count == 0)
            {
                numOfEnemies = Random.Range(minEnemies, maxEnemies + 1);
                enemies = new List<EnemyData>();
                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemies.Add(rc.possibleEnemies[Random.Range(0, rc.possibleEnemies.Count)]);
                }
            }
            // Recieve the battle info from the enemy, such as enemy types and # of enemies
            GameControl.control.numOfEnemies = numOfEnemies;
            GameControl.control.enemies = enemies;

            beenBattled = true;
            //save the current room, to acheive persistency while paused
            GameControl.control.RecordRoom();

            GameControl.audioManager.StartMusic(GameControl.control.battleIntro, GameControl.control.battleLoop, true, false);

            //UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene"); // load the battle scene
            GameControl.control.LoadSceneAsync("BattleScene");
        }
    }
}

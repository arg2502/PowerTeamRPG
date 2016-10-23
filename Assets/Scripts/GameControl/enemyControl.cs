using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyControl : OverworldObject {

    //attributes
    float moveSpeed = 0;
    float walkSpeed = 200;
    float coolDownSpeed = 150;
    float runSpeed = 375;
    Vector2 speed;

    public Transform player;
    public float dist = 0;
    float safeDistance;

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
    List<Enemy> enemies; // the enemies this object carries
    public List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    float distance = 100.0f;

	// Use this for initialization
	void Start () {

        canMove = true;

        player = GameObject.FindObjectOfType<characterControl>().transform;
        state = State.wait;
        walkTimer = 3.0f;
        waitTimer = 2.0f;
        pursueTimer = 5.0f;
        coolDownTimer = 1.5f;
        safeDistance = 500.0f;

        rc = GameObject.FindObjectOfType<roomControl>();

        //check player's distance from enemy
        if (!GameControl.control.isPaused)
        {
            do
            {
                transform.position = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f));
                dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
                + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
            } while (dist <= safeDistance + 100.0f);

            // if both top and bottom are both colliding with object, it's stuck
            //RaycastHit2D topHit = Physics2D.Raycast(transform.position, directions[2], 30.0f, mask);
            //RaycastHit2D bottomHit = Physics2D.Raycast(transform.position, directions[3], 30.0f, mask);
            for (int i = 0; i < 2; i++ )
            {
                raycastHits.Add(Physics2D.Raycast(transform.position, directions[i], distance, mask));
            }
            foreach (RaycastHit2D rh in raycastHits)
            {
                if (rh.collider != null)
                {
                    //print("inside if: " + topHit.collider);
                    OverworldObject owo = rh.collider.GetComponent<OverworldObject>();
                    while (raycastHits[0].collider == owo.GetComponent<Collider2D>()
                            || raycastHits[1].collider == owo.GetComponent<Collider2D>())
                            //|| raycastHits[2].collider == owo.GetComponent<Collider2D>()
                            //|| raycastHits[3].collider == owo.GetComponent<Collider2D>())
                    {

                        transform.position += owo.offset;
                        print(name + ": " + transform.position);
                        for (int i = 0; i < raycastHits.Count; i++ )
                        {
                             raycastHits[i] = Physics2D.Raycast(transform.position, directions[i], distance, mask);
                        }
                
                        if (raycastHits[0].collider == null
                        || raycastHits[1].collider == null)
                        //&& raycastHits[2].collider == null
                        //&& raycastHits[3].collider == null) 
                        { break; }
                        else if (raycastHits[0].collider != null)
                        {
                            owo = raycastHits[0].collider.GetComponent<OverworldObject>();
                        }
                        else if (raycastHits[1].collider != null)
                        {
                             owo = raycastHits[1].collider.GetComponent<OverworldObject>();
                        }
                        //else if (raycastHits[2].collider != null)
                        //{
                        //     owo = raycastHits[2].collider.GetComponent<OverworldObject>();
                        //}
                        //else if (raycastHits[3].collider != null)
                        //{
                        //     owo = raycastHits[3].collider.GetComponent<OverworldObject>();
                        //}
                    }
                }
            }
        }
        
        numOfEnemies = Random.Range(minEnemies, maxEnemies + 1);
        enemies = new List<Enemy>();
        for (int i = 0; i < numOfEnemies; i++)
        {
            enemies.Add(rc.possibleEnemies[Random.Range(0, rc.possibleEnemies.Count)]);
        }

        base.Start();
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

        if (canMove)
        {
            //iterate timer
            timer += Time.deltaTime;

            //check player's distance from enemy
            dist = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
                + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));

            if (state == State.wait)
            {

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
                checkCollision();

                if (dist <= safeDistance) { state = State.pursue; timer = 0.0f; }

                if (timer >= walkTimer)
                {
                    state = State.wait;
                    timer = 0.0f;
                }
            }
            else if (state == State.coolDown)
            {
                checkCollision();

                if (timer >= coolDownTimer)
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

                checkCollision();

                if (dist <= 15.0f)
                {
                    GameControl.control.currentPosition = player.position; //record the player's position before entering battle
                    GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene

                    // Recieve the battle info from the enemy, such as enemy types and # of enemies
                    GameControl.control.numOfEnemies = numOfEnemies;
                    GameControl.control.enemies = enemies;

                    //save the current room, to acheive persistency while paused
                    GameControl.control.RecordRoom();

                    UnityEngine.SceneManagement.SceneManager.LoadScene("testMenu"); // load the battle scene
                }

                if (timer >= pursueTimer)
                {
                    state = State.coolDown;
                    timer = 0.0f;
                    int prevDir = dir;
                    while (dir == prevDir) { dir = Random.Range(0, 4); }
                    speed = directions[dir] * coolDownSpeed * Time.deltaTime;
                }
            }

            //foreach (RaycastHit2D rh in raycastHits)
            //{
            //    if (rh.collider != null)
            //    {
            //        //print("inside if: " + topHit.collider);
            //        OverworldObject owo = rh.collider.GetComponent<OverworldObject>();
            //        if (raycastHits[0].collider == owo.GetComponent<Collider2D>()
            //                || raycastHits[1].collider == owo.GetComponent<Collider2D>())
            //        //|| raycastHits[2].collider == owo.GetComponent<Collider2D>()
            //        //|| raycastHits[3].collider == owo.GetComponent<Collider2D>())
            //        {

            //            transform.position += owo.offset;
            //            print(name + ": " + transform.position);
            //            for (int i = 0; i < raycastHits.Count; i++)
            //            {
            //                raycastHits[i] = Physics2D.Raycast(transform.position, directions[i], distance, mask);
            //            }

            //            if (raycastHits[0].collider == null
            //            && raycastHits[1].collider == null)
            //            //&& raycastHits[2].collider == null
            //            //&& raycastHits[3].collider == null) 
            //            { break; }
            //            else if (raycastHits[0].collider != null)
            //            {
            //                owo = raycastHits[0].collider.GetComponent<OverworldObject>();
            //            }
            //            else if (raycastHits[1].collider != null)
            //            {
            //                owo = raycastHits[1].collider.GetComponent<OverworldObject>();
            //            }
            //            //else if (raycastHits[2].collider != null)
            //            //{
            //            //     owo = raycastHits[2].collider.GetComponent<OverworldObject>();
            //            //}
            //            //else if (raycastHits[3].collider != null)
            //            //{
            //            //     owo = raycastHits[3].collider.GetComponent<OverworldObject>();
            //            //}
            //        }
            //    }
            //}
        }
	}

    void checkCollision()
    {
        RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y + 5.0f, transform.position.z), speed, 30.0f, mask);
        RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 10.0f, transform.position.z), speed, 30.0f, mask);
        if (topHit.collider == null && bottomHit.collider == null)
        {
            transform.Translate(speed);
        }        
        else
        {
            state = State.coolDown;
            timer = 0.0f;
            int prevDir = dir;
            while (dir == prevDir) { dir = Random.Range(0, 3); }
            speed = directions[dir] * coolDownSpeed * Time.deltaTime;
        }
    }
}

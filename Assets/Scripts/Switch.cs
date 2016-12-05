using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Switch : OverworldObject {

    public enum SwitchType { activateObj, createObj, openDoor, colorSwitch };
    // activate object affects a non-door obj in the room
    // create object does what it's name implies
    // open door opens an already existing door

    public SwitchType switchType; // determines what the switch will do once activated

    public bool isActivated = false; // whether or not the player touched the switch

    public List<GameObject> affectedObjs; // stores a list of objects to be effected by the switch
    // blocks will have an active bool, so they always exist, but the switch activates them
    //public List<Vector2> objPositions; // stores the positions of newly created objects, such as blocks
    
    float distFromPlayer;
    Transform player;

    //Color origColor;
    public Sprite passiveSprite;
    public Sprite activeSprite;

	float timer = 0.0f;
	float maxTime = 1.66f;

	// Use this for initialization
	void Start () {
        base.Start();
        player = GameObject.FindObjectOfType<characterControl>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space) && !isActivated)
        {
            distFromPlayer = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
            + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));

            if (distFromPlayer < 100.0f)
            {
                isActivated = true;
                //sr.color = Color.green; // would change the sprite, but for now just change color

                // perform the appropriate activation
                if (switchType == SwitchType.createObj) 
                { 
                    foreach (GameObject go in affectedObjs) 
                    {
                        if (go.GetComponent<MovableOverworldObject>() != null) 
                        { 
                            go.GetComponent<MovableOverworldObject>().isActivated = true; 
                            go.SetActive(true);
                        }
                    }

                    sr.sprite = activeSprite;
                }
                else if(switchType == SwitchType.openDoor)
                {
                    foreach(GameObject go in affectedObjs)
                    {
                        go.SetActive(false);
                    }
                    
                    sr.sprite = activeSprite;
                }
                else if(switchType == SwitchType.activateObj)
                {
                    foreach(GameObject go in affectedObjs)
                    {
                        if(go.GetComponent<OverworldObject>() != null)
                        {
                            go.GetComponent<OverworldObject>().Activate();
                        }
                    }

                    sr.sprite = activeSprite;
                }
                else if(switchType== SwitchType.colorSwitch)
                {
                    foreach(GameObject go in affectedObjs)
                    {
                        if (go.GetComponent<ColorBridge>() != null)
                        {
                            if (!go.GetComponent<ColorBridge>().isMoving)
                            {
                                go.GetComponent<ColorBridge>().Activate();
                                //isActivated = false;
                            }
                        }
                    }
                    if (sr.sprite == passiveSprite)
                    {
                        sr.sprite = activeSprite;
                    }
                    else if (sr.sprite == activeSprite)
                    {
                        sr.sprite = passiveSprite;
                    }
                }                
            }
        }
        // check for bridges still moving
        
        int allClear = 0;

        if (switchType == SwitchType.colorSwitch)
        {		

            /*foreach (GameObject go in affectedObjs)
            {
                if (go.GetComponent<ColorBridge>() != null)
                {
					// keep track of how many bridges have stopped moving
					if (!go.GetComponent<ColorBridge> ().isMoving) {
						allClear++;
					} 

                }
            }*/

			// timer to start movement again
			if (isActivated) {
				//print (timer);
				if (timer < maxTime) {					
					timer += Time.deltaTime;
				} else {
					timer = maxTime;
					ToggleMovement ();
					isActivated = false;
					//print ("toggled");
				}
			} else {
				timer = 0.0f;
			}

            // all bridges have stopped, set isActivated equal to false
            /*if(allClear == affectedObjs.Count)
            {
                isActivated = false;
            }
			else
			{
				isActivated = true;
            }*/
        }
	}    
}

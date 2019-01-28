﻿using UnityEngine;
using System.Collections;

public class Gateway : MonoBehaviour {

    // the specific name of the gateway.
    // In the next room, this door will have a twin with the same way -- that is the gateway where the player will show up at
    public string gatewayName;

    // the scene that will load when the player enters the gateway
    public string sceneName;

    // place where the player will end up upon entering
    internal Vector2 entrancePos;

    // place where the player will end up upon exiting
    internal Vector2 exitPos;

    float transitionDist = 5.0f;

    public enum Direction
    {
        North,
        South,
        East,
        West
    }
    public Direction direction;

    public enum Type
    {
        NORMAL,
        DOOR
    }
    public Type gatewayType;

    void Awake()
    {
        SetPositions();
    }

    public void NextScene()
    {
        GameControl.control.AssignEntrance(gatewayName);
        GameControl.control.RecordRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void SetPositions()
    {
        if(direction == Direction.North)
        {
            entrancePos = new Vector2(transform.position.x, transform.position.y - transitionDist);
            exitPos = new Vector2(transform.position.x, transform.position.y + transitionDist);
        }
        else if (direction == Direction.South)
        {
            entrancePos = new Vector2(transform.position.x, transform.position.y + transitionDist);
            exitPos = new Vector2(transform.position.x, transform.position.y - transitionDist);
        }
        else if (direction == Direction.East)
        {
            entrancePos = new Vector2(transform.position.x - transitionDist, transform.position.y);
            exitPos = new Vector2(transform.position.x + transitionDist, transform.position.y);
        }
        else if (direction == Direction.West)
        {
            entrancePos = new Vector2(transform.position.x + transitionDist, transform.position.y);
            exitPos = new Vector2(transform.position.x - transitionDist, transform.position.y);
        }
    }


}

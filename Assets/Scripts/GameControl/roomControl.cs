﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class roomControl : MonoBehaviour {

    // store the default entrance of the room
    // this is where the player will be sent when they die if no statue has been tagged
    public Vector2 entrance;

    // Attributes dealing with enemies
    public int areaIDNumber; // helps the gameControl obj know what data to synch
    public int areaLevel; // the locked level of enemies in the area
    public int numOfEnemies; //number of enemies walking around
    public List<Enemy> possibleEnemies;// Array of possible enemy types
    List<enemyControl> enemies = new List<enemyControl>();
    public GameObject enemyControlPrefab; // a generic enemy control object
    public int minEnemiesPerBattle;
    public int maxEnemiesPerBattle;
    public List<MovableOverworldObject> movables = new List<MovableOverworldObject>();
    public List<TreasureChest> treasureChests = new List<TreasureChest>();
    public List<DoorQuestion> doorsInRoom = new List<DoorQuestion>();
	// Use this for initialization
	void Start () {
        //tell the gameControl object what it needs to know
        GameControl.control.areaEntrance = entrance;

        // convert arrays to lists
        foreach(MovableOverworldObject m in GameObject.FindObjectsOfType<MovableOverworldObject>())
        {
            movables.Add(m);
        }
        foreach(TreasureChest tc in GameObject.FindObjectsOfType<TreasureChest>())
        {
            treasureChests.Add(tc);
        }
        foreach(DoorQuestion dq in GameControl.FindObjectsOfType<DoorQuestion>())
        {
            doorsInRoom.Add(dq);
        }
        //create the appropriate amount of enemies
        if (!GameControl.control.isPaused)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                GameObject temp = GameObject.Instantiate(enemyControlPrefab);
                temp.transform.position = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f)); // hopefully we will have a better way of placing enemies
                temp.GetComponent<enemyControl>().minEnemies = minEnemiesPerBattle;
                temp.GetComponent<enemyControl>().maxEnemies = maxEnemiesPerBattle; // maybe this shouldn't be here
                enemies.Add(temp.GetComponent<enemyControl>());
            }
        }
        else if (GameControl.control.isPaused)
        {
            for (int i = 0; i < GameControl.control.enemyPos.Count; i++)
            {
                GameObject temp = GameObject.Instantiate(enemyControlPrefab);
                temp.transform.position = new Vector2(GameControl.control.enemyPos[i].x, GameControl.control.enemyPos[i].y); // hopefully we will have a better way of placing enemies
                temp.GetComponent<enemyControl>().minEnemies = minEnemiesPerBattle;
                temp.GetComponent<enemyControl>().maxEnemies = maxEnemiesPerBattle; // maybe this shouldn't be here
                enemies.Add(temp.GetComponent<enemyControl>());
            }
        }

        // Check if this room is already tracked by the game control obj - if not, make it so!
        foreach (RoomControlData rc in GameControl.control.rooms)
        {
            if (areaIDNumber == rc.areaIDNumber) 
            {
                // synch the data
                // first the blocks -- since that's what ive got
                for (int i = 0; i < movables.Count; i++)
                {
                    movables[i].transform.position = new Vector3(rc.movableBlockPos[i].x, rc.movableBlockPos[i].y, rc.movableBlockPos[i].z);
                }
                // sync open chests
                for (int i = 0; i < treasureChests.Count; i++)
                {
                    for (int j = 0; j < treasureChests.Count; j++)
                    {
                        if (treasureChests[i].name == rc.chestData[j].chestName)
                        {
                            treasureChests[i].isOpen = rc.chestData[j].isChestOpen;
                        }
                    }
                }
                // sync doors
                for (int i = 0; i < doorsInRoom.Count; i++)
                {
                    for (int j = 0; j < doorsInRoom.Count; j++)
                    {
                        if (doorsInRoom[i].name == rc.doorData[j].doorName)
                        {
                            doorsInRoom[i].gameObject.SetActive(rc.doorData[j].isLocked);
                        }
                    }
                }
                
                return;
            }
        }
        // if we're here, this room must not be currently tracked. Lets add it
        GameControl.control.rooms.Add(new RoomControlData());
        GameControl.control.rooms[GameControl.control.rooms.Count - 1].areaIDNumber = areaIDNumber;
        // add the necessary data to store all movable object positions
        foreach (MovableOverworldObject m in movables)
        {
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos.Add(new SerializableVector3());
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos[GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos.Count - 1].x = m.transform.position.x;
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos[GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos.Count - 1].x = m.transform.position.y;
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos[GameControl.control.rooms[GameControl.control.rooms.Count - 1].movableBlockPos.Count - 1].x = m.transform.position.z;
        }
        for(int i = 0; i < treasureChests.Count; i++)
        {
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData.Add(new TreasureData());//isChestOpen.Add(tc.isOpen);
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData[i].isChestOpen = treasureChests[i].isOpen;
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData[i].chestName = treasureChests[i].name;
        }
        for(int i = 0; i < doorsInRoom.Count; i++)
        {
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData.Add(new DoorData());
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData[i].isLocked = doorsInRoom[i].gameObject.activeSelf;
            GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData[i].doorName = doorsInRoom[i].name;

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

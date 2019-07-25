﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<EnemyData> possibleEnemies;
    public GameObject enemyPrefab;
    public int numOfEnemies;
    public int minPossibleBattleEnemies, maxPossibleBattleEnemies;
    BoxCollider2D spawnArea;
    private float spawnXMin, spawnXMax, spawnYMin, spawnYMax;
    public float SpawnXMin { get { return spawnXMin; } }
    public float SpawnXMax { get { return spawnXMax; } }
    public float SpawnYMin { get { return spawnYMin; } }
    public float SpawnYMax { get { return spawnYMax; } }

    // Use this for initialization
    void Start () {
        spawnArea = GetComponent<BoxCollider2D>();
        spawnXMin = spawnArea.bounds.min.x;
        spawnXMax = spawnArea.bounds.max.x;
        spawnYMin = spawnArea.bounds.min.y;
        spawnYMax = spawnArea.bounds.max.y;

        for (int i = 0; i < numOfEnemies; i++)
        {
            var randomX = Random.Range(spawnXMin, spawnXMax);
            var randomY = Random.Range(spawnYMin, spawnYMax);

            var enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector2(randomX, randomY);
            enemy.GetComponent<enemyControl>().Init(minPossibleBattleEnemies, maxPossibleBattleEnemies, possibleEnemies);
        }

	}
	
}
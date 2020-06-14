using System.Collections;
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

    private void Start()
    {
        Spawn();
    }

    public void Spawn () {
        spawnArea = GetComponent<BoxCollider2D>();
        spawnXMin = spawnArea.bounds.min.x;
        spawnXMax = spawnArea.bounds.max.x;
        spawnYMin = spawnArea.bounds.min.y;
        spawnYMax = spawnArea.bounds.max.y;

        for (int i = 0; i < numOfEnemies; i++)
        {
            var enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = GetNewPosition();
            enemy.GetComponent<enemyControl>().Init(minPossibleBattleEnemies, maxPossibleBattleEnemies, possibleEnemies);
        }

	}

    public Vector2 GetNewPosition()
    {
        var randomX = Random.Range(spawnXMin, spawnXMax);
        var randomY = Random.Range(spawnYMin, spawnYMax);

        return new Vector2(randomX, randomY);
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<EnemyData> possibleEnemies;
    public GameObject enemyPrefab;
    public int numOfEnemies;
    public int minPossibleBattleEnemies, maxPossibleBattleEnemies;
    BoxCollider2D spawnArea;

    // Use this for initialization
    void Start () {
        spawnArea = GetComponent<BoxCollider2D>();

        for(int i = 0; i < numOfEnemies; i++)
        {
            var randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var randomY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);

            var enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector2(randomX, randomY);
            enemy.GetComponent<enemyControl>().Init(minPossibleBattleEnemies, maxPossibleBattleEnemies, possibleEnemies);
        }

	}
	
}

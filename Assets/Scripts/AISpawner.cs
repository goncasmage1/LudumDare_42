using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    public float EnemySpawnDistance = 10f;
    public float SpawnInterval = 6f;
    [Range(0.01f, 1f)]
    public float SpawnIntervalReduction = 0.9f;

    public Transform enemyPrefab = null;
    private List<Enemy> enemies = new List<Enemy>();

	void Start () {
        SpawnNewEnemy();
	}
	
    void SpawnNewEnemy()
    {
        Vector3 newLocation = new Vector3(1f, 0f, 0f);
        newLocation = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * newLocation;
        Transform newTransform = Instantiate(enemyPrefab, newLocation * EnemySpawnDistance, Quaternion.identity);

        Enemy newEnemy = newTransform.GetComponent<Enemy>();
        if (newEnemy != null) enemies.Add(newEnemy);

        SpawnInterval *= SpawnIntervalReduction;
        Invoke("SpawnNewEnemy", SpawnInterval);
    }
}

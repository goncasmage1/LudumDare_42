﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    public Vector3 mapCenter = new Vector3(0.5f, 0f, 0.5f);
    public float EnemySpawnDistance = 10f;
    public float SpawnInterval = 6f;
    [Range(0.01f, 1f)]
    public float SpawnIntervalReduction = 0.9f;
    public float SpawnReduceCD = 2f;
    public PoolSpawner enemyPoolSpawner;

    public Transform[] spawnPoints;
    public Transform enemyPrefab = null;
    public List<Enemy> enemies = new List<Enemy>();
    public Transform target;

    private void Awake()
    {
        if (spawnPoints.Length == 0) Debug.LogError("No spawn points in AIManager!");
    }

    void Start () {
        SpawnNewEnemy();
        StartCoroutine("reduceCD");
	}

    void SpawnNewEnemy()
    {
        Vector3 newLocation = new Vector3(1f, 0f, 0f);
        newLocation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * newLocation;
        newLocation.y = 0;
        GameObject go = enemyPoolSpawner.Spawn(transform, spawnPoints[Random.Range(0, spawnPoints.Length-1)].position);
        Transform newTransform = go.transform;

        Enemy newEnemy = newTransform.GetComponent<Enemy>();
        if (newEnemy != null)
        {
            enemies.Add(newEnemy);
            newEnemy.GetComponent<EnemyHealth>().spawner = this;
            if (target != null) newEnemy.target = target;
        }

        Invoke("SpawnNewEnemy", SpawnInterval);
    }
    IEnumerator reduceCD()
    {
        while (true)
        {
            SpawnInterval *= SpawnIntervalReduction;
            yield return new WaitForSeconds(SpawnReduceCD);
        }
    }
    public void EnemyDied(Enemy deadEnemy)
    {
        enemies.Remove(deadEnemy);
    }
}

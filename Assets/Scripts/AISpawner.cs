using System.Collections;
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

    public float PortalToSpawnDelay = 0.3f;
    public float PortalDeactivateDelay = 0.2f;

    public GameObject spawnFX;
    public Transform enemyDeathFX;
    public Transform enemyStrongDeathFX;
    public Transform enemyPainFX;
    public Transform enemyStrongPainFX;
    public Transform enemyEatFX;
    public Transform enemyTransformFX;
    public Transform[] spawnPoints;
    public Transform enemyPrefab = null;
    public List<Enemy> enemies = new List<Enemy>();
    public Transform target;

    private void Awake()
    {
        if (spawnPoints.Length == 0) Debug.LogError("No spawn points in AIManager!");

        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            spawnPoints[i].GetChild(0).gameObject.SetActive(false);
        }
    }

    void Start () {
        StartCoroutine("SpawnNewEnemy");
        StartCoroutine("reduceCD");
	}

    IEnumerator SpawnNewEnemy()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length - 1);
            Vector3 newLocation = spawnPoints[randomIndex].position;
            spawnFX.transform.position = newLocation;
            spawnFX.transform.rotation = spawnPoints[randomIndex].rotation;
            spawnPoints[randomIndex].GetChild(0).gameObject.SetActive(true);

            yield return new WaitForSeconds(PortalToSpawnDelay);
            GameObject go = enemyPoolSpawner.Spawn(transform, newLocation);
            Transform newTransform = go.transform;

            Enemy newEnemy = newTransform.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                enemies.Add(newEnemy);
                newEnemy.spawner = this;
                newEnemy.GetComponent<EnemyHealth>().spawner = this;
                if (target != null) newEnemy.target = target;
            }

            yield return new WaitForSeconds(PortalDeactivateDelay);

            spawnPoints[randomIndex].GetChild(0).gameObject.SetActive(false);

            float waitTime = SpawnInterval - PortalToSpawnDelay - PortalDeactivateDelay;
            yield return new WaitForSeconds(waitTime > 0f ? waitTime : PortalToSpawnDelay + PortalDeactivateDelay);
        }
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

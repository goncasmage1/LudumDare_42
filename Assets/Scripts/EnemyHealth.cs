using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health {

    public Transform rockPrefab;
    public Transform poisonPrefab;
    
    public Transform GridHolder;

    private Vector3[] raycastLocations = { new Vector3(0f, 0f, 0f),
                                           new Vector3(1f, 0f, 1f),
                                           new Vector3(1f, 0f, 0f),
                                           new Vector3(1f, 0f, -1f),
                                           new Vector3(0f, 0f, -1f),
                                           new Vector3(-1f, 0f, -1f),
                                           new Vector3(-1f, 0f, 0f),
                                           new Vector3(-1f, 0f, 1f),
                                           new Vector3(0f, 0f, 1f) };

    [HideInInspector]
    public AISpawner spawner;

    [SerializeField]
    private LayerMask raycastMask;

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null) Debug.LogError("Couldn't find Enemy component!");
    }

    public override void ReceiveDamage(float DamageAmount)
    {
        base.ReceiveDamage(DamageAmount);

        if (HealthAmount > 0f)
        {
            Debug.Log("Flinching!");
            enemy.Flinch();
            Transform particles = Instantiate(enemy.HasConsumedPlant() ? spawner.enemyStrongPainFX : spawner.enemyPainFX, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 0.3f);
        }

        transform.Find("Canvas/HP/ImageFiller").GetComponent<Image>().fillAmount = HealthAmount / MaxHealth;
    }

    public override void Die()
    {
        if (spawner != null) spawner.EnemyDied(enemy);

        if (enemy.targetPlant != null) enemy.targetPlant.StoppedEating();
        enemy.Die();

        Vector3 clampedLocation = FindDropLocation();
        if (clampedLocation.y != 5f)
        {
            if (enemy.HasConsumedPlant()) SpawnRockAtLocation(clampedLocation);
            else SpawnPoisonAtLocation(clampedLocation);
        }

        Transform particles = Instantiate(enemy.HasConsumedPlant() ? spawner.enemyStrongDeathFX : spawner.enemyDeathFX, transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 1f);

        Destroy(gameObject);
    }

    Vector3 FindDropLocation()
    {
        Vector3 enemyPosition = transform.position;

        Vector3 clampedLocation = new Vector3(Mathf.Round(enemyPosition.x), 0f, Mathf.Round(enemyPosition.z));
        foreach (Vector3 pos in raycastLocations) {
            RaycastHit Hit;
            Vector3 origin = clampedLocation + pos;
            origin.y = 2f;
            if (Physics.Raycast(origin, Vector3.down, out Hit, 1f, raycastMask))
            {
                if (Hit.collider.GetComponent<RockCell>() != null || Hit.collider.GetComponent<PoisonCell>() != null) continue;

                 PlantCell plant = Hit.collider.GetComponent<PlantCell>();
                if (plant != null && !plant.IsPlantGrown()) Destroy(Hit.collider.gameObject);
            }
            return clampedLocation + pos;
        }
        return new Vector3(0f, 5f, 0f);
    }

    void SpawnRockAtLocation(Vector3 location)
    {
        Transform t = GridHolder.Find("Cell" + location.x + "|" + location.z);
        Instantiate(rockPrefab, t);
        t.GetComponent<GridCell>().assignChildTransform(t);

    }

    void SpawnPoisonAtLocation(Vector3 location)
    {
        Transform t = GridHolder.Find("Cell" + location.x + "|" + location.z);
        Instantiate(poisonPrefab, t);
        t.GetComponent<GridCell>().assignChildTransform(t);
    }
  
}

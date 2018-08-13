using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health {

    public Transform rockPrefab;
    public Transform poisonPrefab;
    public Transform GridHolder;

    [HideInInspector]
    public AISpawner spawner;

    [SerializeField]
    private LayerMask raycastMask;

    public override void ReceiveDamage(float DamageAmount)
    {
        base.ReceiveDamage(DamageAmount);

        GetComponent<Enemy>().Flinch();

        transform.Find("Canvas/HP/ImageFiller").GetComponent<Image>().fillAmount = HealthAmount / MaxHealth;
    }

    public override void Die()
    {
        if (spawner != null) spawner.EnemyDied(GetComponent<Enemy>());
        Enemy enemy = GetComponent<Enemy>();
        if (enemy == null) Debug.LogError("Couldn't find enemy script!");

        Vector3 enemyPosition = transform.position;

        Vector3 clampedLocation = new Vector3(Mathf.Round(enemyPosition.x), 0f, Mathf.Round(enemyPosition.z));
        Debug.Log(clampedLocation);
        RaycastHit Hit;
        Vector3 origin = clampedLocation;
        origin.y = 2f;
        if (Physics.Raycast(origin, Vector3.down, out Hit, 1f, raycastMask))
        {
            Debug.Log("Found object!");
            if (Hit.collider.GetComponent<RockCell>() != null && enemy.HasConsumedPlant() ||
                Hit.collider.GetComponent<PoisonCell>() != null && !enemy.HasConsumedPlant())
            {
                bool pickX = Mathf.Abs(enemyPosition.x - clampedLocation.x) > Mathf.Abs(enemyPosition.z - clampedLocation.z);

                if (pickX) clampedLocation.x += (clampedLocation.x - enemyPosition.x <= 0) ? 1f : -1f;
                else clampedLocation.z += (clampedLocation.z - enemyPosition.z <= 0) ? 1f : -1f;
                origin = clampedLocation;
                origin.y = 2f;
            }

            if (Hit.collider.GetComponent<PlantCell>() != null) Destroy(Hit.collider.gameObject);

            //TODO: Could there be two occupied spots?
            if (enemy.HasConsumedPlant()) SpawnRockAtLocation(clampedLocation);
            else SpawnPoisonAtLocation(clampedLocation);
        }
        else
        {
            if (enemy.HasConsumedPlant()) SpawnRockAtLocation(clampedLocation);
            else SpawnPoisonAtLocation(clampedLocation);
        }
        enemy.Die();
        Destroy(gameObject);
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

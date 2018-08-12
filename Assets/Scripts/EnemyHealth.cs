using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health {

    public Transform rockPrefab;
    public Transform poisonPrefab;

    [HideInInspector]
    public AISpawner spawner;

    [SerializeField]
    private LayerMask raycastMask;

    public override void ReceiveDamage(float DamageAmount)
    {
        base.ReceiveDamage(DamageAmount);

        GetComponent<Enemy>().Flinch();

        transform.Find("Canvas/HP/ImageFiller").GetComponent<Image>().fillAmount = HealthAmount / MaxHealth;
        Debug.Log("Damage!");
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
        if (Physics.Raycast(clampedLocation, -Vector3.up, out Hit, 1f, raycastMask))
        {
            Debug.Log("Found object!");
            if (Hit.collider.GetComponent<RockCell>() && enemy.HasConsumedPlant() ||
                Hit.collider.GetComponent<PoisonCell>() && !enemy.HasConsumedPlant())
            {
                bool pickX = Mathf.Abs(enemyPosition.x - clampedLocation.x) > Mathf.Abs(enemyPosition.z - clampedLocation.z);

                if (pickX) clampedLocation.x += (clampedLocation.x - enemyPosition.x <= 0) ? 1f : -1f;
                else clampedLocation.z += (clampedLocation.z - enemyPosition.z <= 0) ? 1f : -1f;
            }

            //TODO: Could there be two occupied spots?
            if (enemy.HasConsumedPlant()) SpawnRockAtLocation(clampedLocation);
            else SpawnPoisonAtLocation(clampedLocation);
        }
        else
        {
            if (enemy.HasConsumedPlant()) SpawnRockAtLocation(clampedLocation);
            else SpawnPoisonAtLocation(clampedLocation);
        }
        Destroy(gameObject);
    }

    void SpawnRockAtLocation(Vector3 location)
    {
        Instantiate(rockPrefab, location, Quaternion.identity);
    }

    void SpawnPoisonAtLocation(Vector3 location)
    {
        Instantiate(poisonPrefab, location, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlantCell : MonoBehaviour {

    private bool bGrown = false;

    public float PlantRegenAmount = 40f;
    public float PlantGrowthTime = 10f;
    public float PlantToTowerTime = 20f;

    private Transform plant = null;
    private Transform plantReady = null;
    private CapsuleCollider capsule;

    public Transform towerPrefab;

    public bool IsPlantGrown() { return bGrown; }

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        capsule.isTrigger = true;

        plant = gameObject.transform.GetChild(0);
        plant.gameObject.SetActive(true);

        plantReady = gameObject.transform.GetChild(1);
        plantReady.gameObject.SetActive(false);
    }

    private void Start()
    {
        Invoke("GrowPlant", PlantGrowthTime);
    }

    public void GrowPlant()
    {
        bGrown = true;
        plant.gameObject.SetActive(false);
        plantReady.gameObject.SetActive(true);
        Invoke("SpawnTower", PlantToTowerTime);
    }

    private void SpawnTower()
    {
        if (towerPrefab == null) Debug.LogError("Null tower prefab!");

        Instantiate(towerPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant()) return;

        enemy.ConsumePlant();
        Destroy(gameObject);
    }
}

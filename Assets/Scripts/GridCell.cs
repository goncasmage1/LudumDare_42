using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell : MonoBehaviour {

    enum PlantStage { None, Growing, Ready};

    private bool block = false;
    private bool highlighted = false;
    private bool isTower = false;

    public float PlantRegenAmount = 40f;
    public float PlantGrowthTime = 10f;
    public float PlantToTowerTime = 20f;
    public float TowerAttackDistance = 6f;
    public float TowerAttackInterval = 3f;
    public float TowerDamage = 50f;

    private PlantStage plantStage = PlantStage.None;

    private BoxCollider boxCollider = null;
    private Transform floor = null;
    private Transform highlight = null;
    private Transform plant = null;
    private Transform plantReady = null;
    private Transform tower = null;

    private Enemy enemyTarget = null;

    private AISpawner spawner;

    private void Awake () {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        spawner = FindObjectOfType<AISpawner>();
        if (spawner == null) Debug.LogError("Couldn't find AISpawner!");

        floor = gameObject.transform.GetChild(0);

        highlight = gameObject.transform.GetChild(1);
        highlight.gameObject.SetActive(false);

        plant = gameObject.transform.GetChild(2);
        plant.gameObject.SetActive(false);

        plantReady = gameObject.transform.GetChild(3);
        plantReady.gameObject.SetActive(false);

        tower = gameObject.transform.GetChild(4);
        tower.gameObject.SetActive(false);
    }

    private void Start()
    {
        //PutPlant();
    }

    private void Update()
    {
        if (!isTower) return;

        Enemy closestTarget = null;
        float closestDistance = 0f;

        foreach (Enemy enemy in spawner.enemies)
        {
            float distance = (enemy.transform.position - transform.position).magnitude;
            if (distance <= TowerAttackDistance)
            {
                if (closestDistance == 0f || distance < closestDistance)
                {
                    closestTarget = enemy;
                    closestDistance = distance;
                }
            }
        }

        //If found a target
        if (closestTarget != null)
        {
            //and enemy wasn't set, start attacking
            if (enemyTarget == null) Invoke("AttackTarget", TowerAttackInterval);

            if (enemyTarget != closestTarget) enemyTarget = closestTarget;
        }
        //If didn't find a target
        else
        {
            //and enemy is still set, stop attacking
            if (enemyTarget != null)
            {
                enemyTarget = null;
                CancelInvoke("AttackTarget");
            }
        }
    }

    public void Block()
    {
        boxCollider.enabled = true;
        block = true;
    }

    public void UnBlock()
    {
        boxCollider.enabled = true;
        block = false;
    }

    public bool IsBlocking() { return block; }

    public void Highlight()
    {
        highlight.gameObject.SetActive(true);
        highlighted = true;
    }

    public void Unhighlight()
    {
        highlight.gameObject.SetActive(false);
        highlighted = false;
    }

    public bool IsHighlighted() { return highlighted; }

    private void ShowPlant()
    {
        plantStage = PlantStage.Growing;
        plant.gameObject.SetActive(true);
        floor.gameObject.SetActive(false);
    }

    private void ShowGrowedPlant()
    {
        plantStage = PlantStage.Ready;
        plant.gameObject.SetActive(false);
        plantReady.gameObject.SetActive(true);
        boxCollider.enabled = true;
        boxCollider.isTrigger = true;
    }

    private void HidePlant()
    {
        plantStage = PlantStage.None;
        plant.gameObject.SetActive(false);
        plantReady.gameObject.SetActive(false);
        boxCollider.enabled = false;
        boxCollider.isTrigger = false;
    }

    private void ShowFloor()
    {
        floor.gameObject.SetActive(true);
    }

    private void ShowTower()
    {
        tower.gameObject.SetActive(true);
        isTower = true;
    }

    private void HideTower()
    {
        tower.gameObject.SetActive(false);
        isTower = false;
    }

    public bool IsTower() { return isTower; }

    public void PutPlant()
    {
        if (plantStage != PlantStage.None || block) return;

        Unhighlight();
        ShowPlant();

        Invoke("GrowPlant", PlantGrowthTime);
    }

    public void GrowPlant()
    {
        ShowGrowedPlant();
        Invoke("GrowIntoTower", PlantToTowerTime);
    }

    public bool HarvestPlant()
    {
        if (plantStage != PlantStage.Ready) return false;

        HidePlant();
        ShowFloor();
        return true;
    }

    private void GrowIntoTower()
    {
        if (plantStage != PlantStage.Ready) return;

        HidePlant();
        ShowTower();
        Block();
    }

    void AttackTarget()
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<Health>().ReceiveDamage(TowerDamage);
            Invoke("AttackTarget", TowerAttackInterval);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant()) return;

        enemy.ConsumePlant();
        HarvestPlant();
    }
}

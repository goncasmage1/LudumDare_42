using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlantCell : MonoBehaviour {

    enum PlantStage { Initial, Grown, Tower};

    PlantStage plantStage = PlantStage.Initial;

    public float PlantRegenAmount = 40f;
    public float PlantGrowthTime = 10f;
    public float PlantToTowerTime = 20f;

    public float TowerAttackDistance = 6f;
    public float TowerAttackInterval = 3f;
    public float TowerDamage = 50f;

    private CapsuleCollider capsule;

    private Enemy enemyTarget = null;
    private AISpawner spawner;

    private Animator anim;

    private List<Collider> consumers = new List<Collider>();

    public bool IsPlantGrown() { return plantStage == PlantStage.Grown; }

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        capsule.isTrigger = true;

        spawner = FindObjectOfType<AISpawner>();
        if (spawner == null) Debug.LogError("Couldn't find AISpawner!");

        anim = GetComponentInChildren<Animator>();
        if (anim == null) Debug.LogError("Couldn't find Animator in plant!");
    }

    private void Start()
    {
        Invoke("GrowPlant", PlantGrowthTime);
    }

    private void Update()
    {
        if (!(plantStage == PlantStage.Tower)) return;

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
            if (enemyTarget == null)
            {
                CancelInvoke("AttackTarget");
                Invoke("AttackTarget", TowerAttackInterval);
            }

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

    void AttackTarget()
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<Health>().ReceiveDamage(TowerDamage);
            Invoke("AttackTarget", TowerAttackInterval);
        }
    }

    void OnDestroy()
    {
        //transform.parent.GetComponent<GridCell>().setHasPlantRipe(false);
    }
    public void GrowPlant()
    {
        plantStage = PlantStage.Grown;
        //transform.parent.GetComponent<GridCell>().setHasPlantRipe(true);
        anim.SetBool("Ready", true);
        if (consumers.Count > 0)
        {
            consumers[0].GetComponentInParent<Enemy>().ConsumePlant();
            Destroy(gameObject, consumers[0].GetComponentInParent<Enemy>().PlantConsumptionTime);
            return;
        }
        Invoke("BecomeTower", PlantToTowerTime);
    }

    private void BecomeTower()
    {
        plantStage = PlantStage.Tower;
        capsule.isTrigger = false;
        anim.SetBool("Tower", true);
    }
    public bool isRipe()
    {
        return plantStage == PlantStage.Grown;
        
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant()) return;

        if (plantStage == PlantStage.Grown)
        {
            enemy.ConsumePlant();
            Destroy(gameObject, 2f / 3f);
        }
        else
        {
            consumers.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant()) return;

        consumers.Remove(other);
    }
}

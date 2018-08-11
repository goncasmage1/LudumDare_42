using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCell : MonoBehaviour {

    public float TowerAttackDistance = 6f;
    public float TowerAttackInterval = 3f;
    public float TowerDamage = 50f;

    private Enemy enemyTarget = null;
    private AISpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<AISpawner>();
        if (spawner == null) Debug.LogError("Couldn't find AISpawner!");
    }

    void Start () {
		
	}
	
	void Update () {
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

    void AttackTarget()
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<Health>().ReceiveDamage(TowerDamage);
            Invoke("AttackTarget", TowerAttackInterval);
        }
    }
}

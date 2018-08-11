using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {

    [HideInInspector]
    public AISpawner spawner;

    public override void Die()
    {
        if (spawner != null) spawner.EnemyDied(GetComponent<Enemy>());
        Destroy(gameObject);
    }
}

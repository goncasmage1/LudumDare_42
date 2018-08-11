using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health {

    [HideInInspector]
    public AISpawner spawner;

    public override void ReceiveDamage(float DamageAmount)
    {
        base.ReceiveDamage(DamageAmount);

        transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = HealthAmount / MaxHealth;
        Debug.Log("Damage!");
    }

    public override void Die()
    {
        if (spawner != null) spawner.EnemyDied(GetComponent<Enemy>());
        Destroy(gameObject);
    }
}

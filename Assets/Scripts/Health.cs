using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float MaxHealth = 100f;
    private float HealthAmount = 0f;

    private void Awake()
    {
        HealthAmount = MaxHealth;
    }

    public float GetHealth()
    {
        return HealthAmount;
    }

    public void ReceiveDamage(float DamageAmount)
    {
        HealthAmount -= DamageAmount;
        if (HealthAmount <= 0f) Die();
    }

    public void Regenerate(float RegenAmount)
    {
        HealthAmount += RegenAmount;
        if (HealthAmount > MaxHealth) HealthAmount = MaxHealth;
    }

    public virtual void Die()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float MaxHealth = 100f;
    public float maxHealthBig = 150f;
    protected float HealthAmount = 0f;

    private void Start()
    {
        HealthAmount = MaxHealth;
    }

    public float GetHealth()
    {
        return HealthAmount;
    }

    virtual public void ReceiveDamage(float DamageAmount)
    {
        Debug.Log("levei dano " + transform.name + " " + DamageAmount);
        HealthAmount -= DamageAmount;
        if (HealthAmount <= 0f) Die();
    }

    virtual public void Regenerate(float RegenAmount)
    {
        HealthAmount += RegenAmount;
        if (HealthAmount > MaxHealth) HealthAmount = MaxHealth;
    }

    public virtual void Die()
    {
        Vector3 pos = transform.position;
    }
    public  void GrowBigger()
    {
        MaxHealth = maxHealthBig;
        HealthAmount = MaxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PoisonCell : MonoBehaviour {

    public float Damage = 10f;
    public float DamageInterval = 0.5f;
    public float Lifespan = 10f;

    private Health victim;

    private void Awake()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
    }

    void Start () {
        Destroy(gameObject, Lifespan);
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.GetComponent<PlayerScript>();
        if (player == null) return;

        victim = player.GetComponent<Health>();
        DoDamage();
    }

    void OnTriggerExit(Collider other)
    {
        PlayerScript player = other.GetComponent<PlayerScript>();
        if (player == null) return;

        victim = null;
        CancelInvoke("DoDamage");
    }

    void DoDamage()
    {
        if (victim == null) return;

        victim.ReceiveDamage(Damage);
        Invoke("DoDamage", DamageInterval);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PoisonCell : MonoBehaviour {

    public float Damage = 10f;
    public float DamageInterval = 1.1f;
    public float Lifespan = 10f;

    private PlayerScript victim;

    private void Awake()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
    }

    void Start () {
        Destroy(gameObject, Lifespan);
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.transform.parent.parent.parent.gameObject.GetComponent<PlayerScript>();
        if (player == null) return;

        Debug.Log("Poisoning!");
        victim = player;
        DoDamage();
    }

    void OnTriggerExit(Collider other)
    {
        PlayerScript player = other.transform.parent.parent.parent.gameObject.GetComponent<PlayerScript>();
        if (player == null) return;

        victim = null;
        CancelInvoke("DoDamage");
    }

    void DoDamage()
    {
        if (victim == null) return;

        victim.takeDamage(Damage);
        Invoke("DoDamage", DamageInterval);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    private bool bConsumedPlant = false;
    private bool bTargetInRange = false;
    private bool isFlinching = false;

    public float Speed = 2f;
    public float Damage = 20f;
    public float AttackDistance = 0.4f;
    public float AttackInitialDelay = 0.2f;
    public float AttackInterval = 0.6f;
    public float flinchTime = 0.5f;

    private Transform aimRotation;
    public Transform target = null;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aimRotation = transform.Find("AimRotation").GetComponent<Transform>();
    }
	
	void Update () {
        if (target == null || isFlinching) return;

        aimRotation.LookAt(target);
        Quaternion clampedRotation = aimRotation.rotation;
        clampedRotation.x = 0;
        clampedRotation.z = 0;
        aimRotation.rotation = clampedRotation;

        Vector3 position = transform.position;
        if ((position - target.position).magnitude <= AttackDistance)
        {
            if (!bTargetInRange)
            {
                bTargetInRange = true;
                Invoke("AttackTarget", AttackInitialDelay);
            }
        }
        else
        {
            if (bTargetInRange)
            {
                bTargetInRange = false;
                CancelInvoke("AttackTarget");
            }
            rb.MovePosition(position + aimRotation.forward * Time.deltaTime);
        }

    }

    public bool HasConsumedPlant() { return bConsumedPlant; }

    public void ConsumePlant()
    {
        bConsumedPlant = true;
    }

    void AttackTarget()
    {
        if (target == null) return;
        target.GetComponent<PlayerScript>().takeDamage(Damage);
        Invoke("AttackTarget", AttackInterval);
        Debug.Log("Damaged player!");
    }

    public void Flinch()
    {
        isFlinching = true;
        Invoke("RecoverFromFlinch", flinchTime);
    }

    void RecoverFromFlinch()
    {
        isFlinching = false;
    }
}

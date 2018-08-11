using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    private bool bConsumedPlant = false;
    public float Speed = 2f;
    private Transform aimRotation;
    public Transform target = null;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aimRotation = transform.Find("AimRotation").GetComponent<Transform>();
    }
	
	void Update () {
        if (target == null) return;

        aimRotation.LookAt(target);
        Quaternion clampedRotation = aimRotation.rotation;
        clampedRotation.x = 0;
        clampedRotation.z = 0;
        aimRotation.rotation = clampedRotation;

        rb.MovePosition(transform.position + aimRotation.forward * Time.deltaTime);
    }

    public bool HasConsumedPlant() { return bConsumedPlant; }

    public void ConsumePlant()
    {
        bConsumedPlant = true;
    }
}

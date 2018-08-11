using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    public float Speed = 2f;

    public Transform target = null;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start () {
		
	}
	
	void Update () {
        if (target == null) return;

        transform.LookAt(target);
        Quaternion clampedRotation = transform.rotation;
        clampedRotation.x = 0;
        clampedRotation.z = 0;
        transform.rotation = clampedRotation;

        rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
    }
}

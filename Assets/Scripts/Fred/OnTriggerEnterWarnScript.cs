using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterWarnScript : MonoBehaviour {
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.transform.parent.parent.parent.GetComponent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.ReceiveDamage(30f);

       
    }
 
}

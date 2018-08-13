using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterWarnScript : MonoBehaviour {
    // Use this for initialization
    public float damage = 30f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyHealth enemyHealth = other.transform.parent.parent.parent.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.ReceiveDamage(damage);
        }

       
    }
 
}

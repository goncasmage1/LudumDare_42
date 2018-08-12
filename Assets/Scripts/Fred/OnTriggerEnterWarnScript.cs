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
        other.transform.parent.parent.parent.GetComponent<EnemyHealth>().ReceiveDamage(30f);

       
    }
 
}

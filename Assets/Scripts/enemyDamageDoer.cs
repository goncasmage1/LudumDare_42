using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamageDoer : MonoBehaviour {

    Enemy en;
	// Use this for initialization
	void Start () {
        en = transform.parent.parent.parent.parent.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DoDamage()
    {
        en.DoDamage();
    }
}

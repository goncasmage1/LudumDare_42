using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCanvasToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

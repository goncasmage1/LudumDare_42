using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerPlayerCell : MonoBehaviour {
    GridCell currentCell;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (currentCell != null)
        {
            currentCell.removeTargeted();
        }
        currentCell = other.GetComponent<GridCell>();
        currentCell.getTargeted();
    }
}

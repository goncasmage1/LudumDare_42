using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGridCellFilled : MonoBehaviour {
    public Transform gc;
	// Use this for initialization
	void Start () {
        Invoke("setGridCell", 1f);

		
	}
	void setGridCell()
    {
        Debug.Log(transform.name);
        Transform t = gc.Find("Cell" + Mathf.Floor(transform.position.x) + "|" + Mathf.Floor(transform.position.z));
        t.GetComponent<GridCell>().assignChildTransform(transform);
    }
	// Update is called once per frame
	void Update () {
		
	}
}

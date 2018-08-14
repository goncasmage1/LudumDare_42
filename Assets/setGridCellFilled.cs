using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGridCellFilled : MonoBehaviour {
    public Transform gc;

	void Start () {
        Invoke("setGridCell", 1f);

		
	}
	void setGridCell()
    {
        Transform t = gc.Find("Cell" + Mathf.Floor(transform.position.x) + "|" + Mathf.Floor(transform.position.z));
        t.GetComponent<GridCell>().assignChildTransform(transform);
    }

	void Update () {
		
	}
}

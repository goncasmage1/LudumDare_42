using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GridCell : MonoBehaviour {

    [HideInInspector]
    public bool isBlocking = false;

    private BoxCollider2D boxCollider;

    void Awake () {
        boxCollider = GetComponent<BoxCollider2D>();
    }
	
    public void Block()
    {
        //Block collision
    }

    public void UnBlock()
    {
        //Unblock collision
    }
}

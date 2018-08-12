using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Weapon,
    Pickaxe,
    Plant
}
public class OnTriggerPlayerCell : MonoBehaviour {
    GridCell currentCell;
    bool isToShow = false;
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
        if (isToShow)
        {
            currentCell.getTargeted();
        }
    }
    public void showTargetting()
    {
        isToShow = true;
        if (currentCell != null)
        {
            currentCell.getTargeted();
        }
    }
    public void hideTargetting()
    {
        isToShow = false;
        if (currentCell != null)
        {
            currentCell.removeTargeted();
        }
    }
}

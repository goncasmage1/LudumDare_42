using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell : MonoBehaviour {
    MeshRenderer mr;
    public Material materialGreen;
    public Material materialRed;
    private void Awake () {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();

    }
    public void getTargeted()
    {
        mr.material = materialGreen;
        mr.enabled = true;
        
    }
    public void removeTargeted()
    {
        mr.enabled = false;
    }
}

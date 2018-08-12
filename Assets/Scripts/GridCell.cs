using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell : MonoBehaviour {
    MeshRenderer mr;
    public Material materialGreen;
    public Material materialRed;
    Transform childTransform;

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
    public void assignChildTransform(Transform transf)
    {
        childTransform = transf;
    }
    public void removeChildTransform(Transform transf)
    {
        childTransform = null;
    }
    public bool hasChildTransform()
    {
        return childTransform;
    }
}

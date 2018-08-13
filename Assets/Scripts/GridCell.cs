using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell : MonoBehaviour {
    MeshRenderer mr;
    public Material materialGreen;
    public Material materialRed;
    Transform childTransform;
    public bool hasRipePlant = false;
    public GameObject arrow;

    private void Awake () {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();

    }
    public void getTargeted()
    {
        if (childTransform == null)
        {
            mr.material = materialGreen;
        }
        else
        {
            mr.material = materialRed;
        }
        if (hasRipePlant)
        {
            arrow.SetActive(true);
        }
        else {
            mr.enabled = true;
        }

    }
    public void setHasPlantRipe(bool isRipe)
    {
        hasRipePlant = isRipe;
        if (mr.enabled || arrow.activeSelf)
        {
            if (hasRipePlant)
            {
                arrow.SetActive(true);
                mr.enabled = false;
            }
            else
            {
                mr.enabled = true;
                arrow.SetActive(false);
            }
        }
    }
    public void removeTargeted()
    {
        arrow.SetActive(false);
        mr.enabled = false;

    }
    public void assignChildTransform(Transform transf)
    {
        mr.material = materialRed;
        childTransform = transf;
    }
    public void removeChildTransform(Transform transf)
    {
        mr.material = materialGreen;
        childTransform = null;
    }
    public bool hasChildTransform()
    {
        return childTransform;
    }
    public Transform getChildTransform()
    {
        return childTransform;
    }
}

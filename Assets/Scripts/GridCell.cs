using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell : MonoBehaviour {
    MeshRenderer mr;
    public Material materialGreen;
    public Material materialRed;
    public Transform childTransform;
    public bool hasRipePlant = false;
    public bool isBlockedToPlants = false;
    public bool hasTower = false;
    public GameObject arrow;

    private void Awake () {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();

    }
    public void getTargeted()
    {
        if (childTransform == null && !isBlockedToPlants)
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
    public void setHasTower(bool hT)
    {
        hasTower = hT;
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
  
    public void blockForPlants()
    {
        isBlockedToPlants = true;
    }
    public void removeChildTransform(Transform transf)
    {
        mr.material = materialGreen;
        childTransform = null;
    }
    public bool hasChildTransform(bool isForPlant)
    {
        if (isForPlant)
        {
            return childTransform != null || isBlockedToPlants;
        }
        else
        {
            return childTransform != null;
        }
    }
    public Transform getChildTransform()
    {
        return childTransform;
    }
   
}

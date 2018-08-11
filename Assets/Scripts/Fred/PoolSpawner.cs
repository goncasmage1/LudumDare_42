using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolSpawner : MonoBehaviour {
	private int maxSpawns;
	private GameObject[] allSpawns;
	private int nextChild=0;

    [SerializeField] bool setRandomPos;

	// Use this for initialization
	void Start () {
		
	
		maxSpawns=transform.childCount;

	}
    public GameObject Spawn(Transform targetParent,float Yoffset)
    {
        GameObject targetObj = null;
        if (transform.childCount <= 0)
            Debug.Log("We need more Items pooled");
        else
        {
            targetObj = transform.GetChild(0).gameObject;
            targetObj.transform.parent = targetParent;
            if (setRandomPos)
            {
                targetObj.transform.localPosition = new Vector3(Random.Range(-0.15f, 0.15f), 0, Random.Range(-0.15f, 0.15f));
                targetObj.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
            }
            else
            {
                targetObj.transform.localPosition =new Vector3(0, Yoffset,0);
                targetObj.transform.localRotation = Quaternion.identity;
            }
            targetObj.SetActive(true);
            nextChild++;
            if (nextChild >= maxSpawns)
                nextChild = 0;
        }
        return targetObj;
    }
    public GameObject Spawn(Transform targetParent){
        GameObject targetObj = null ;
        if (transform.childCount <= 0)
            Debug.Log("We need more Items pooled");
        else
        {
            targetObj = transform.GetChild(0).gameObject;
            targetObj.transform.parent = targetParent;
            if (setRandomPos)
            {
                targetObj.transform.localPosition = new Vector3(Random.Range(-0.15f, 0.15f), 0, Random.Range(-0.15f, 0.15f));
                targetObj.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
            }
            else
            {
                targetObj.transform.localPosition = Vector3.zero;
                targetObj.transform.localRotation = Quaternion.identity;
            }
            targetObj.SetActive(true);
            nextChild++;
            if (nextChild >= maxSpawns)
                nextChild = 0;
        }
		return targetObj;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

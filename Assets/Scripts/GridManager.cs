using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] Transform cellParent;
    [SerializeField] PoolSpawner cellSpawner;
    private void Awake()
    {
        StartCoroutine("CreateCells");
    }
    IEnumerator CreateCells()
    {
        int size = 30;
        int i, j = 0;
        for (i = -(size / 2);  i <= (size / 2) ;i++)
        {
            for (j = -(size / 2); j <= (size / 2); j++)
            {
                GameObject go= cellSpawner.Spawn(cellParent, new Vector3(i, 0, j));
                go.transform.name = "Cell" + i + "|" + j;
            }
            yield return null;
        }

        yield return null;
    }
    void Start () {
		
	}
}

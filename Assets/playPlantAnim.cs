using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playPlantAnim : MonoBehaviour
{
    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        anim.Play("ANIM_Plant_Shake", -1, 0f);
    }
}

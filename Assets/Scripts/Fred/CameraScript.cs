using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    Transform target;
    [SerializeField] float lerpStrength = 0.05f;
    [SerializeField] float distZToIgnore = 0;
    [SerializeField] float distXToIgnore=0;
    Transform myTransform;
    Vector3 offset;
	// Use this for initialization
	void Awake () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myTransform = transform;
        offset = new Vector3(-0.6f, 0.0f, 0.39f);

    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 myOffsettedPosition = myTransform.position - offset;
        Vector3 distanceVect = myOffsettedPosition - target.position;
        float AbsoluteX = Mathf.Abs(distanceVect.x);
        float AbsoluteZ = Mathf.Abs(distanceVect.z);
        if (AbsoluteX > distXToIgnore || AbsoluteZ > distZToIgnore)
        {
            Vector3 distanceVector = target.position - myOffsettedPosition;
            float vectAbsoluteX = Mathf.Abs(distanceVector.x);
            float vectAbsoluteZ = Mathf.Abs(distanceVector.z);
            if (vectAbsoluteX > distXToIgnore)
            {
                distanceVector.x = distXToIgnore * Mathf.Sign(distanceVector.x);
            }
            if (vectAbsoluteZ > distZToIgnore)
            {
                distanceVector.z = distZToIgnore * Mathf.Sign(distanceVector.z);
            }
            Debug.Log(myOffsettedPosition + " || " + distanceVector + " || " + target.position);
            myTransform.position = Vector3.Lerp(myOffsettedPosition  + offset, target.position + offset-distanceVector, lerpStrength);

        }
    }
}

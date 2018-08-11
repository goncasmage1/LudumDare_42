using UnityEngine;
using System.Collections;

public class targetting : MonoBehaviour {
	Transform myTransform;

	void Start () {
		myTransform=transform;
	}

	void Update () {

	}
	void calcZ(Vector2 aimDir){
		float valZ;

		Vector3 valHip=aimDir;
		// calcula o angulo que queres sempre no quadrante de cima a direita (devido aos math.abs) (isto porque so sacas angulos de 0 a 90 e portanto tens de escolher um quadrante qualquer)
		valZ=Mathf.Atan (Mathf.Abs (valHip.x)/Mathf.Abs(valHip.y));
		valZ*=Mathf.Rad2Deg;
		// Passa desse quadrante para o que for devido 
		if (valHip.y>0)
		if (valHip.x>0)
			valZ=180-valZ;
		else
			valZ=180+valZ;
		else
			if (valHip.x<0)
				valZ=360-valZ;

		//E pronto esta aqui o valor do Z que uso na rotacao do meu targetting.
		Debug.Log (valZ);

	}
}

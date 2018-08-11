﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    private Transform myTransform;
    private generalInput gInput;
    private Rigidbody rb;
    private float rotationZ;
    private Transform canvasRotTransf;
    private Transform regularRotTransf;
    public int SpellCharges = 0;
    public int weaponSpellHeld = -1;
    [SerializeField] private Animator anim;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashTime = 1.5f;
    [SerializeField] private float dashMultiplier = 3f;
    [SerializeField] Transform itemSpot;
    [SerializeField] Transform ItemHolder;
    [SerializeField] GameObject HasSpellParticles;





    public int itemHeld;
    private GameObject GameObjectHeld;
    private Image hpBarImg;
    private float currWalkSpeed;
    private float startFireTarget = -1f;
    private Image aimTargetImage;
    private Vector2 lastAimDir;
    private Vector2 lastMoveDir;
    private PoolSpawner regularGrenadePS;
    private PoolSpawner pullGrenadePS;
    private PoolSpawner pushGrenadePS;
    private PoolSpawner solidGrenadePS;
    private SpriteRenderer sr;
    private int health = 15;
    private float maxHP = 50;
    private float HP = 50;
    private bool inputDisabled = false;
    private float startDash = -1;
    [SerializeField] bool AmICraft;



    private int CountBombs = 4;


    // Use this for initialization
    void Start() {

    }
    void OnEnable() {
        lastAimDir = new Vector2(1, 0);
        HP = maxHP;
        myTransform = transform;
        //anim=gameObject.GetComponent<Animator>();
        gInput = gameObject.GetComponent<generalInput>();
        rb = gameObject.GetComponent<Rigidbody>();
        //sr=myTransform.Find("SpriteHolder/Body").GetComponent<SpriteRenderer>();
        regularRotTransf = myTransform.Find("AimRotation");

        canvasRotTransf = myTransform.Find("Canvas/AimRotation");
        if (anim != null)
        {
            anim.SetBool("HasItem", false);
            anim.SetBool("HasSpell", false);
        }
        currWalkSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (gInput.GetInput())
        {
            if (!inputDisabled) {
                calcWalkSpeed();
                Vector2 moveDirXY = gInput.getMovementInput();

                Vector3 moveDir = new Vector3(moveDirXY.x, 0, moveDirXY.y);

                if (rb.velocity.magnitude < maxSpeed) {
                    rb.AddForce(moveDir * currWalkSpeed);
                }

                //anim.SetFloat("MoveSpeed",moveDir.magnitude*(currWalkSpeed/walkSpeed));
                Vector2 aimDir = gInput.getAimInput();
                if (moveDir != Vector3.zero) {
                    lastMoveDir = moveDir;
                }
                if (aimDir != Vector2.zero) {
                    lastAimDir = aimDir;
                }
                Debug.Log(moveDirXY);
                rotationZ = calcZ(moveDirXY);
                canvasRotTransf.rotation=Quaternion.Euler(0,0,rotationZ);
                regularRotTransf.rotation = Quaternion.Euler(-90, rotationZ, 0);
                if (gInput.getFireDown()) {
                    
                    }
               

              
				if (gInput.getDashDown()){
					inputDisabled=true;
					currWalkSpeed=walkSpeed*dashMultiplier;
					//anim.SetBool("Dashing",true);
					Invoke("endDash",dashTime);
					startDash=Time.time;
				}
				
			}else{
				startFireTarget=-1;
				if (startDash!=-1){
					if (rb.velocity.magnitude<maxSpeed*dashMultiplier){
						rb.AddForce(lastMoveDir*currWalkSpeed);
					}
				}

			}
        }
		aimTarget();
		setHPBar();

	}

    

    void UsePot( )
    {
        

    }
  
	
	void endDash(){
		if (inputDisabled && startDash!=-1){

			inputDisabled=false;
			//anim.SetBool("Dashing",false);

			startDash=-1;

		}
	}

	void setHPBar(){
		//hpBarImg.fillAmount=HP/maxHP;
	}
	void calcWalkSpeed(){
		
			currWalkSpeed=walkSpeed;
		
	}
	void Fire(Vector2 direction){
		


	}

	void aimTarget(){
	/*	if (startFireTarget!=-1){
			if (!aimTargetImage.gameObject.activeSelf){
				aimTargetImage.gameObject.SetActive(true);
			}
			aimTargetImage.fillAmount=Mathf.Clamp((Time.time-startFireTarget)/maxFireTarget,0,1);
		}else{
			if (aimTargetImage.gameObject.activeSelf){
				aimTargetImage.gameObject.SetActive(false);
			}

		}*/
	}

	 void takeDamage(float damage){
		HP-=damage;
		if (HP<=0){
			Die();
		}
	}
    void DestroyMyHeldItem()
    {
        
        itemHeld = 0;
    }
	void Die(){
        inputDisabled = true;
        anim.SetBool("IsDead", true);
	}
	float calcZ(Vector2 aimDir){
		float valZ=rotationZ;
		if (aimDir!=Vector2.zero){
			Vector3 valHip=new Vector3(aimDir.x,aimDir.y,0);
			// calcula o angulo que queres sempre no quadrante de cima a direita (devido aos math.abs) (isto porque so sacas angulos de 0 a 90 e portanto tens de escolher um quadrante qualquer)
			valZ=Mathf.Atan (Mathf.Abs (valHip.x)/Mathf.Abs(valHip.y));
			valZ*=Mathf.Rad2Deg;
			// Passa desse quadrante para o que for devido 
			//Debug.Log (valZ+" antes ");

			if (valHip.y>0)
			{
				if (valHip.x>0)
				{
					valZ=180+valZ;
				}
				else
				{
					valZ=180-valZ;
				}
			}
			else
			{
				if (valHip.x>=0)
					valZ=360-valZ;
				else
					valZ=valZ;
			}

		}
		return valZ;

	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
       

    }

     void OnCollisionEnter(Collision collision)
    {
      
    }

     void getHit(int damage)
    {
        takeDamage(damage);
        Debug.Log("MATARAM-ME");
    }
    
     void setAllChildren(int nr,Transform transf)
    {
        transf.gameObject.layer = nr;
        foreach(Transform childTransf in transf)
        {
            setAllChildren(nr, childTransf);
        }
    }
     void OnTriggerExit(Collider other)
    {
       
    }
}

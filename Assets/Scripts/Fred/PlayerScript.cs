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
    public int weaponSpellHeld = -1;
    [SerializeField] private Animator anim;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashTime = 1.5f;
    [SerializeField] private float dashMultiplier = 3f;
    [SerializeField] Transform itemSpot;
    [SerializeField] Transform ItemHolder;
    [SerializeField] GameObject HasSpellParticles;
    [SerializeField] float singleAttackCD = 0.4f;
    [SerializeField] float singleAttackWaitStart = 0.2f;
    [SerializeField] float lastAttackCD = 0.6f;
    [SerializeField] float timeAttackMove = 0.5f;
    [SerializeField] float speedAttackMultiplier = 3f;
    private int attackNumber = 0;

    public Sprite swordSprite;
    public Sprite pickaxeSprite;
    public Sprite flowerSprite;
    public GameObject PlantObj;
    private Vector3 directionWhileAttacking;


    private GameObject canvas;
    private GameObject deathCanvas;
    private Image hpBarImg;
    private Image itemImage;
    private Text seedsText;

    private float currWalkSpeed;
    private float startFireTarget = -1f;
    private Image aimTargetImage;
    private Vector2 lastAimDir;
    public Vector3 lastMoveDir;
    private PoolSpawner regularGrenadePS;
    private PoolSpawner pullGrenadePS;
    private PoolSpawner pushGrenadePS;
    private PoolSpawner solidGrenadePS;
    private SpriteRenderer sr;
    private float maxHP = 100;
    private float HP = 100;
    private bool inputDisabled = false;
    private float startDash = -1;
    [SerializeField] bool AmICraft;
    private ItemHeldType currItemType = ItemHeldType.Weapon;

    private bool isPlacingPlant;
    private bool isShowingCellTargetting = false;
    private int CountBombs = 4;
    private OnTriggerPlayerCell myCellTargetting;
    private bool isTryingToParry;
    private bool isParrying;
    public float blockAngle = 120;


    // Use this for initialization
    void Start() {
    }
    void GeneralEnable()
    {
        Time.timeScale = 1f;
        lastAimDir = new Vector2(1, 0);
        HP = maxHP;
        myTransform = transform;
        if (anim==null)
             anim = transform.Find("AimRotation/ModelHolder/MODEL_CHAR_Hero").GetComponent<Animator>();
        gInput = gameObject.GetComponent<generalInput>();
        rb = gameObject.GetComponent<Rigidbody>();
        //sr=myTransform.Find("SpriteHolder/Body").GetComponent<SpriteRenderer>();
        regularRotTransf = myTransform.Find("AimRotation");
        if (canvas==null)
            canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas == null) Debug.LogError("Couldn't find Canvas!");
        if (deathCanvas == null)
            deathCanvas = GameObject.FindGameObjectWithTag("DeathCanvas");
        if (deathCanvas == null) Debug.LogError("Couldn't find Death Canvas!");
        if (hpBarImg==null)
            hpBarImg = canvas.transform.GetChild(0).GetComponent<Image>();
        if (hpBarImg == null) Debug.LogError("Couldn't find Health Bar!");
        if (itemImage == null)
            itemImage = canvas.transform.GetChild(1).GetComponent<Image>();
        if (itemImage == null) Debug.LogError("Couldn't find Item Image!");
        if (seedsText == null)
            seedsText = canvas.transform.GetChild(2).GetComponent<Text>();
        if (seedsText == null) Debug.LogError("Couldn't find Seeds Text!");
        if (myCellTargetting==null)
            myCellTargetting = myTransform.Find("AimRotation/CellTargetting").GetComponent<OnTriggerPlayerCell>();
        if (canvasRotTransf==null)
            canvasRotTransf = myTransform.Find("Canvas/AimRotation");
        if (anim != null)
        {
            anim.SetBool("IsGuarding", false);
            anim.SetFloat("Speed", 0);
        }
        currWalkSpeed = walkSpeed;
    }
    void OnEnable() {
        GeneralEnable();
    }

    // Update is called once per frame
    void Update() {
        if (gInput.GetInput())
        {
            if (!inputDisabled) {
                //Debug.Log(isShowingCellTargetting + " || " + isPlacingPlant);
                if (!isShowingCellTargetting && isPlacingPlant)
                {
                    isShowingCellTargetting = true;
                    myCellTargetting.showTargetting();
                }
                if(isTryingToParry && !isParrying)
                {
                    isParrying = true;
                    anim.SetBool("IsGuarding", true);

                }
                calcWalkSpeed();
                Vector2 moveDirXY = gInput.getMovementInput();

                Vector3 moveDir = new Vector3(moveDirXY.x, 0, moveDirXY.y);

                if (rb.velocity.magnitude < maxSpeed && !isParrying) {
                    rb.AddForce(moveDir * currWalkSpeed);
                }

                //anim.SetFloat("MoveSpeed",moveDir.magnitude*(currWalkSpeed/walkSpeed));
                Vector2 aimDir = gInput.getAimInput();
                if (moveDir != Vector3.zero) {
                    anim.SetFloat("Speed", 1f);
                    lastMoveDir = moveDir;
                }
                else
                {
                    anim.SetFloat("Speed", 0f);
                }
                if (aimDir != Vector2.zero) {
                    lastAimDir = aimDir;
                }
                rotationZ = calcZ(moveDirXY);
                canvasRotTransf.rotation = Quaternion.Euler(0, 0, rotationZ);
                regularRotTransf.rotation = Quaternion.Euler(0, rotationZ, 0);
                if (gInput.getFireDown()) {
                    Fire();
                }




                if (gInput.getDashDown()) {
                    inputDisabled = true;
                    currWalkSpeed = walkSpeed * dashMultiplier;
                    //anim.SetBool("Dashing",true);
                    Invoke("endDash", dashTime);
                    startDash = Time.time;
                }

            } else {
                if (attackNumber > 0)
                {
                    float timeSinceFire = Time.time - startFireTarget;
                    Vector2 moveDirXY = gInput.getMovementInput();

                    Vector3 moveDir = new Vector3(moveDirXY.x, 0, moveDirXY.y);
                    if (moveDir != Vector3.zero)
                    {
                        directionWhileAttacking = moveDir;
                    }
                    if (timeSinceFire < timeAttackMove)
                    {
                        if (rb.velocity.magnitude < maxSpeed * speedAttackMultiplier)
                        {
                            rb.AddForce(lastMoveDir * currWalkSpeed);
                        }

                    }
                    if (timeSinceFire < singleAttackCD)
                    {
                        if (timeSinceFire > singleAttackWaitStart)
                        {
                            if (gInput.getFireDown())
                            {
                                Fire();
                            }
                        }
                    }
                    else
                    {
                        attackNumber = 0;
                        timeSinceFire = -1;
                        directionWhileAttacking = Vector3.zero;
                        inputDisabled = false;
                    }
                }
                if (startDash != -1) {
                    if (rb.velocity.magnitude < maxSpeed * dashMultiplier) {
                        rb.AddForce(lastMoveDir * currWalkSpeed);
                    }
                }

            }
            if (gInput.getLeftActionDown())
            {
                currItemType = ItemHeldType.Weapon;
                isShowingCellTargetting = false;
                isPlacingPlant = false;
                myCellTargetting.hideTargetting();
            }
            if (gInput.getRightActionDown())
            {
                currItemType = ItemHeldType.Tool;
                isShowingCellTargetting = false;
                isPlacingPlant = false;
                myCellTargetting.hideTargetting();

            }
            if (gInput.getUpActionDown())
            {
                currItemType = ItemHeldType.Seed;
                isPlacingPlant = true;

                if (!inputDisabled)
                {
                    isShowingCellTargetting = true;
                    myCellTargetting.showTargetting();
                }
            }if (gInput.getParryDown())
            {
                isTryingToParry = true;
                if (!inputDisabled)
                {
                    isParrying = true;
                    anim.SetBool("IsGuarding", true);
                }
            }
            if (gInput.getParryUp())
            {
                isTryingToParry = false;
                isParrying = false;
                anim.SetBool("IsGuarding", false);
                
            }

        }

        aimTarget();
        setHPBar();

    }


    void Use()
    {

    }
    void UsePot()
    {


    }


    void endDash() {
        if (inputDisabled && startDash != -1) {

            inputDisabled = false;
            //anim.SetBool("Dashing",false);

            startDash = -1;

        }
    }

    void setHPBar() {
        hpBarImg.fillAmount = HP / maxHP;
    }
    void calcWalkSpeed() {

        currWalkSpeed = walkSpeed;

    }
    void Fire() {
        if (currItemType == ItemHeldType.Weapon)
        {

            if (attackNumber < 3)
            {
                if (directionWhileAttacking != Vector3.zero)
                {
                    Debug.Log("Last Move Was" + lastMoveDir + " and rotationZ was " + rotationZ + " and directionWhileAttacking is " + directionWhileAttacking);
                    lastMoveDir = directionWhileAttacking;

                    rotationZ = calcZ(new Vector3(lastMoveDir.x, lastMoveDir.z, 0));
                    Debug.Log("Now rotationZ is " + rotationZ);
                    canvasRotTransf.rotation = Quaternion.Euler(0, 0, rotationZ);
                    regularRotTransf.rotation = Quaternion.Euler(0, rotationZ, 0);
                }
                currWalkSpeed = walkSpeed * speedAttackMultiplier;
                startFireTarget = Time.time;
                inputDisabled = true;
                attackNumber++;
                anim.Play("ANIM_Hero_Attack0" + attackNumber + "_EDIT", -1, 0f);
            }
        }else if (currItemType == ItemHeldType.Seed)
        {
            GridCell myCell = myCellTargetting.getCurrentCell();
            if (myCell!= null)
            {
                if (!myCell.hasChildTransform())
                {
                    GameObject go = Instantiate(PlantObj, myCell.transform);
                    myCell.assignChildTransform(go.transform);
                }
                else
                {
                    PlantCell pc = myCell.getChildTransform().GetComponent<PlantCell>();
                    if (pc != null)
                    {
                        if (pc.isRipe())
                        {
                            Destroy(pc.gameObject);
                            myCell.removeTargeted();

                        }
                    }
                }
                
            }
        }
        

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

	public void takeDamage(float damage){
		HP-=damage;
        setHPBar();
		if (HP<=0){
			Die();
		}
	}

    public void takeDamage(float damage,Vector3 sourcePos)
    {
        Debug.Log(Vector3.Angle(sourcePos - myTransform.position, lastMoveDir));
        if (isParrying && Vector3.Angle(sourcePos - myTransform.position, lastMoveDir) < blockAngle)
        {
            anim.Play("ANIM_Hero_GuardHit_EDIT", -1, 0f);
        }
        else
        {
            HP -= damage;
            setHPBar();
            if (HP <= 0)
            {
                Die();
            }
        }
    }

    void Die(){
        inputDisabled = true;
        anim.SetBool("IsDead", true);
        deathCanvas.GetComponent<DeathMenuFade>().BeginFadeLogic();
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
        //Debug.Log(other.transform.name);
       

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

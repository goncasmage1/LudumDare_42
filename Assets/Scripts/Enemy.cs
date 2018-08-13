using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    private bool bConsumedPlant = false;
    private bool bConsumingPlant = false;
    private bool bTargetInRange = false;
    private bool isFlinching1 = false;
    private bool isFlinching2 = false;
    private bool isAttacking = false;
    private bool waitingForImpact = true;

    public float Speed = 2f;
    public float Damage = 20f;
    public float AttackDistance = 0.6f;
    public float AttackInterruptDistance = 1.4f;
    public float AttackInitialDelay = 0.5f;
    public float AttackInterval = 1f;
    public float StrongAttackFirstDelay = 0.53f;
    public float StrongAttackSecondDelay = 0.53f;
    public float StrongAttackInterval = 5f / 3f;
    public float flinchTime = 0.56666f;
    public float PlantConsumptionTime = 4f;
    public float FreezeDelay = 0.1f;
    public float FreezeTime = 0.2f;

    [HideInInspector]
    public PlantCell targetPlant;

    [HideInInspector]
    public AISpawner spawner;

    public Transform target = null;
    private Transform aimRotation;
    private Transform eatFX;

    private Rigidbody rb = null;
    private Animator anim = null;
    private Animator strongAnim = null;
    private GameObject enemyObject = null;
    private GameObject strongEnemyObject = null;
    public float maxSpeed;
    public float walkSpeed;
    public float delayDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aimRotation = transform.Find("AimRotation").GetComponent<Transform>();
        anim = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
        strongAnim = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Animator>();
        if (anim == null || strongAnim == null) Debug.LogError("Couldn't find Animator!");

        enemyObject = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        strongEnemyObject = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
        if (enemyObject == null || strongEnemyObject == null) Debug.LogError("Couldn't find Enemy objects!");
        RuntimeManager.PlayOneShot("event:/SFX/Enemys/normalenemy_grunt", Vector3.zero);
        strongEnemyObject.SetActive(false);
    }
	
	void Update () {

        if (target == null) return;
        bool busy = isFlinching1 || isFlinching2 || bConsumingPlant || isAttacking;

        if (!busy)
        {
            aimRotation.LookAt(target);
            Quaternion clampedRotation = aimRotation.rotation;
            clampedRotation.x = 0;
            clampedRotation.z = 0;
            aimRotation.rotation = clampedRotation;
        }

        Vector3 position = transform.position;
        if (!bTargetInRange)
        {
            if ((position - target.position).magnitude <= AttackDistance && !busy)
            {
                bTargetInRange = true;
                StartAttacking();
            }
            if (!busy)
            {
                if (rb.velocity.magnitude < maxSpeed)
                {
                    rb.AddForce(aimRotation.forward * walkSpeed);
                    anim.SetFloat("Speed", 1f);
                }
                
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
        else 
        {
            if ((position - target.position).magnitude > AttackInterruptDistance)
                InterruptAttack();
        }

    }

    public bool HasConsumedPlant() { return bConsumedPlant; }

    public void ConsumePlant(PlantCell plant)
    {
        targetPlant = plant;
        bConsumingPlant = true;
        anim.SetBool("Eating", true);
        Invoke("GrowStronger", PlantConsumptionTime);
        RuntimeManager.PlayOneShot("event:/SFX/Enemys/enemy_eatingplants", Vector3.zero);

        eatFX = Instantiate(spawner.enemyEatFX, transform.position, Quaternion.identity);
    }

    void GrowStronger()
    {
        if (eatFX != null) Destroy(eatFX.gameObject);
        bConsumedPlant = true;
        RuntimeManager.PlayOneShot("event:/SFX/Enemys/enemy_transformation", Vector3.zero);
        enemyObject.SetActive(false);
        strongEnemyObject.SetActive(true);
        transform.GetComponent<EnemyHealth>().GrowBigger();
        anim = strongAnim;
        Destroy(targetPlant.gameObject);
        targetPlant = null;
        Invoke("FinishGrowing", 1f);

        Transform particles = Instantiate(spawner.enemyTransformFX, transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 1f);
    }

    void FinishGrowing()
    {
        bConsumingPlant = false;
    }

    void StartAttacking()
    {
        anim.SetBool("Attacking", true);
        isAttacking = true;
        waitingForImpact = true;
        Debug.Log("Start Attacking");
        if (!bConsumedPlant)
        {
            Invoke("AttackTarget", AttackInitialDelay);
            Invoke("FinishAttack", AttackInterval);
        }
        else
        {
            Invoke("AttackTargetStrong1", StrongAttackFirstDelay);
            Invoke("FinishAttack", StrongAttackInterval);
        }
    }

    void AttackTarget()
    {
        waitingForImpact = false;
        if (!bTargetInRange) return;
        Debug.Log("Attack!");
        Invoke("AttackTarget", AttackInterval);
    }

    void AttackTargetStrong1()
    {
        waitingForImpact = false;
        if (!bTargetInRange) return;
        Debug.Log("Attack 1");
        Invoke("AttackTargetStrong2", StrongAttackSecondDelay);
        Invoke("AttackTargetStrong1", StrongAttackInterval);
    }
    public void DoDamage()
    {
        if (!bTargetInRange) return;
             target.GetComponent<PlayerScript>().takeDamage(Damage, transform.position);
    }
    void AttackTargetStrong2()
    {
        if (!bTargetInRange) return;
        Debug.Log("Attack 2");
    }

    void FinishAttack()
    {
        if (isAttacking)
        {
            if (bTargetInRange) Invoke("FinishAttack", AttackInterval);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
    }

    void InterruptAttack()
    {
        Debug.Log("Attack interrupted!");
        bTargetInRange = false;
        if (waitingForImpact)
        {
            anim.SetBool("Attacking", false);
        }
        isAttacking = false;

        if (bConsumedPlant)
        {
            CancelInvoke("AttackTarget");
        }
        else
        {
            CancelInvoke("AttackTargetStrong1");
            CancelInvoke("AttackTargetStrong2");
        }
    }

    public void Flinch()
    {
        if (isFlinching1 || isFlinching2) CancelInvoke("RecoverFromFlinch");
        RuntimeManager.PlayOneShot("event:/SFX/Enemys/enemy_pain", Vector3.zero);
        if (isAttacking)
        {
            InterruptAttack();
            isAttacking = false;
            CancelInvoke("FinishAttack");
        }

        if (isFlinching1)
        {
            isFlinching1 = false;
            isFlinching2 = true;
            anim.SetBool("Flinching2", true);
            anim.SetBool("Flinching1", false);
        }
        else
        {
            isFlinching1 = true;
            isFlinching2 = false;
            anim.SetBool("Flinching1", true);
            anim.SetBool("Flinching2", false);
        }

        //Invoke("Freeze", FreezeDelay);
        Invoke("RecoverFromFlinch", flinchTime /*+ FreezeTime*/);
    }

    void Freeze()
    {
        anim.enabled = false;
        Invoke("Unfreeze", FreezeTime);
    }

    void Unfreeze()
    {
        anim.enabled = true;
    }

    void RecoverFromFlinch()
    {
        Debug.Log("Recovered!");
        isFlinching1 = false;
        isFlinching2 = false;
        anim.SetBool("Flinching1", false);
        anim.SetBool("Flinching2", false);
    }

    public void Die()
    {
        target.GetComponent<PlayerScript>().enemyKilled();
        if(bConsumedPlant)
            RuntimeManager.PlayOneShot("event:/SFX/Enemys/heavyenemy_death", Vector3.zero);
        else
            RuntimeManager.PlayOneShot("event:/SFX/Enemys/normalenemy_death", Vector3.zero);
        if (eatFX != null) Destroy(eatFX.gameObject);
    }
}

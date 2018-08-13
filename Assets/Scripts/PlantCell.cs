using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
[RequireComponent(typeof(CapsuleCollider))]
public class PlantCell : MonoBehaviour {

    enum PlantStage { Initial, Grown, Tower};

    PlantStage plantStage = PlantStage.Initial;

    private bool beingEaten = false;
    private bool shouldBecomTower = false;

    public float PlantRegenAmount = 40f;
    public float PlantGrowthTime = 10f;
    public float PlantToTowerTime = 20f;

    public float TowerAttackDistance = 6f;
    public float TowerAttackInterval = 3f;
    public float TowerDamage = 50f;
    public float shockTime = 0.7f;

    public LayerMask raycastMask;

    private Vector3 towerPosition;
    private Transform shock;
    public GameObject spikes;
    public int ammo = 12;
    private int maxAmmo = 12;
    private List<GameObject> ammoObjs;
    private Vector3[] raycastLocations = { new Vector3(1f, 2f, 1f),
                                           new Vector3(1f, 2f, 0f),
                                           new Vector3(1f, 2f, -1f),
                                           new Vector3(0f, 2f, -1f),
                                           new Vector3(-1f, 2f, -1f),
                                           new Vector3(-1f, 2f, 0f),
                                           new Vector3(-1f, 2f, 1f),
                                           new Vector3(0f, 2f, 1f) };

    private CapsuleCollider capsule;

    private Enemy enemyTarget = null;
    private AISpawner spawner;

    private Animator anim;

    private List<Collider> consumers = new List<Collider>();

    public bool IsPlantGrown() { return plantStage == PlantStage.Grown; }

    private void Awake()
    {
        ammo = 0;
        capsule = GetComponent<CapsuleCollider>();
        capsule.isTrigger = true;
        Transform canvasT = transform.Find("Canvas");
        ammoObjs = new List<GameObject>();
        foreach (Transform t in canvasT)
        {
            ammoObjs.Add(t.gameObject);
        }
        updateAmmoUI();
        
        towerPosition = transform.position;

        shock = transform.GetChild(1);
        shock.gameObject.SetActive(false);

        spawner = FindObjectOfType<AISpawner>();
        if (spawner == null) Debug.LogError("Couldn't find AISpawner!");

        anim = GetComponentInChildren<Animator>();
        if (anim == null) Debug.LogError("Couldn't find Animator in plant!");
    }
    private void updateAmmoUI()
    {
        for (int i = 0; i < ammoObjs.Count; i++)
        {
            if (i < ammo )
            {
                ammoObjs[i].SetActive(true);
            }
            else
            {
                ammoObjs[i].SetActive(false);
            }
        }
    }
    private void Start()
    {
        Invoke("GrowPlant", PlantGrowthTime);
    }

    private void Update()
    {
        if (!(plantStage == PlantStage.Tower)) return;

        Enemy closestTarget = null;
        float closestDistance = 0f;

        foreach (Enemy enemy in spawner.enemies)
        {
            float distance = (enemy.transform.position - transform.position).magnitude;
            if (distance <= TowerAttackDistance)
            {
                if (closestDistance == 0f || distance < closestDistance)
                {
                    closestTarget = enemy;
                    closestDistance = distance;
                }
            }
        }

        //If found a target
        if (closestTarget != null)
        {
            //and enemy wasn't set, start attacking
            if (enemyTarget == null)
            {
                CancelInvoke("AttackTarget");
                Invoke("AttackTarget", TowerAttackInterval);
            }

            if (enemyTarget != closestTarget) enemyTarget = closestTarget;
        }
        //If didn't find a target
        else
        {
            //and enemy is still set, stop attacking
            if (enemyTarget != null)
            {
                enemyTarget = null;
                CancelInvoke("AttackTarget");
            }
        }
    }

    void AttackTarget()
    {
        if (enemyTarget != null)
        {
            if (ammo > 0) { 
            enemyTarget.GetComponent<Health>().ReceiveDamage(TowerDamage);
            RuntimeManager.PlayOneShot("event:/SFX/Scenery/tower_shooting", Vector3.zero);
            Vector3 distance = enemyTarget.transform.position - towerPosition;
            Quaternion rotation = Quaternion.LookRotation(distance);
            rotation *= Quaternion.AngleAxis(90, Vector3.up);
            shock.transform.rotation = rotation;
            shock.localScale = (new Vector3(1f, 1f, 1f) * distance.magnitude);
            shock.gameObject.SetActive(true);
            ammo--;
            updateAmmoUI();
                if (ammo == 0)
                {
                    anim.Play("ANIM_Tower_NoAmmo", -1, 0f);
                }
            }
            Invoke("AttackTarget", TowerAttackInterval);
            Invoke("HideShock", shockTime);
        }
    }
    public void reload()
    {
        if(ammo==0)
            anim.Play("ANIM_Tower_AddAmmo", -1, 0f);
        ammo =Mathf.Min(maxAmmo,ammo + 3);
        updateAmmoUI();
        

    }
    void HideShock()
    {
        shock.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        transform.parent.GetComponent<GridCell>().setHasPlantRipe(false);
    }
    public void GrowPlant()
    {
        ;
        plantStage = PlantStage.Grown;
        transform.parent.GetComponent<GridCell>().setHasPlantRipe(true);
        anim.SetBool("Ready", true);
        if (consumers.Count > 0)
        {
            beingEaten = true;
            consumers[0].GetComponentInParent<Enemy>().ConsumePlant(this);
            return;
        }
        RuntimeManager.PlayOneShot("event:/SFX/Scenery/flower_ready", Vector3.zero);
        Invoke("BecomeTower", PlantToTowerTime);
    }

    public void StoppedEating()
    {
        beingEaten = false;
        if (shouldBecomTower) BecomeTower();
    }

    private void BecomeTower()
    {
        if (beingEaten)
        {
            shouldBecomTower = true;
            return;
        }
        spikes.SetActive(true);
        plantStage = PlantStage.Tower;
        capsule.isTrigger = false;
        anim.SetBool("Tower", true);
        DestroyAdjacentTiles();
        ammo = maxAmmo;
        updateAmmoUI();
    }
    public bool isTowerAndNeedsAmmo()
    {
        return (ammo < maxAmmo && plantStage == PlantStage.Tower); 
    }

    private void DestroyAdjacentTiles()
    {
        foreach (Vector3 pos in raycastLocations)
        {
            RaycastHit Hit;
            if (Physics.Raycast(towerPosition + pos, Vector3.down, out Hit, 1f, raycastMask))
            {
                Debug.Log("Found plant!");
                //Debug.DrawLine()
             
                if (Hit.collider.GetComponent<PlantCell>() != null) Destroy(Hit.collider.gameObject);
            }
        }
        StartCoroutine("disableAdjacentTiles");
    }
    IEnumerator disableAdjacentTiles()
    {
        int i, j = -1;
        Transform gc = GameObject.FindObjectOfType<GridManager>().transform;

        for (i = -1; i <= 1; i++)
        {
            for (j = -1; j <= 1; j++)
            {
              
                    Debug.Log("Cell" + Mathf.Floor(transform.position.x + i) + "|" + Mathf.Floor(transform.position.z + j));
                    Transform t = gc.Find("Cell" + Mathf.Floor(transform.position.x + i) + "|" + Mathf.Floor(transform.position.z + j));
                    GridCell gridC=t.GetComponent<GridCell>();
                    gridC.blockForPlants();
                
            }
            yield return null;
        }
        yield return null;
    }
    public bool isRipe()
    {
        return plantStage == PlantStage.Grown;
        
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant() || beingEaten) return;

        if (plantStage == PlantStage.Grown)
        {
            beingEaten = true;
            enemy.ConsumePlant(this);
        }
        else
        {
            consumers.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null || enemy.HasConsumedPlant()) return;

        consumers.Remove(other);
    }
}

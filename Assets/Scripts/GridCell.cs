using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GridCell : MonoBehaviour {

    enum PlantStage { None, Growing, Ready};

    private bool block = false;
    private bool highlighted = false;
    private bool isTower = false;

    public float PlantGrowthTime = 10f;
    public float PlantToTowerTime = 20f;

    private PlantStage plantStage = PlantStage.None;

    private BoxCollider boxCollider = null;
    private Transform highlight = null;
    private Transform plant = null;
    private Transform plantReady = null;
    private Transform tower = null;

    private void Awake () {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        highlight = gameObject.transform.GetChild(1);
        highlight.gameObject.SetActive(false);

        plant = gameObject.transform.GetChild(2);
        plant.gameObject.SetActive(false);

        plantReady = gameObject.transform.GetChild(3);
        plantReady.gameObject.SetActive(false);

        tower = gameObject.transform.GetChild(4);
        tower.gameObject.SetActive(false);
    }

    private void Start()
    {
        //PutPlant();
    }

    public void Block()
    {
        boxCollider.enabled = true;
        block = true;
    }

    public void UnBlock()
    {
        boxCollider.enabled = true;
        block = false;
    }

    public bool IsBlocking() { return block; }

    public void Highlight()
    {
        highlight.gameObject.SetActive(true);
        highlighted = true;
    }

    public void Unhighlight()
    {
        highlight.gameObject.SetActive(false);
        highlighted = false;
    }

    public bool IsHighlighted() { return highlighted; }

    private void ShowPlant()
    {
        plantStage = PlantStage.Growing;
        plant.gameObject.SetActive(true);
    }

    private void ShowGrowedPlant()
    {
        plantStage = PlantStage.Ready;
        plant.gameObject.SetActive(false);
        plantReady.gameObject.SetActive(true);
    }

    private void HidePlant()
    {
        plantStage = PlantStage.None;
        plant.gameObject.SetActive(false);
        plantReady.gameObject.SetActive(false);
    }

    private void ShowTower()
    {
        tower.gameObject.SetActive(true);
        isTower = true;
    }

    private void HideTower()
    {
        tower.gameObject.SetActive(false);
        isTower = false;
    }

    public bool IsTower() { return isTower; }

    public void PutPlant()
    {
        if (plantStage != PlantStage.None || block) return;

        Unhighlight();
        ShowPlant();

        Invoke("GrowPlant", PlantGrowthTime);
    }

    public void GrowPlant()
    {
        ShowGrowedPlant();
        Invoke("GrowIntoTower", PlantToTowerTime);
    }

    public bool HarvestPlant()
    {
        if (plantStage != PlantStage.Ready) return false;

        HidePlant();
        return true;
    }

    private void GrowIntoTower()
    {
        if (plantStage != PlantStage.Ready) return;

        HidePlant();
        ShowTower();
        Block();
    }
}

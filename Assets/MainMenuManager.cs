using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private GameObject Instructions;
    private GameObject Credits;

	// Use this for initialization
	void Start () {

        Instructions = transform.GetChild(4).gameObject;
        Credits = transform.GetChild(5).gameObject;
        if (Credits == null || Instructions == null) Debug.LogError("Couldn't find Credits or Instructions!");

        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OnPlay);
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(OnInstructions);
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(OnCredits);

        Instructions.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ExitInstructions);
        Credits.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ExitCredits);

        Instructions.SetActive(false);
        Credits.SetActive(false);
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Fred", LoadSceneMode.Single);
    }

    public void OnInstructions()
    {
        Instructions.SetActive(true);
    }

    public void OnCredits()
    {
        Credits.SetActive(true);
    }

    public void ExitInstructions()
    {
        Instructions.SetActive(false);
    }

    public void ExitCredits()
    {
        Credits.SetActive(false);
    }
}

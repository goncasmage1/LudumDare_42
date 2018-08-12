using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBankAndScene : MonoBehaviour
{
    [SerializeField]
    public string sceneName;

    [FMODUnity.BankRef]
    public List<string> banks;

    private void Awake()
    {
        foreach (string b in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(b, true);
            Debug.Log("Loaded bank " + b);
        }
    }

    void Update()
	{
        if (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
        {
            Debug.Log("Master Bank Loaded");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Fred", LoadSceneMode.Single);
    }
}

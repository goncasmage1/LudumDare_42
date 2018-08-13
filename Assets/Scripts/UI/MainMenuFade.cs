using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFade : MonoBehaviour {

    public float fadeDelay = 3f;
    public float fadeTime = 2f;

    private bool fading = false;

    private float fadeCounter = 0f;

    private CanvasGroup fadeTarget;

    private void Awake()
    {
        fadeTarget = GetComponent<CanvasGroup>();
        if (fadeTarget == null) Debug.LogError("Couldn't find fade Image!");
    }

    // Use this for initialization
    void Start () {
        Invoke("StartFade", fadeDelay);
	}
	
	// Update is called once per frame
	void Update () {
        if (!fading) return;

        fadeCounter += Time.deltaTime;

        float newAlpha = fadeCounter / fadeTime;
        if (newAlpha > 1f) fading = false;
        fadeTarget.alpha = newAlpha;
    }

    void StartFade()
    {
        fading = true;
    }
}

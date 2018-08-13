using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenuFade : MonoBehaviour {

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

    public void BeginFadeLogic()
    {
        Invoke("StartFade", fadeDelay);
    }

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

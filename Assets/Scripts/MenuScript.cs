using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour, IClickable
{
    public event Action RestartGame;
    public GameObject loadingScreen;

    private void Start()
    {
        gameObject.SetActive(false);
        loadingScreen.SetActive(false);
    }

    public void Tapped()
    {
        StartCoroutine(LoadingScreenCoroutine(2));
    }

    private IEnumerator LoadingScreenCoroutine(float delayDuration)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(delayDuration);
        // reload scene after pressing Restart button
        RestartGame?.Invoke();
        loadingScreen.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
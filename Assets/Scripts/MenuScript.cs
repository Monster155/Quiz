using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour, IClickable
{
    public event Action RestartGame;
    public Animator imageAnim;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Tapped()
    {
        // reload scene after pressing Restart button
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // version with no reloading scene (but some animation can be disabled)
        /*RestartGame.Invoke();
        gameObject.SetActive(false);*/
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive)
            imageAnim.Play("ImageFadeIn");
    }
}
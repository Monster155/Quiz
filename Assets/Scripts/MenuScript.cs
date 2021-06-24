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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // RestartGame.Invoke();
        // gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive)
            imageAnim.Play("ImageFadeIn");
    }
}
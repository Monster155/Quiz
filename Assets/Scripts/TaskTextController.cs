using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTextController : MonoBehaviour
{
    public Text text;

    public void SetText(string newText, bool isFirst)
    {
        text.text = newText;
        if (isFirst)
        {
            // for animation playing and other restart functionality
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    public string objectName;
    public Sprite sprite;
}
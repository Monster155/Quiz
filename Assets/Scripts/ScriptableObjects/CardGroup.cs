using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Group", menuName = "Cards/Card Group")]
public class CardGroup : ScriptableObject
{
    public string groupName;
    public Card[] cards;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CellScript))]
public class CardToPrefab : MonoBehaviour
{
    public Card Card;
    public Image Image;
    public CellScript CellScript;

    void Start()
    {
        if (CellScript == null)
        {
            CellScript = GetComponent<CellScript>();
        }

        SetCard(Card);
    }

    public void SetCard(Card card)
    {
        Card = card;
        Image.sprite = card.sprite;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using SRandom = System.Random;
using URandom = UnityEngine.Random;

public class CellsController : MonoBehaviour
{
    public event Action LevelFinished;
    public ParticleController particleController;
    public GameObject cellPrefab;
    public TaskTextController taskTextController;
    public Card[] cards;

    private int[] _answers;

    void OnEnable()
    {
        GenerateNewAnswers();
    }

    public void GenerateNewAnswers()
    {
        // generating array of answers
        var ran = new SRandom();
        _answers = Enumerable.Range(0, cards.Length).OrderBy(x => ran.Next()).ToArray();
        // logging
        string log = "";
        foreach (var answer in _answers)
        {
            log += answer + " ";
        }

        Debug.Log("Answers: " + log);
    }

    public void LoadLevel(int levelNum)
    {
        Debug.Log("Level number: " + levelNum);

        // destroy all children before creating new
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Generating answer id in array and card id
        int answerId = URandom.Range(0, 3 * levelNum - 1);
        Debug.Log("Answer Id: " + answerId);

        int answerCardId = _answers[levelNum - 1];
        Debug.Log("Answer Card Id: " + answerCardId);

        // Generating array of cards on map
        int[] slots = new int[3 * levelNum];
        var ran = new SRandom();
        slots = Enumerable.Range(0, cards.Length).OrderBy(x => ran.Next()).ToArray();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == answerCardId)
            {
                var temp = slots[i];
                slots[i] = slots[answerId];
                slots[answerId] = temp;
            }
        }

        // Set task text for game
        taskTextController.SetText("Find " + cards[answerCardId].objectName, levelNum <= 1);

        // Calculating positions for cells and fill cells
        int z = 0;
        for (int i = 0; i < levelNum; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // prepare position for cell
                Vector3 pos = new Vector3(2.2f - j * 2.2f, 0);
                switch (levelNum)
                {
                    case 1:
                        pos.y = 2 - 1 * 2.2f;
                        break;
                    case 2:
                        pos.y = 2 - 1.1f - i * 2.2f; // 2 - levelNum * 1.1f - i * 2.2f
                        break;
                    case 3:
                        pos.y = 2 - i * 2.2f;
                        break;
                }

                // spawn cell
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                // if answer cell
                if (z == answerId)
                {
                    // add card Scriptable Object and convert in to Prefab's values
                    cell.GetComponent<CardToPrefab>().SetCard(cards[answerCardId]);
                    // Set settings for Cell Script 
                    CellScript cellScript = cell.GetComponent<CellScript>();
                    cellScript.IsAnswer = true;
                    cellScript.CorrectButtonTapped += () => LevelFinished?.Invoke();
                    cellScript.ParticleController = particleController;
                }
                // if other cell
                else
                {
                    cell.GetComponent<CardToPrefab>().SetCard(cards[slots[z]]);
                }

                // set IsFirst, if this is first level 
                if (levelNum <= 1)
                {
                    cell.GetComponent<CellScript>().IsFirst = true;
                }

                // moving cells to Z=0 from Z< -3000
                var localPosition = cell.transform.localPosition;
                localPosition = new Vector3(localPosition.x, localPosition.y, 0);
                cell.transform.localPosition = localPosition;

                z++;
            }
        }
    }
}
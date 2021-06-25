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
    public CardGroup[] cardGroups;

    private int[][] _answers;
    private int[] _nextAnswerId;

    void OnEnable()
    {
        GenerateNewAnswers();
    }

    public void GenerateNewAnswers()
    {
        _answers = new int[cardGroups.Length][];
        _nextAnswerId = new int[cardGroups.Length];
        // generating array of answers
        for (int i = 0; i < cardGroups.Length; i++)
        {
            var ran = new SRandom();
            _answers[i] = Enumerable.Range(0, cardGroups[i].cards.Length).OrderBy(x => ran.Next()).ToArray();
            _nextAnswerId[i] = -1;
        }

        // logging
        string log = "";
        foreach (var answer in _answers)
        {
            foreach (int an in answer)
            {
                log += an + " ";
            }

            log += "\n";
        }

        Debug.Log("Answers: " + log);
    }

    public void LoadLevel(int levelNum)
    {
        Debug.Log("Level number: " + levelNum);

        // getting next value from answers array by chosen type of cards
        int nextType = URandom.Range(0, cardGroups.Length - 1); // 0-0.999 convert to 0, and only 1.0 convert to 1
        Debug.Log("Next Type: " + nextType);
        _nextAnswerId[nextType]++;
        int k = nextType;
        // if current cards group is not available, choosing next cards group
        while (_nextAnswerId[k] >= _answers[k].Length)
        {
            k = (k + 1) % _nextAnswerId.Length;
            _nextAnswerId[k]++;
            if (k == nextType)
            {
                //TODO game ends (all cards groups are not available)
                Debug.LogError("No more cards");
                Application.Quit();
                break;
            }
        }

        nextType = k;
        // get cards group for this level
        Card[] cards = cardGroups[nextType].cards;

        // destroy all children before creating new
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Generating answer id in array and card id
        int answerId = URandom.Range(0, 3 * levelNum - 1);
        Debug.Log("Answer Id: " + answerId);

        int answerCardId = _answers[nextType][_nextAnswerId[nextType]];
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CellsController cellsController;
    public MenuScript menu;

    private int _currentLevelNum = 1;
    private int _maxLevelNum = 3;

    void Start()
    {
        cellsController.LoadLevel(_currentLevelNum);
        cellsController.LevelFinished += LoadNextLevel;
        menu.RestartGame += DropLevelCount;
    }

    private void LoadNextLevel()
    {
        Debug.Log("Loading new Level");
        // increase level number
        _currentLevelNum++;
        // if this level was the last - open menu, else - generate next level
        if (_currentLevelNum <= _maxLevelNum)
            cellsController.LoadLevel(_currentLevelNum);
        else
            menu.SetActive(true);
    }

    private void DropLevelCount()
    {
        _currentLevelNum = 0;
        LoadNextLevel();
    }
}
﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


    public Level firstLevel;
    public Level LastLevel;

    public Level mainMenu;
    public Level helpScreen;

    public Level[] levels;

    public Level currentLevel;

	// Use this for initialization
	void Start () {
        levels = GetComponentsInChildren<Level>();

        foreach (Level l in levels)
        {
            l.gameObject.SetActive(false);
        }

        firstLevel.gameObject.SetActive(true);
        currentLevel = firstLevel;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToLevel(Level level)
    {
        currentLevel.Finish();
        LastLevel = currentLevel;
        currentLevel.gameObject.SetActive(false);
        level.gameObject.SetActive(true);
        level.Init();
        currentLevel = level;
    }

    public void GoToNextLevel()
    {
        GoToLevel(currentLevel.nextLevel);
    }

    public void GoToMainMenu()
    {
        GoToLevel(mainMenu);
    }

    public void GoToHelp()
    {
        GoToLevel(helpScreen);
    }

    public void GoToPrevious()
    {
        if (LastLevel != null)
        {
            GoToLevel(LastLevel);
        } else
        {
            GoToMainMenu();
        }
    }
}

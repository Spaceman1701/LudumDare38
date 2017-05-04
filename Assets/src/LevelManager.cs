using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


    public Level firstLevel;
    public Level LastLevel;

    public Level[] levels;

    public Level currentLevel;

    public Camera sceneCamera;

	// Use this for initialization
	void Start () {
        levels = GetComponentsInChildren<Level>();

        foreach (Level l in levels)
        {
            l.gameObject.SetActive(false);
        }

        firstLevel.gameObject.SetActive(true);
        sceneCamera.transform.position = new Vector3(firstLevel.transform.position.x, firstLevel.transform.position.y, -1);
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
        sceneCamera.transform.position = new Vector3(level.transform.position.x, level.transform.position.y, -1);
        currentLevel = level;
    }

    public void GoToNextLevel()
    {
        GoToLevel(currentLevel.nextLevel);
    }

    public void GoToPrevious()
    {
        if (LastLevel != null)
        {
            GoToLevel(LastLevel);
        }
    }
}

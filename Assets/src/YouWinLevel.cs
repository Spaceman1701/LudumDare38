using UnityEngine;
using System.Collections;
using System;

public class YouWinLevel : Level {
    public override void Finish()
    {
        Debug.Log("leaving you win");
    }

    public override void Init()
    {
        Debug.Log("entering you win");
    }

    public override void ResetLevel()
    {
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponentInParent<LevelManager>().GoToNextLevel();
        }
	}
}

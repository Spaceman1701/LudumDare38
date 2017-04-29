using UnityEngine;
using System.Collections;
using System;
using src;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameLevel : Level {

    public string prefix = "TEST: ";

    public Text header;

    public TerminalManager terminalManger;

    public string name;
	// Use this for initialization
	void Start () {
        header.text = prefix + name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public TerminalManager GetTerminalManager()
    {
        return terminalManger;
    }

    public override void Finish()
    {
        Debug.Log("finsish level: " + name);
    }

    public override void Init()
    {
        Debug.Log("init level: " + name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void ResetLevel()
    {
        Debug.Log("reset called");
        GetComponentInChildren<PlayerController>().ForceReset();
    }
}

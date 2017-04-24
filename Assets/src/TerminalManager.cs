﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using src;

public class TerminalManager : MonoBehaviour {

    public Text info;
    public Text code;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InputCommand(string command)
    {
        Debug.Log(command);
        if (command == "run")
        {
            info.text = "INFO: Running program";
            RunCode();
        }
        if (command == "reset")
        {
            info.text = "INFO: Reseting simulation...";
            GetComponent<Level>().ResetLevel();
        }
        if (command == "clear")
        {
            info.text = "INFO:";
        }
        if (command == "exit")
        {
            info.text = "INFO: Exiting...";
            Application.Quit();
        }
    }

    public void RunCode()
    {
        string toRun = code.text;

        PlayerController pc = GetComponentInChildren<PlayerController>();
        pc.LoadProgram(toRun);
    }

    public void ShowRuntimeError()
    {
        info.text = "INFO: Error in program!";
    }

    public void ShowCodeHalt()
    {
        info.text = "INFO: HALT";
    }

    public void ShowCompilerError(string msg)
    {
        info.text = "ERROR: " + msg;
    }
}

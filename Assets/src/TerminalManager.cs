using UnityEngine;
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
            GetComponent<Level>().ResetLevel();
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
        if (command == "help")
        {
            GetComponentInParent<LevelManager>().GoToHelp();
        }
        if (command == "please god help")
        {
            info.text = "INFO: Not even God can help you.";
        }
        if (command == "menu")
        {
            GetComponentInParent<LevelManager>().GoToMainMenu();
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
        info.text = "ERROR: RUNTIME ERROR!";
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

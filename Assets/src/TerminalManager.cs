using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using src;

public class TerminalManager : MonoBehaviour {

    public delegate void CommandFunctiod();

    public Text info;
    public Text code;

    public Dictionary<string, CommandFunctiod> commands;

	// Use this for initialization
	void Start () {
        commands = new Dictionary<string, CommandFunctiod>();
        commands["run"] = RunCode;
        commands["reset"] = ResetLevel;
        commands["clear"] = Clear;
        commands["exit"] = Quit;
        commands["quit"] = Quit;
        commands["please god help"] = HelpGod;
        commands["help"] = Help;
        commands["menu"] = Menu;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InputCommand(string command)
    {
        Debug.Log(command);
        if (commands.ContainsKey(command))
        {
            commands[command].Invoke();
        } else
        {
            CommandNotFound();
        }
    }

    public void CommandNotFound()
    {
        info.text = "INFO: Command Not Found!";
    }

    public void Help()
    {

    }

    public void Menu()
    {

    }

    public void Clear()
    {
        info.text = "INFO:";
    }

    public void HelpGod()
    {
        info.text = "INFO: Not even God can help you.";
    }

    public void Quit()
    {
        info.text = "INFO: Exiting...";
        Application.Quit();
    }

    public void ResetLevel()
    {
        info.text = "INFO: Reseting simulation...";
        GetComponent<Level>().ResetLevel();
    }

    public void RunCode()
    {
        ResetLevel();
        info.text = "INFO: Running program";

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

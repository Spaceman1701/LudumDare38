using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalManager : MonoBehaviour {

    public Text info;

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
            info.text = "INFO: Running code...";
        }
        if (command == "reset")
        {
            info.text = "INFO: Reseting simulation...";
        }
        if (command == "compile")
        {
            info.text = "INFO: Compiling code...";
        }
    }
}

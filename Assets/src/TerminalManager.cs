using UnityEngine;
using System.Collections;

public class TerminalManager : MonoBehaviour {

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
            Debug.Log("Run Command Recieved");
        }
    }
}

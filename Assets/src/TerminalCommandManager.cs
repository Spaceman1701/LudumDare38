using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalCommandManager : MonoBehaviour {

    public string last;

	public void HandleCommand()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputField infield = GetComponent<InputField>();
            string command = infield.text.Trim();
            last = command;
            infield.text = "";
            GetComponentInParent<TerminalManager>().InputCommand(command);
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    public Text[] options;
    public Level[] levels;

    public uint selected;

	// Use this for initialization
	void Start () {
        //Screen.fullScreen = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selected -= 1;
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selected += 1;
        }

        selected %= (uint)options.Length;

        foreach (Text t in options)
        {
            t.fontStyle = FontStyle.Normal;
        }

        options[selected].fontStyle = FontStyle.Bold;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (levels.Length < selected || levels[selected] == null)
            {
                Application.Quit();
            }
            GetComponentInParent<LevelManager>().GoToLevel(levels[selected]);
        }
    }
}

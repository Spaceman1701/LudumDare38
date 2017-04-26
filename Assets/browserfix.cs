using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class browserfix : MonoBehaviour {

    bool visible = false;
    Text txt;
    string warningText = "CURRENT DISPLAY MODE UNSUPPORTED IN WEBGL. PLEASE SWITCH TO FULLSCREEN. KNOW INPUT ISSUES EXIST IN THE CURRENT MODE.";

    // Use this for initialization
    void Start () {
        txt = GetComponent<Text>();
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Screen.fullScreen)
        {
            txt.text = warningText;
        } else
        {
            txt.text = "";
        }
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class autoselection : MonoBehaviour {

    GameObject lastSelect;
    public bool overrideSelect = false;

	// Use this for initialization
	void Start () {
        InputField field = GetComponent<InputField>();
        field.Select();

        lastSelect = gameObject;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

    // Update is called once per frame
    void Update () {
        if (EventSystem.current.currentSelectedGameObject == null || overrideSelect)
        {
            overrideSelect = false;
            EventSystem.current.SetSelectedGameObject(lastSelect);
            InputField field = GetComponent<InputField>();
        }
        else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }
}

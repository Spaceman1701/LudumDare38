using UnityEngine;
using System.Collections;
using System;

public class MenuLevel : Level {
    public override void Finish()
    {
        Debug.Log("Leaving menu");
    }

    public override void Init()
    {
        Debug.Log("Entering Menu");
    }

    public override void ResetLevel()
    {
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}

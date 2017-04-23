using UnityEngine;
using System.Collections;
using System;

public class GameLevel : Level {

    public int name;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Finish()
    {
        Debug.Log("finsish level: " + name);
    }

    public override void Init()
    {
        Debug.Log("init level: " + name);
    }
}

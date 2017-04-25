using UnityEngine;
using System.Collections;

public class HelpLevel : Level {

    public override void Finish()
    {
        Debug.Log("leaving you win");
    }

    public override void Init()
    {
        Debug.Log("entering you win");
    }

    public override void ResetLevel()
    {
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponentInParent<LevelManager>().GoToPrevious();
        }
    }
}

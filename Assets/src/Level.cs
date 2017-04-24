using UnityEngine;
using System.Collections;

public abstract class Level : MonoBehaviour {

    public Level nextLevel;

    public abstract void Finish();
    public abstract void Init();
    public abstract void ResetLevel();
   
    
}

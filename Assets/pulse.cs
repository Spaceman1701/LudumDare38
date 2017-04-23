using UnityEngine;
using System.Collections;

public class pulse : MonoBehaviour {


    public float timer;
    public Color change = new Color(0, 0, 0, 0.0001f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        if (timer < 6)
        {
            sr.color -= change;
        } else if (timer < 12)
        {
            sr.color += change;
        } else
        {
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}

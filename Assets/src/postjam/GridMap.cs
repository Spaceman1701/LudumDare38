using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridMap : MonoBehaviour {

    public Texture2D mapImage;

    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject portalPrefab;

    public int xoffset = -20;
    public int yoffset = -15;

    public int[,] grid;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BuildMap()
    {
        Debug.Log("building map");

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 30; y++)
            {
                Color c = mapImage.GetPixel(x, y);
                Debug.Log(c);
                if (c == Color.black)
                {
                    GameObject wall = Instantiate(wallPrefab);
                    wall.transform.parent = transform;
                    wall.transform.localPosition = new Vector3(x + xoffset, y + yoffset, transform.position.z);
                } else if (c == Color.red)
                {
                    GameObject portal = Instantiate(portalPrefab);
                    portal.transform.parent = transform;
                    portal.transform.localPosition = new Vector3(x + xoffset, y + yoffset, transform.position.z);
                } else if (c == Color.green)
                {
                    GameObject player = Instantiate(playerPrefab);
                    player.transform.parent = transform;
                    player.transform.localPosition = new Vector3(x + xoffset, y + yoffset, transform.position.z);
                }
            }
        }
    }

    public void ClearMap()
    {
        foreach (Transform mb in GetComponentsInChildren<Transform>())
        {
            Debug.Log(mb.gameObject);
            if (mb.gameObject != gameObject)
            {
                GameObject.DestroyImmediate(mb.gameObject);
            }
        }
    }
}

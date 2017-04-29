using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using src;

public class GridMap : MonoBehaviour {
#if UNITY_EDITOR
    public Texture2D mapImage;
#endif
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject portalPrefab;

    public int xoffset = -20;
    public int yoffset = -15;

    public int width = 20;
    public int height = 30;

    public int goalX;
    public int goalY;

    public GridObject[,] grid;

    public enum ObjectType
    {
        NULL, WALL, ROCK, PORTAL, PLAYER
    }

    // Use this for initialization
    void Start () {
        grid = new GridObject[width, height];
        BuildGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BuildGrid()
    {
        Debug.Log("building grid");
        foreach (GridObject go in GetComponentsInChildren<GridObject>())
        {
            Vector3 location = go.gameObject.transform.localPosition;
            int gridX = (int)location.x - xoffset;
            int gridY = (int)location.y - yoffset;
            Debug.Log(go.gameObject + ", " + gridX + ", " + gridY);
            grid[gridX, gridY] = go;
            go.SetLocation(gridX, gridY);
        }
    }

    public ObjectType GetObjectAt(int x, int y)
    {
        if (x >= width || x < 0 || y >= height || y < 0 || grid[x, y] == null)
        {
            return ObjectType.NULL;
        }
        return grid[x, y].GetGridObjType();
    }


    public int GetGoalX()
    {
        return goalX;
    }

    public int GetGoalY()
    {
        return goalY;
    }

    public int Raycast(int startx, int starty, int dir)
    {
        if (dir < 0 || dir > 3)
        {
            return 0;
        }
        int delx = 0, dely = 0;
        switch (dir)
        {
            case 0:
                delx = 0;
                dely = 1;
                break;
            case 1:
                delx = 1;
                dely = 0;
                break;
            case 2:
                delx = 0;
                dely = -1;
                break;
            case 3:
                delx = -1;
                dely = 0;
                break;
        }
        int x = startx + delx, y = starty + dely;
        int steps = 1;
        while (GetObjectAt(x, y) == ObjectType.NULL)
        {
            x += delx;
            y += dely;
            steps++;
        }
        Debug.Log(GetObjectAt(x, y));
        return steps;
    }

#if UNITY_EDITOR
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
#endif

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

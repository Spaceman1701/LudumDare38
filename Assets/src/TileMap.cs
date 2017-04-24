using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace src
{
    public class TileMap : MonoBehaviour
    {
        public enum ObjectType
        {
            NULL, WALL, ROCK, PORTAL, PLAYER
        }

        public static float TILE_SIZE = 1.0f; //calculated for ~16px size tiles

        public int width = 20;
        public int height = 30;


        public GameObject[,] grid;

    // Use this for initialization
        void Start()
        {
            InitGrid();
        }


        void InitGrid()
        {
            grid = new GameObject[width, height];
            int centerX = width / 2;
            int centerY = height / 2;
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child == transform)
                {
                    continue;
                }
                float xFloat = child.localPosition.x / TILE_SIZE;
                float yFloat = child.localPosition.y / TILE_SIZE;
                int xInt = Mathf.FloorToInt(child.localPosition.x / TILE_SIZE);
                int yInt = Mathf.FloorToInt(child.localPosition.y / TILE_SIZE);
                if (xFloat != xInt || yFloat != yInt)
                {
                    float delX = -Mathf.Abs(xFloat - xInt) * TILE_SIZE;
                    float delY = -Mathf.Abs(yFloat - yInt) * TILE_SIZE;
                    child.localPosition += new Vector3(delX, delY, 0);
                    if ((int)child.localPosition.y != yInt)
                    {
                        Debug.Log("bug: " + yInt + ", " + child.localPosition.y / TILE_SIZE);
                    }
                }

                int gridX = (int)(child.localPosition.x / TILE_SIZE + centerX);
                int gridY = (int)(child.localPosition.y / TILE_SIZE + centerY);
                grid[gridX, gridY] = child.gameObject;
                GridObject go = child.gameObject.GetComponent<GridObject>();
                if (go != null)
                {
                    go.SetLocation(gridX, gridY);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("Grid: " + IsObjectAt(10, 19));
        }

        public ObjectType IsObjectAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height || grid[x, y] == null)
            {
                return ObjectType.NULL;
            }
            return grid[x, y].GetComponent<GridObject>().GetGridObjType();
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
            while (IsObjectAt(x, y) == ObjectType.NULL)
            {
                x += delx;
                y += dely;
                steps++;
            }
            Debug.Log(IsObjectAt(x, y));
            return steps;
        }

        public int GetGoalX()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (grid[i, j] != null)
                    {
                        if (grid[i, j].GetComponent<GridObject>().GetGridObjType() == ObjectType.PORTAL)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        public int GetGoalY()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (grid[i, j] != null)
                    {
                        if (grid[i, j].GetComponent<GridObject>().GetGridObjType() == ObjectType.PORTAL)
                        {
                            return j;
                        }
                    }
                }
            }
            return -1;
        }
    }
}


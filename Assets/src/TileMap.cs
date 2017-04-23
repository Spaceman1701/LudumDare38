using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace src
{
    public class TileMap : MonoBehaviour
    {


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
            Debug.Log(children.Length);
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
        }

        public bool IsObjectAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return true;
            }
            return grid[x, y] != null;
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
            while (!IsObjectAt(x, y))
            {
                x += delx;
                y += dely;
                steps++;
            }
            return steps;
        }
    }
}


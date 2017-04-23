using UnityEngine;
using System.Collections;

namespace src {
    public class PlayerController : MonoBehaviour, GridObject {

        public TileMap map;

        public float unitsToMove;

        public int gridX;
        public int gridY;

        public int dir;

        // Use this for initialization
        void Start() {

        }


        public void SetLocation(int x, int y)
        {
            gridX = x;
            gridY = y;
        }

        // Update is called once per frame
        void Update() {
            int oldX = gridX;
            int oldY = gridY;
            if (Input.GetKeyDown(KeyCode.UpArrow) && gridY < map.height - 1 && !map.IsObjectAt(gridX, gridY + 1))
            {
                Move(0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && gridY > 0 && !map.IsObjectAt(gridX, gridY - 1))
            {
                Move(2);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && gridX > 0 && !map.IsObjectAt(gridX - 1, gridY))
            {
                Move(3);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && gridX < map.width - 1 && !map.IsObjectAt(gridX + 1, gridY))
            {
                Move(1);
            }
            UpdateGridLocation(oldX, oldY);
        }

        void TurnLeft()
        {
            dir -= 1;
            dir %= 4;
        }

        void TurnRight()
        {
            dir += 1;
            dir %= 4;
        }

        void Move(int dir)
        {
            switch(dir)
            {
                case 0:
                    transform.position += new Vector3(0, unitsToMove, 0);
                    gridY++;
                    break;
                case 1:
                    transform.position += new Vector3(unitsToMove, 0, 0);
                    gridX++;
                    break;
                case 2:
                    transform.position += new Vector3(0, -unitsToMove, 0);
                    gridY--;
                    break;
                case 3:
                    transform.position += new Vector3(-unitsToMove, 0, 0);
                    gridX--;
                    break;
            }
        }

        int DistanceToObject()
        {
            return map.Raycast(gridX, gridY, dir);
        }

        void UpdateGridLocation(int oldX, int oldY)
        {
            map.grid[oldX, oldY] = null;
            map.grid[gridX, gridY] = gameObject;
        }
    }
}

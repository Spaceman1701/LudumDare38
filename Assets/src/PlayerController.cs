using UnityEngine;
using System.Collections.Generic;
using src.emulator;
using System;

namespace src {
    public class PlayerController : MonoBehaviour, GridObject {
        public byte MEMORY_DIR = 0;
        public byte LOC = 1; //2
        public byte GOAL_LOC = 3;

        public byte TYPE_WALL = 0;
        public byte TYPE_ROCK = 1;
        public byte TYPE_PORTAL = 2;

        public int initalX;
        public int initalY;
        public Vector3 initalLoc;

        public TileMap map;

        public float unitsToMove;

        public int gridX;
        public int gridY;

        public int dir;

        public CPU cpu;

        public int counter = 0;
        public bool running = false;

        IDictionary<Instruction.Type, CPU.ExternalInst> cpuInst;

        // Use this for initialization
        void Start() {
            cpuInst = new Dictionary<Instruction.Type, CPU.ExternalInst>();
            cpuInst[Instruction.Type.LOC] = Loc;
            cpu = new CPU(64, 4, cpuInst);
        }

        public void Loc(byte[] mem, byte[] regs, Program p, Instruction i)
        {
            byte dir = p.EvaluateNumber(i.ParamOne.data);
            SafeMove(dir);
            mem[1] = (byte)gridX;
            mem[2] = (byte)gridY;
        }

        public void Tul(byte[] mem, byte[] regs, Program p, Instruction i)
        {
            TurnLeft();
            mem[0] = (byte)dir;
        }

        public void Tur(byte[] mem, byte[] regs, Program p, Instruction i)
        {
            TurnRight();
            mem[1] = (byte)dir;
        }

        public void Sca(byte[] mem, byte[] regs, Program p, Instruction i)
        {
            int ptr = p.EvaulateMemoryPtr(i.ParamOne.data, regs);
            int dist = DistanceToObject();
            mem[ptr] = (byte)dist;
        }

        public void LoadProgram(string program)
        {
            Program p = Compiler.Compile(program);
            cpu.Reset();
            cpu.LoadProgram(p);
            counter = 0;
        }

        public TileMap.ObjectType GetGridObjType()
        {
            return TileMap.ObjectType.PLAYER;
        }

        public void SetLocation(int x, int y)
        {
            gridX = x;
            gridY = y;

            initalX = gridX;
            initalY = gridY;

            initalLoc = transform.localPosition;
        }

        public void ForceReset()
        {
            gridX = initalX;
            gridY = initalY;

            transform.localPosition = initalLoc;
            running = false;
            cpu.Reset();
        }


        bool IsPassable(int x, int y)
        {
            return map.IsObjectAt(x, y) != TileMap.ObjectType.WALL && map.IsObjectAt(x, y) != TileMap.ObjectType.ROCK;
        }

        void SafeMove(int dir)
        {
            if (dir == 0 && gridY < map.height - 1 && IsPassable(gridX, gridY + 1))
            {
                Move(0);
            }
            if (dir == 2 && gridY > 0 && IsPassable(gridX, gridY - 1))
            {
                Move(2);
            }
            if (dir == 3 && gridX > 0 && IsPassable(gridX - 1, gridY))
            {
                Move(3);
            }
            if (dir == 1 && Input.GetKeyDown(KeyCode.RightArrow) && gridX < map.width - 1 && IsPassable(gridX + 1, gridY))
            {
                Move(1);
            }
        }

        // Update is called once per frame
        void Update() {
            int oldX = gridX;
            int oldY = gridY;
            if (Input.GetKeyDown(KeyCode.UpArrow) && gridY < map.height - 1 && IsPassable(gridX, gridY + 1))
            {
                //Move(0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && gridY > 0 && IsPassable(gridX, gridY - 1))
            {
                //Move(2);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && gridX > 0 && IsPassable(gridX - 1, gridY))
            {
                //Move(3);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && gridX < map.width - 1 && IsPassable(gridX + 1, gridY))
            {
                //Move(1);
            }
            UpdateGridLocation(oldX, oldY);
            CheckAtGoal();
            try
            {
                if (running)
                {
                    if (counter == 0)
                    {
                        Debug.Log("exec");
                        running = cpu.ExecuteSingleLine();
                        counter = 20;
                        if (!running)
                        {
                            GetComponentInParent<TerminalManager>().ShowCodeHalt();
                        }
                    }
                    counter--;
                }
            } catch (Exception e)
            {
                running = false;
                GetComponentInParent<TerminalManager>().ShowRuntimeError();
            }
        }

        void CheckAtGoal()
        {
            if (map.IsObjectAt(gridX, gridY) == TileMap.ObjectType.PORTAL)
            {
                GetComponentInParent<LevelManager>().GoToNextLevel();
            }
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
        }
    }
}

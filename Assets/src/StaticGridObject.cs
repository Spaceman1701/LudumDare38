using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace src
{
    class StaticGridObject : GridObject
    {
        public GridMap.ObjectType type;

        public int gridX;
        public int gridY;

        public override GridMap.ObjectType GetGridObjType()
        {
            return type;
        }

        public override void SetLocation(int x, int y)
        {
            gridX = x;
            gridY = y;
        }
    }
}

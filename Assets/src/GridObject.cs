using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace src
{
    public abstract class GridObject : MonoBehaviour
    {
        public abstract void SetLocation(int x, int y);
        public abstract GridMap.ObjectType GetGridObjType();
    }
}

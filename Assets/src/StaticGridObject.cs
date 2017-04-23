using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace src
{
    class StaticGridObject : MonoBehaviour, GridObject
    {
        public TileMap.ObjectType type;


        public TileMap.ObjectType GetGridObjType()
        {
            return type;
        }

        public void SetLocation(int x, int y)
        {
        }
    }
}

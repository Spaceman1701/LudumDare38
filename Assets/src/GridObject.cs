using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace src
{
    public interface GridObject
    {
        void SetLocation(int x, int y);
        TileMap.ObjectType GetGridObjType();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace src
{
    public class GridLocation
    {
        public int x;
        public int y;

        public GridLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public GridLocation() : this(0, 0)
        {

        }
    }
}



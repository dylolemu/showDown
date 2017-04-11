using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace showDown
{
    class Ammo
    {
        public int x, y, size, time, ogSize;

        public Ammo (int _x, int _y, int _size, int _time, int _ogSize)
        {
            x = _x;
            y = _y;
            size = _size;
            time = _time;
            ogSize = _ogSize;
        }
    }
}

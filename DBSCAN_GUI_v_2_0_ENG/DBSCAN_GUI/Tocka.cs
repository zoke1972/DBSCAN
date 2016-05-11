using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBSCAN_GUI
{
    public class Tocka
    {
        public int X, Y, C;
        public Tocka(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return String.Format(" ({0}, {1})", X, Y);
        }
    }
}

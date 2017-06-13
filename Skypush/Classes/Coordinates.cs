using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skypush.Classes
{
    /// <summary>
    /// For storing locations in pixels
    /// </summary>
    class ScreenLocation
    {
        public int Horizontal { get; set; }
        public int Vertical { get; set; }

        public ScreenLocation(int horizontal, int vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}

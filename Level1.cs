using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Level1 : Level
    {
        public Level1()
        {
            player = new Player(500, 50, 50, 150);
            platforms = new List<Platform> ();
            barrier = new List<SFML.Graphics.FloatRect> ();
            AddPlatform(0, 250, 1300, 50);
            AddPlatform(0, 500, 1300, 50);
            AddPlatform(0, 750, 1300, 50);
        }
    }
}

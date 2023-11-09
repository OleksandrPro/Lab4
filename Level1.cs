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
            player = new Player(500, 150);
            platforms = new List<Platform> ();
            barrier = new List<SFML.Graphics.FloatRect> ();
            AddPlatform(0, 350, 1300, 50);
            AddPlatform(0, 600, 1300, 50);
            AddPlatform(0, 850, 1300, 50);
        }
    }
}

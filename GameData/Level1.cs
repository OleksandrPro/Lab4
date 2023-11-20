using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab4
{
    public class Level1 : ILevel
    {
        public Level1()
        {
            Player = new Player(500, 150);
            Platforms = new List<Platform> ();
            Barrier = new List<SFML.Graphics.FloatRect> ();
            AddPlatform(0, 350, 1300, 50);
            AddPlatform(0, 600, 1300, 50);
            AddPlatform(0, 850, 1300, 50);
        }
        private Player _player;
        public Player Player
        {
            get { return _player; }
            private set { _player = value; }
        }
        private List<Platform> _platforms;
        public List<Platform> Platforms
        { 
            get { return _platforms; } 
            private set { _platforms = value; }
        }
        private List<FloatRect> _barrier;
        public List<FloatRect> Barrier
        { 
            get { return _barrier; } 
            private set { _barrier = value; }
        }
        public void AddPlatform(int x, int y, int height, int width)        
        {
            Platforms.Add(new Platform(x, y, height, width));
        }
        public void AddBarrier(int x, int y, int height, int width)
        {
            Barrier.Add(new FloatRect(x, y, height, width));
        }
    }
}

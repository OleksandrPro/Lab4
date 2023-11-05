using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Platform
    {
        public uint X {  get; set; }
        public uint Y { get; set; }
        public uint Height { get; set; }
        public uint Width { get; set; }
        public FloatRect Collider { get; set; }
        public Platform(uint x, uint y, uint height, uint width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
        public Platform() : this(0,0,50, 1280) { }
    }
}

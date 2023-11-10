namespace Lab4
{
    public class Platform : GameObject
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
//        public FloatRect Collider { get; set; }
        public Platform(int x, int y, int height, int width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
        public Platform() : this(0,0,50, 1280) { }
    }
}

namespace Lab4
{
    public class FallingObject : GameObject
    {
        private int _x;
        private int _y;
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    Collider = Engine.ChangeColliderPositionX(Collider, _x);
                }
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    Collider = Engine.ChangeColliderPositionY(Collider, _y);
                }
            }
        }
        public FallingObject(int x, int y)
        {
            X = x;
            Y = y;
        }
        public FallingObject() : this(0, 0) { }
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            UpdateColliderPosition();
        }
        public void IncreaseVerticalSpeed(int speedAmount) 
        {
            Y += speedAmount;
            Collider = Engine.ChangeColliderPositionY(Collider, Collider.Top + Y);
        }
        public void UpdateColliderPosition()
        {
            Collider = Engine.ChangeColliderPosition(Collider, X, Y);
        }
    }
}

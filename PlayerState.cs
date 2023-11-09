namespace Lab4
{
    public abstract class PlayerState
    {
        public abstract int MovementCoeffcientX { get; }
        
        public abstract void Move();
        public abstract void BackToIdle();
    }
}

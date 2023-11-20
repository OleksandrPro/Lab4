namespace Lab4
{
    public interface IPlayerState
    {
        int MovementCoeffcientX { get; }        
        void MoveHorizontal();
        void BackToIdle();
    }
}

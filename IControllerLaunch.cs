namespace Lab4
{
    public interface IControllerLaunch
    {
        bool IsNotGameOver { get; set; }
        void MovementHandler();
        void RenderLevel();
        void Update();
    }
}

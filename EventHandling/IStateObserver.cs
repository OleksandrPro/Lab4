namespace Lab4
{
    public interface IStateObserver
    {
        void Update(IStateUpdate subject);
    }
}

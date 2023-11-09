namespace Lab4
{
    public interface IStateUpdate
    {
        void Attach(IStateObserver observer);
        void Detach(IStateObserver observer);
        void StateEventNotify();
    }
}

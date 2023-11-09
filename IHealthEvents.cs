namespace Lab4
{
    public interface IHealthEvents
    {
        void Attach(IHealthEventObserver observer);
        void Detach(IHealthEventObserver observer);
        void HealthEventNotify();
    }
}

namespace Lab4
{
    public interface IDied
    {
        void Attach(IDeathObserver observer);
        void Detach(IDeathObserver observer);
        void DiedNotify();
    }
}

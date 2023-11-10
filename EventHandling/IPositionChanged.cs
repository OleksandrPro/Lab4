namespace Lab4
{
    public interface IPositionChanged
    {
        void Attach(IPositionChangeObserver observer);
        void Detach(IPositionChangeObserver observer);
        void PositionChangeNotify();
    }
}

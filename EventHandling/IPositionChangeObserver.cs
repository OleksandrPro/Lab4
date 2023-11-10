namespace Lab4
{
    public interface IPositionChangeObserver
    {
        void Update(IPositionChanged subject);
    }
}

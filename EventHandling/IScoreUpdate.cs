namespace Lab4
{
    public interface IScoreUpdate
    {
        void Attach(IScoreUpdateObserver observer);
        void Detach(IScoreUpdateObserver observer);
        void ScoreUpdateNotify();
    }
}

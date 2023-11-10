namespace Lab4
{
    public interface IDespawnObject
    {
        void Attach(IDespawnObjectObserver observer);
        void Detach(IDespawnObjectObserver observer);
        void DespawnObjectNotify(Model.FallingObjectTypes type, FallingObject newFObj);
    }
}

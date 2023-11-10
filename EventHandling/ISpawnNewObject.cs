namespace Lab4
{
    public interface ISpawnNewObject
    {
        void Attach(ISpawnNewObjectObserver observer);
        void Detach(ISpawnNewObjectObserver observer);
        void SpawnNewObjectNotify(Model.FallingObjectTypes type, FallingObject newFObj);
    }
}

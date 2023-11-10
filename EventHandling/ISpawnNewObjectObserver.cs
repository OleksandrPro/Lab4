namespace Lab4
{
    public interface ISpawnNewObjectObserver
    {
        void UpdateSpawn(Model.FallingObjectTypes type, FallingObject fobj);
    }
}

namespace Lab4
{
    public interface IDespawnObjectObserver
    {
        void UpdateDespawn(Model.FallingObjectTypes type, FallingObject fobj);
    }
}

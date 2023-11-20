using System.Collections.Generic;
using static Lab4.Model;

namespace Lab4
{
    public interface IModel
    {
        ObjectPool<FallingObject> FallingDamageObjects { get; }
        ObjectPool<FallingObject> FallingScoreObjects { get; }
        List<FallingObject> SpawnedDamageObjects { get; }
        List<FallingObject> SpawnedScoreObjects { get; }
        int Score { get; }
        ILevel CurrentLevel { get; set; }
        void AddController(Controller controller);
        void SpawnFallingObject(FallingObjectTypes type, int x, int y);
        void DespawnFallingObject(FallingObjectTypes type, FallingObject obj);
        void AddScore();
    }
}

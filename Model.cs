using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Model
    {
        private Controller _controller;


        public Level currentLevel;
        private ObjectPool<FallingObject> _fallingObjects;
        private ObjectPool<HealingItem> _healingItems;
        public List<FallingObject> SpawnedObjects;

        public Model()
        {
            currentLevel = new Level1();
            _fallingObjects = new ObjectPool<FallingObject>(1);
            _healingItems = new ObjectPool<HealingItem>(1, 1);
            SpawnedObjects = new List<FallingObject>();
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        public FallingObject SpawnFallingObject(int x, int y)
        {
//            Console.WriteLine("Spawning FallingObject");
            FallingObject newFObj = _fallingObjects.Get();
            newFObj.SetPosition(x, y);
            newFObj.SetActive(true);
            SpawnedObjects.Add(newFObj);

//            Console.WriteLine("Spawned objects: " + SpawnedObjects.Count);
            return newFObj;
        }
        public void DespawnFallingObject(FallingObject obj)
        {
//            Console.WriteLine("Despawning FallingObject");
            FallingObject toRemove = SpawnedObjects.FirstOrDefault(remove => remove.X == obj.X && remove.Y == obj.Y);
            SpawnedObjects.Remove(toRemove);
            obj.SetPosition(0, 0);            
            _fallingObjects.Release(toRemove);
//            Console.WriteLine("Spawned objects: " + SpawnedObjects.Count);
        }
        void SetLevel(Level level) 
        {
            currentLevel = level;
        }
    }
}

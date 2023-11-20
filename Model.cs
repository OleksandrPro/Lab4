using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class Model : IModel, IScoreUpdate, ISpawnNewObject, IDespawnObject
    {
        private Controller _controller;

        public ILevel CurrentLevel { get; set; }
        private ObjectPool<FallingObject> _fallingDamageObjects;
        private ObjectPool<FallingObject> _fallingScoreObjects;
        public ObjectPool<FallingObject> FallingDamageObjects 
        { 
            get { return _fallingDamageObjects; }
            private set { _fallingDamageObjects = value; }
        }
        public ObjectPool<FallingObject> FallingScoreObjects 
        { 
            get { return _fallingScoreObjects; }
            private set { _fallingScoreObjects = value; }
        }
        public List<FallingObject> SpawnedDamageObjects { get; private set; }
        public List<FallingObject> SpawnedScoreObjects { get; private set; }
        private List<ISpawnNewObjectObserver> _spawnNewObjectObservers = new List<ISpawnNewObjectObserver>();
        private List<IDespawnObjectObserver> _despawnNewObjectObservers = new List<IDespawnObjectObserver>();
        private List<IScoreUpdateObserver> _scoreUpdateObservers = new List<IScoreUpdateObserver>();

        public const int HORIZONTAL_UNIT_SIZE = 10;
        public const int VERTICAL_UNIT_SIZE = 250;
        public const int OBJECT_DAMAGE = 1;
        public const int PLAYER_START_HEALTH = 3;
        public const int OBJECT_FALLING_SPEED = 2;
        public const int FALLING_OBJECT_SPAWN_TIME = 2784;
        public const int INITIAL_NUMBER_OF_FALLING_OBJECTS = 8;
        public const int INITIAL_NUMBER_OF_FALLING_SCORE_OBJECTS = 15;
        public const int POINTS_PER_ITEM = 10;
        public const int FALLING_SCORE_OBJECT_SPAWN_TIME = 1200;

        private int _score;
        public int Score 
        { 
            get
            {
                return _score;
            }
            private set
            {
                if (value != _score) 
                {
                    _score = value;
                    ScoreUpdateNotify();
                }
            }
        }
        public Model()
        {
            CurrentLevel = new Level1();
            _fallingDamageObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_OBJECTS);
            _fallingScoreObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_SCORE_OBJECTS);
            SpawnedDamageObjects = new List<FallingObject>();
            SpawnedScoreObjects = new List<FallingObject>();
            Score = 0;
        }
        public enum FallingObjectTypes
        {
            DamageObject, ScoreObject
        }
        public void Attach(ISpawnNewObjectObserver observer)
        {
            _spawnNewObjectObservers.Add(observer);
        }
        public void Detach(ISpawnNewObjectObserver observer)
        {
            _spawnNewObjectObservers.Remove(observer);
        }
        public void SpawnNewObjectNotify(Model.FallingObjectTypes type, FallingObject newFObj)
        {
            foreach (var observer in _spawnNewObjectObservers)
            {
                observer.UpdateSpawn(type, newFObj);
            }
        }
        public void Attach(IScoreUpdateObserver observer)
        {
            _scoreUpdateObservers.Add(observer);
        }
        public void Detach(IScoreUpdateObserver observer)
        {
            _scoreUpdateObservers.Remove(observer);
        }
        public void ScoreUpdateNotify()
        {
            foreach (var observer in _scoreUpdateObservers)
            {
                observer.Update(this);
            }
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        private FallingObject TemplateSpawn(ObjectPool<FallingObject> pool, int x, int y, List<FallingObject> result)
        {
            FallingObject newFObj = pool.Get();
            newFObj.SetPosition(x, y);
            newFObj.SetActive(true);
            result.Add(newFObj);
            return newFObj;
        }
        private void TemplateDespawn(FallingObject obj, ObjectPool<FallingObject> pool, List<FallingObject> list)
        {
            FallingObject toRemove = list.FirstOrDefault(remove => remove.X == obj.X && remove.Y == obj.Y);
            list.Remove(toRemove);
            obj.SetPosition(0, 0);
            pool.Release(toRemove);
        }
        public void SpawnFallingObject(FallingObjectTypes type, int x, int y)
        {
            if (type == FallingObjectTypes.DamageObject)
                SpawnDamageObject(x, y);
            if (type == FallingObjectTypes.ScoreObject)
                SpawnScoreObject(x, y);
        }
        private void SpawnDamageObject(int x, int y)
        {
            FallingObject newFObj = TemplateSpawn(_fallingDamageObjects, x, y, SpawnedDamageObjects);
            SpawnNewObjectNotify(FallingObjectTypes.DamageObject, newFObj);
        }
        private void SpawnScoreObject(int x, int y)
        {
            FallingObject newFObj = TemplateSpawn(_fallingScoreObjects, x, y, SpawnedScoreObjects);
            SpawnNewObjectNotify(FallingObjectTypes.ScoreObject, newFObj);
        }
        public void DespawnFallingObject(FallingObjectTypes type, FallingObject obj)
        {
            if (type == FallingObjectTypes.DamageObject)
                DespawnDamageObject(obj);
            if (type == FallingObjectTypes.ScoreObject)
                DespawnScoreObject(obj);
        }
        private void DespawnDamageObject(FallingObject obj)
        {
            DespawnObjectNotify(FallingObjectTypes.DamageObject, obj);
            TemplateDespawn(obj, _fallingDamageObjects, SpawnedDamageObjects);            
        }       
        private void DespawnScoreObject(FallingObject obj)
        {
            DespawnObjectNotify(FallingObjectTypes.ScoreObject, obj);
            TemplateDespawn(obj, _fallingScoreObjects, SpawnedScoreObjects);            
        }
        public void AddScore()
        {
            Score += POINTS_PER_ITEM;
        }
        public void Attach(IDespawnObjectObserver observer)
        {
            _despawnNewObjectObservers.Add(observer);
        }
        public void Detach(IDespawnObjectObserver observer)
        {
            _despawnNewObjectObservers.Remove(observer);
        }
        public void DespawnObjectNotify(Model.FallingObjectTypes type, FallingObject newFObj)
        {
            foreach (var observer in _despawnNewObjectObservers)
            {
                observer.UpdateDespawn(type, newFObj);
            }
        }
    }
}

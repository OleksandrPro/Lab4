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
        private ObjectPool<FallingObject> _fallingScoreObjects;
        public List<FallingObject> SpawnedObjects;
        public List<FallingObject> SpawnedScoreObjects;

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
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Model()
        {
            currentLevel = new Level1();
            _fallingObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_OBJECTS);
            _fallingScoreObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_SCORE_OBJECTS);
            SpawnedObjects = new List<FallingObject>();
            SpawnedScoreObjects = new List<FallingObject>();
            Score = 0;
        }
        public delegate void ScoreChange(object sender, EventArgs e);
        public event ScoreChange ScoreChanged;
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
        public FallingObject SpawnFallingObject(int x, int y)
        {
            return TemplateSpawn(_fallingObjects, x, y, SpawnedObjects);
        }
        public void DespawnFallingObject(FallingObject obj)
        {
            TemplateDespawn(obj, _fallingObjects, SpawnedObjects);
        }        
        public FallingObject SpawnFallingScoreObject(int x, int y)
        {
            return TemplateSpawn(_fallingScoreObjects, x, y, SpawnedScoreObjects);
        }
        public void DespawnFallingScoreObject(FallingObject obj)
        {
            TemplateDespawn(obj, _fallingScoreObjects, SpawnedScoreObjects);
        }
        public void AddScore()
        {
            Score += POINTS_PER_ITEM;
        }
    }
}

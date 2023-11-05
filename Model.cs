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

        public Model()
        {
            currentLevel = new Level1();
            _fallingObjects = new ObjectPool<FallingObject>(6);
            _healingItems = new ObjectPool<HealingItem>(1, 1);
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        void SetLevel(Level level) 
        {
            currentLevel = level;
        }
    }
}

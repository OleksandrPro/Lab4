using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class FallingObjectManager
    {
        private ObjectPool<FallingObject> _allObjects;
        public FallingObjectManager(int initialNumberOfObjects)
        {
            _allObjects = new ObjectPool<FallingObject> (initialNumberOfObjects);
        }
        public FallingObjectManager() : this(0) { } 
    }
}

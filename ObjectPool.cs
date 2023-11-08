using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ObjectPool<T> where T : GameObject, new()
    {
        private List<T> _objects;
        private int _maxAmountOfObjects;
        public ObjectPool(int initialNumberOfObjects) 
        {
            if (initialNumberOfObjects < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _objects = new List<T>();
            for (int i = 0; i < initialNumberOfObjects; i++)
            {
                T obj = Create();
                obj.SetActive(false);
                _objects.Add(obj);
            }
        }
        public T Get()
        {
            var obj = _objects.FirstOrDefault(x=>!x.IsActive);
            if (obj == null)
            {
                obj = Create();
            }
            return obj;
        }
        private T Create()
        {            
            T obj = new T();            
            _objects.Add(obj);
            return obj;
        }
        public void Release(T obj) 
        {
            obj.SetActive(false);
            obj.Collider = GameObject.DefaultCollider;
        }        
    }
}

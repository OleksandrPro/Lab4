using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public abstract class GameObject
    {
        public bool IsActive {  get; set; }
        public FloatRect Collider { get; set; }
        protected FloatRect DefaultCollider { get; private set; }
        public GameObject() 
        {
            DefaultCollider = new FloatRect();
        }
        public void SetActive(bool b)
        {
            IsActive = b;
        }
    }
}

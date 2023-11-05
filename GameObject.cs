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
        public GameObject() { }
        public void SetActive(bool b)
        {
            IsActive = b;
        }
    }
}

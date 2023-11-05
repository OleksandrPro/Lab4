using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class HealingItem : GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int HealthValue {  get; set; }

        public HealingItem(int x, int y, int healthValue)
        {
            X = x;
            Y = y;
            HealthValue = healthValue;
        }
        public HealingItem() : this(0, 0, 1) { }
    }
}

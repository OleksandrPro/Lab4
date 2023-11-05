using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class HealingItemManager
    {
        private ObjectPool<HealingItem> _healingItemPool;
    
        public HealingItemManager()
        {
            _healingItemPool = new ObjectPool<HealingItem>(1);
        }
    }
}

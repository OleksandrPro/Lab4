using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public interface IControllerLaunch
    {
        bool IsNotGameOver { get; set; }
        void MovementHandler();
        void RenderLevel();
        void Update();
        void ShowFinalResult();
    }
}

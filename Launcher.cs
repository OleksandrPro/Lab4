using SFML.Graphics;
using SFML.Window;
using System;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class Launcher
    {
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        static void Main()
        {
            RenderWindow MainWindow = new RenderWindow(new VideoMode(1300, 800), "Yonko's adventures");
//            RenderWindow MainWindow = new RenderWindow(new VideoMode(2000, 1500), "Yonko's adventures");
            MainWindow.SetFramerateLimit(30);
            MainWindow.Closed += new EventHandler(OnClose);
            Controller controller = new Controller(MainWindow);

            while (MainWindow.IsOpen)
            {
                MainWindow.DispatchEvents();                
                MainWindow.Clear(new Color(Color.White));
                MainWindow.Draw(controller.UIElements.CurrentPlayerModel);
                foreach (var p in controller.UIElements.Platforms)
                {
                    MainWindow.Draw(p);
                }
//                controller.UIElements.PlayIdleAnimation();
//                controller.Update();
                //                Controller.UIElements.PlayerAnimator();
                //                Controller.player.increaseVerticalSpeed(1);
                MainWindow.Display();
            }
        }

    }
}

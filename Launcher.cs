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
            RenderWindow MainWindow = new RenderWindow(new VideoMode(1300, 1000), "Yonko's adventures");
            MainWindow.SetFramerateLimit(60);
            MainWindow.Closed += new EventHandler(OnClose);

            View visual = new View(MainWindow);
            Model model = new Model();
            Controller controller = new Controller(visual, model);

            visual.AddController(controller);
            model.AddController(controller);
            controller.RenderLevel();            

            while (MainWindow.IsOpen)
            {
                if (!controller.IsNotGameOver)
                {
                    break;
                }
                MainWindow.DispatchEvents();
                controller.MovementHandler();
                controller.Update();
                visual.DrawScene();
            }
            while (MainWindow.IsOpen)
            {
                controller.ShowFinalResult();
            }
            //while (MainWindow.IsOpen)
            //{
            //    //second way
            //    if (controller.IsNotGameOver)
            //    {
            //        MainWindow.DispatchEvents();
            //        controller.MovementHandler();
            //    }
            //    if (!controller.IsNotGameOver)
            //    {
            //        controller.ShowFinalResult();
            //    }
            //    else
            //    {
            //        controller.Update();
            //        visual.DrawScene();
            //    }

            //}
        }
    }
}

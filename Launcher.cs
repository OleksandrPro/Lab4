using SFML.Graphics;
using SFML.Window;
using System;

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
            RenderWindow MainWindow = new RenderWindow(new VideoMode(1300, 1000), "Yonko Adventures");
            MainWindow.SetFramerateLimit(60);
            MainWindow.Closed += new EventHandler(OnClose);

            View visual = new View(MainWindow);
            Model model = new Model();
            IControllerLaunch controller = new Controller(visual, model);

            visual.AddController((Controller)controller);
            model.AddController((Controller)controller);
            controller.RenderLevel();            

            while (MainWindow.IsOpen)
            {                
                MainWindow.DispatchEvents();
                controller.MovementHandler();
                controller.Update();
                visual.DrawScene();
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

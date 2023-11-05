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
            bool isAKeyPressed = false;
            bool isDKeyPressed = false;
            void OnKeyReleased(object sender, EventArgs e)
            {
                if (((KeyEventArgs)e).Code == Keyboard.Key.A)
                {
                    isAKeyPressed = false;
                }

                if (((KeyEventArgs)e).Code == Keyboard.Key.D)
                {
                    isDKeyPressed = false;
                }
            }
            void OnKeyPressed(object sender, EventArgs e)
            {
                if (((KeyEventArgs)e).Code == Keyboard.Key.A)
                {
                    isAKeyPressed = true;
                }

                if (((KeyEventArgs)e).Code == Keyboard.Key.D)
                {
                    isDKeyPressed = true;
                }
            }

            RenderWindow MainWindow = new RenderWindow(new VideoMode(1300, 800), "Yonko's adventures");
            MainWindow.SetFramerateLimit(60);
            MainWindow.Closed += new EventHandler(OnClose);

            View visual = new View(MainWindow);
            Model model = new Model();
            Controller controller = new Controller(visual, model);

            visual.AddController(controller);
            model.AddController(controller);
            controller.RenderLevel();

            MainWindow.KeyPressed += OnKeyPressed;
            MainWindow.KeyReleased += OnKeyReleased;

            while (MainWindow.IsOpen)
            {
                MainWindow.DispatchEvents();
                if (isAKeyPressed)
                {
                    controller.MovePlayer(-10, 0);
                }

                if (isDKeyPressed)
                {
                    controller.MovePlayer(10, 0);
                }
//                controller.Update();
                visual.DrawScene();
            }
            
        }

    }
}

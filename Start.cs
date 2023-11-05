using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class Start
    {
        static void Main1(string[] args)
        {
            void OnCloseHandler(object sender, EventArgs e)
            {
                
                // Close the window when OnClose event is received
                RenderWindow window = (RenderWindow)sender;
                window.Close();
            }
            void OnKeyPressed(object sender, EventArgs e)
            {
                KeyEventArgs kargs = (KeyEventArgs)e;
                if (kargs.Code == Keyboard.Key.W)
                {
                    Console.WriteLine("wwwwwwwww");
                }
            }
            RenderWindow MainWindow = new RenderWindow(new VideoMode(1000, 1000), "title");
            MainWindow.Closed += new EventHandler(OnCloseHandler);
            MainWindow.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            while (MainWindow.IsOpen)
            {
                MainWindow.DispatchEvents();
                MainWindow.Clear(new Color(Color.Black));

                //foreach (var item in ItemsToRender)
                //{
                //    MainWindow.Draw(item);
                //}
                MainWindow.Display();
            }
        }
    }
}

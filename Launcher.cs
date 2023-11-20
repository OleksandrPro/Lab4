using SFML.Graphics;
using SFML.Window;
using System;

namespace Lab4
{
    public class Launcher : ILauncher
    {
        public RenderWindow MainWindow { get; set; }
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        public Launcher()
        {

        }
        public void Run()
        {
            ConfigureWindow();

            IView visual = new View(MainWindow);
            IModel model = new Model();
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
        }
        public void ConfigureWindow()
        {
            MainWindow = new RenderWindow(new VideoMode(1300, 1000), "Yonko Adventures");
            MainWindow.SetFramerateLimit(60);
            MainWindow.Closed += new EventHandler(OnClose);
        }
    }
}

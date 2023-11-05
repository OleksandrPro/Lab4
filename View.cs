using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class View
    {
        public RenderWindow GameWindow { get; set; }
        private Controller _controller;

        public Sprite CurrentPlayerModel;
        private List<RectangleShape> Platforms;
        private LinkedList<Sprite> _idleAnimationData;

        private string _folderPathNumbers = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\numbers";
        private string _folderPathSingleSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\newPlaceholder.png";
        public View(RenderWindow window)
        {
            GameWindow = window;

            Platforms = new List<RectangleShape>();
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        public void DrawScene()
        {
            GameWindow.Clear(new Color(Color.White));

            GameWindow.Draw(CurrentPlayerModel);
            foreach (var p in Platforms)
            {
                GameWindow.Draw(p);
            }

            GameWindow.Display();
        }
        public void LoadLevel(Level level) 
        {
            AddPlatforms(level);
            AddPlayerModel(level.player);
            SetBarrier(level);
        }
        private void AddPlatforms(Level level)
        {
            foreach (var p in level.platforms)
            {
                RectangleShape newPlatform = new RectangleShape(new Vector2f(p.Height, p.Width));
                newPlatform.Position = new Vector2f(p.X, p.Y);
                newPlatform.FillColor = Color.Cyan;
                Platforms.Add(newPlatform);
                _controller.AddPlatformCollider(p, newPlatform.GetGlobalBounds());
            }
        }
        private void AddPlayerModel(Player player)
        {
            Texture model = new Texture(_folderPathSingleSprite);
            CurrentPlayerModel = new Sprite(model);
            CurrentPlayerModel.Position = new Vector2f(player.X, player.Y);
        }
        private void SetBarrier(Level level)
        {
            int barrierSize = 300;
            uint windowX = GameWindow.Size.X;
            uint windowY = GameWindow.Size.Y;
            level.barrier.Add(new FloatRect(0, -barrierSize, windowX, barrierSize));
            level.barrier.Add(new FloatRect(0, windowY, windowX, barrierSize));
            level.barrier.Add(new FloatRect(-barrierSize, 0, barrierSize, windowY));
            level.barrier.Add(new FloatRect(windowX, 0, barrierSize, windowY));
            //xywh
        }
        public void UpdateModelPosition(int x, int y)
        {
            CurrentPlayerModel.Position = new Vector2f(x, y);
        }
    }
}

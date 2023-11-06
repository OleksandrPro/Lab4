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

        private List<Sprite> FallingObjects;

        private LinkedList<Sprite> _idleAnimationData;

        private string _folderPathNumbers = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\numbers";
        private string _folderPathSingleSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\newPlaceholder.png";
        private string _folderPathFireBallSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\fireball2.png";
        public View(RenderWindow window)
        {
            GameWindow = window;

            Platforms = new List<RectangleShape>();
            FallingObjects = new List<Sprite>();
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
            foreach (var g in FallingObjects)
            {
                GameWindow.Draw(g);
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
            _controller.AddPlayerCollider();
        }
        private void SetBarrier(Level level)
        {
            int barrierSize = 300;
            int windowX = (int)GameWindow.Size.X;
            int windowY = (int)GameWindow.Size.Y;
            _controller.AddBarrier(0, -barrierSize, windowX, barrierSize);
            _controller.AddBarrier(0, windowY, windowX, barrierSize);
            _controller.AddBarrier(-barrierSize, 0, barrierSize, windowY);
            _controller.AddBarrier(windowX, 0, barrierSize, windowY);
            //xywh
        }
        public void AddFallingObject(FallingObject fObj)
        {
            Texture model = new Texture(_folderPathFireBallSprite);
            Sprite newFObjSprite = new Sprite(model);
            newFObjSprite.Position = new Vector2f(fObj.X, fObj.Y);
            FallingObjects.Add(newFObjSprite);
            _controller.AddFallingObjectCollider(fObj, newFObjSprite.GetGlobalBounds());
        }
        public void UpdateModelPosition(int x, int y)
        {
            CurrentPlayerModel.Position = new Vector2f(x, y);
        }
        public void UpdateFallingObjectPosition(int y)
        {
            Sprite s = null;
            foreach (var item in FallingObjects)
            {
                item.Position = new Vector2f(item.Position.X, item.Position.Y + y);
                if (item.Position.Y >= GameWindow.Size.Y)
                {
                    s = item;
                }
            }
            if (s != null)
            {
                FallingObjects.Remove(s);
            }
        }
    }
}

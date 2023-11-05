using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class UI
    {
        public Window MainWindow { get; set; }
        public Sprite CurrentPlayerModel { get; set; }
        private Controller _controller;
        public List<RectangleShape> Platforms { get; set; }
        private LinkedList<Sprite> _idleAnimationData;
        private string _folderPath = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\testImg";
        private string _folderPathNumbers = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\numbers";
        public UI(Window window) 
        { 
            Platforms = new List<RectangleShape>();
            MainWindow = window;
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        public void AddPlayerModel(Player player)
        {
            Texture model = new Texture("D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\newPlaceholder.png");
            //            Texture model = new Texture("Lab4\\sprites\\placeholder.png");
            CurrentPlayerModel = new Sprite(model);
            CurrentPlayerModel.Position = new Vector2f(player.X, player.Y);
        }
        public void AddIdlePlayerModel(Player player)
        {
            _idleAnimationData = new LinkedList<Sprite>();
            AddSprites(player, _idleAnimationData, _folderPathNumbers);
        }
        void AddSprites(Player player, LinkedList<Sprite> sprites, string path)
        {           
            if (Directory.Exists(path))
            {
                string[] imageFiles = Directory.GetFiles(path, "*.png");
                foreach (string imagePath in imageFiles)
                {
                    Texture texture = new Texture(imagePath);
                    Sprite newSprite = new Sprite(texture);
                    newSprite.Position = new Vector2f(player.X, player.Y);
                    sprites.Add(newSprite);
                }
            }
            else
            {
                Console.WriteLine("Folder doesn't exist");
            }
            CurrentPlayerModel = _idleAnimationData.GetCurrent();
            CurrentPlayerModel.Position = new Vector2f(player.X, player.Y);
        }
        public void AddPlatforms(Level level)
        {   
            foreach (var p in level.platforms)
            {
                RectangleShape newPlatform = new RectangleShape(new Vector2f(p.Height, p.Width));
                newPlatform.Position = new Vector2f(p.X, p.Y);
                newPlatform.FillColor = Color.Cyan;
                Platforms.Add(newPlatform);
//                _controller.AddPlatformCollider(p, newPlatform.GetGlobalBounds());
            }            
        }
        private void SetBarrier(Level level)
        {
            int barrierSize = 300;
            uint windowX = MainWindow.Size.X;
            uint windowY = MainWindow.Size.Y;
            level.barrier.Add(new FloatRect(0, -barrierSize, windowX, barrierSize));
            level.barrier.Add(new FloatRect(0, windowY, windowX, barrierSize));
            level.barrier.Add(new FloatRect(-barrierSize, 0, barrierSize, windowY));
            level.barrier.Add(new FloatRect(windowX, 0, barrierSize, windowY));
            //xywh
        }
        public void LoadLevel(Level level)
        {
            AddPlatforms(level);
            SetBarrier(level);
            AddPlayerModel(level.player);
 //           AddIdlePlayerModel(level.player);            
        }
        public void PlayIdleAnimation()
        {
            _idleAnimationData.MoveToNext();
            CurrentPlayerModel = _idleAnimationData.GetCurrent();
//            CurrentPlayerModel.Position = _controller.GetPlayerPosition();
        }
        //public void PlayerAnimator()
        //{
        //    float x = PlayerModel.Position.X;
        //    float y = PlayerModel.Position.Y;
        //    if (PlayerModel.Radius == 0) 
        //    {
        //        PlayerModel.Radius = startRadius;
        //        PlayerModel.Position = new Vector2f(x-startRadius, y-startRadius);
        //    }
        //    else
        //    {
        //        PlayerModel.Radius = --(PlayerModel.Radius);
        //        PlayerModel.Position = new Vector2f(++x, ++y);
        //    }
        //}
        void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        public void UpdateModelPosition(int x, int y) 
        {
            CurrentPlayerModel.Position = new Vector2f(x, y);
        }
        public void AddInputHandler(EventHandler<KeyEventArgs> handler)
        {
            MainWindow.KeyPressed += handler;
        }
    }
}

using SFML.Graphics;
using SFML.System;

namespace Lab4
{
    public class UI : IHealthEventObserver, IScoreUpdateObserver, IDeathObserver
    {
        private RenderWindow _window;
        private Font _font;
        private const string _folderFontPath = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\font\\FFFFORWA.ttf";
        private const string _folderHPPath = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\heart.png";
        private Text _health;
        private Text _score;
        private RectangleShape _bar;
        private const int TEXT_SIZE = 50;
        private Sprite _HPIcon;

        private const int BAR_HEIGHT = 100;
        private const int BAR_WIDTH = 1300;
        private const bool NEED_BAR = false;
        public UI(RenderWindow window)
        {
            _window = window;
            _font = new Font(_folderFontPath);
            _health = new Text();
            SetText(_health, Model.PLAYER_START_HEALTH.ToString());
            SetTextDefaultSettings(_health);
            _health.Position = new Vector2f(2 * TEXT_SIZE, window.Size.Y - TEXT_SIZE - TEXT_SIZE / 4);
            _score = new Text();
            SetText(_score, "Score : 0");
            SetTextDefaultSettings(_score);
            _score.Position = new Vector2f(window.Size.X / 2, window.Size.Y - TEXT_SIZE - TEXT_SIZE / 4);
            if (NEED_BAR)
                AddBar();
            AddHPIcon();
        }
        public void Draw()
        {
            if (NEED_BAR)
                _window.Draw(_bar);
            _window.Draw(_health);
            _window.Draw(_HPIcon);
            _window.Draw(_score);
        }
        private void AddBar()
        {
            _bar = new RectangleShape(new Vector2f(BAR_WIDTH, BAR_HEIGHT));
            _bar.Position = new Vector2f(0, _window.Size.Y - BAR_HEIGHT);
            _bar.FillColor = Color.Red;
        }
        private void AddHPIcon()
        {
            Texture model = new Texture(_folderHPPath);
            _HPIcon = new Sprite(model);
            _HPIcon.Position = new Vector2f(0, _window.Size.Y - _HPIcon.Texture.Size.Y);
        }
        private void SetText(Text text, string newString)
        {
            text.DisplayedString = newString;
        }
        private void SetTextDefaultSettings(Text text)
        {
            text.Font = _font;
            text.CharacterSize = TEXT_SIZE;
        }        
        public void UpdateHealth(int newHealth)
        {
            SetText(_health, newHealth.ToString());
        }
        public void CreateEndGameScreen()
        {
            _window.Clear(Color.Black);
            Text finalText = new Text();
            SetText(finalText, "Game Over!");
            SetTextDefaultSettings(finalText);
            finalText.Position = new Vector2f(_window.Size.X / 3, _window.Size.Y / 3);
            _score.Position = new Vector2f(finalText.Position.X, finalText.Position.Y + TEXT_SIZE * 2);
//            _window.Draw(finalText);
            _window.Draw(_score);
            _window.Display();
        }
        public void Update(IHealthEvents subject)
        {
            Player player = subject as Player;
            SetText(_health, player.Health.ToString());
        }

        public void Update(IScoreUpdate subject)
        {
            Model model = subject as Model;
            SetText(_score, "Score : " + model.Score.ToString());
        }
        public void Update(IDied subject)
        {
            Player player = subject as Player;
            CreateEndGameScreen();
        }
    }
}

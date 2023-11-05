using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab4.UI;

namespace Lab4
{
    public class Controller
    {
        View _view;
        Model _model;

        private Random _random;
        private Player _player;
        private Level _currentLevel;

        private Dictionary<Keyboard.Key, DirectionCoeff> MovementMap;
        private const int UNIT_SIZE = 50;
        private int _playerPrevX;

        private struct DirectionCoeff
        {
            public int _x, _y;
            public DirectionCoeff(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }

        public Controller(View viev, Model model)
        {
            _view = viev;
            _model = model;
            viev.GameWindow.KeyPressed += OnKeyPressed;
//            viev.GameWindow.KeyReleased += OnKeyReleased;

            _random = new Random();
            MovementMap = new Dictionary<Keyboard.Key, DirectionCoeff>();
            FillMovementMap();

            _player = model.currentLevel.player;
            _currentLevel = model.currentLevel;
            _playerPrevX = _player.X;

            _player.NewPosition += UpdatePlayerPos;
            _player.NewPosition += UpdateColliderPosition;
        }
        void OnKeyPressed(object sender, EventArgs e)
        {
            KeyEventArgs kargs = (KeyEventArgs)e;
            foreach (var item in MovementMap)
            {
                if (item.Key == kargs.Code)
                {
                    int updateX = item.Value._x * UNIT_SIZE;
                    int updateY = item.Value._y * UNIT_SIZE;
                    bool possibleCollision = PreUpdate(_player, updateX, updateY);
                    if (possibleCollision)
                    {
                        return;
                    }
                    _player.BackToMoving();
                    MovePlayer(updateX, updateY);
                }
            }
        }
        void UpdatePlayerPos(object sender, EventArgs e)
        {
            ChangePositionEventArgs changepos = (ChangePositionEventArgs)e;
            _view.UpdateModelPosition(changepos.X, changepos.Y);
        }
        void UpdateColliderPosition(object sender, EventArgs e)
        {
            Player player = (Player)sender;
            player.Collider = GetColiderOfModel();
        }
        public FloatRect GetColiderOfModel()
        {
            return _view.CurrentPlayerModel.GetGlobalBounds();
        }
        private void FillMovementMap()
        {
            //            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -1));
            //            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 1));
            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -5));
            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 5));
            //            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -24));
            //            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 24));
            MovementMap.Add(Keyboard.Key.A, new DirectionCoeff(-1, 0));
            MovementMap.Add(Keyboard.Key.D, new DirectionCoeff(1, 0));

        }
        public void MovePlayer(int x, int y)
        {
            _player.Move(x, y);
            
            Console.WriteLine($"Move player: x = {_player.X}, y = {_player.Y}");
        }

        public bool PreUpdate(Player p, int updateX, int updateY)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Left += updateX;
            newCollider.Top += updateY;

            foreach (var item in _currentLevel.platforms)
            {
                bool willCollide = Engine.isIntersect(newCollider, item.Collider);
                if (willCollide)
                {
                    return true;
                }
            }
            foreach (var item in _currentLevel.barrier)
            {
                bool willCollide = Engine.isIntersect(item, newCollider);
                if (willCollide)
                {
                    return true;
                }
            }
            return false;
        }
        public void RenderLevel()
        {
            _view.LoadLevel(_currentLevel);
        }
        public void AddPlatformCollider(Platform p, FloatRect collider)
        {
            p.Collider = collider;
        }
        public void Update()
        {
            bool newStanding;
            if (_playerPrevX != _player.X)
            {
                newStanding = false;
            }
            else
            {
                newStanding = true;
            }
            _player.Update(newStanding);
            _playerPrevX = _player.X;
        }
    }
}
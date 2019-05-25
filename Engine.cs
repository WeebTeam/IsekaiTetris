using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// This is the engine part of the game (controls, score)
    /// </summary>
    public class Engine : GameState
    {
       // Graphics
        private Texture2D tetrisBackground, tetrisTextures;

        //character backgrounds (possibly have different ones for single/multiplayer)
        private Texture2D _boardSingle, _topBar;

        private SpriteFont gameFont, timerFont;
        private readonly Rectangle[] _pieces = new Rectangle[7];

        // Game
        private Board _board;
        //private GameScore _score;

        private Score _player;

        private bool _pause = false;
        private PauseScreen _pauseScreen;
        public bool _gameover = false;
        public bool _showgameover = false;
        private GameOver _gameoverScreen;

        public List<SoundEffect> _aquaInGameSE, _meguminInGameSE, _blockSoundEffects;

        // Keyboard Input (for debouncing)
        KeyboardState oldKeyboardState = Keyboard.GetState();

        public Engine(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            // rectangles that refers to the tetris.png file
            // each rectangle references one (out of 4) blocks of
            // every piece
            // used to generate the actual pieces in game
            // O figure
            _pieces[0] = new Rectangle(312, 0, 24, 24);
            // I figure
            _pieces[1] = new Rectangle(0, 24, 24, 24);
            // J figure
            _pieces[2] = new Rectangle(120, 0, 24, 24);
            // L figure
            _pieces[3] = new Rectangle(216, 24, 24, 24);
            // S figure
            _pieces[4] = new Rectangle(48, 96, 24, 24);
            // Z figure
            _pieces[5] = new Rectangle(240, 72, 24, 24);
            // T figure
            _pieces[6] = new Rectangle(144, 96, 24, 24);

            _pauseScreen = new PauseScreen(graphicsDevice);
            
            _meguminInGameSE = new List<SoundEffect>();
            _aquaInGameSE = new List<SoundEffect>();
            _blockSoundEffects = new List<SoundEffect>();

            //_score = new GameScore(gameFont);
            //_gameoverScreen = new GameOver(graphicsDevice, ref _score);
        }

        //public Engine(GraphicsDevice graphicsDevice, Character character) : this(graphicsDevice)
        //{
            
        //    _character = character;
        //}

        public Engine(GraphicsDevice graphicsDevice, Score player) : this(graphicsDevice)
        {
            _player = player;
            
            _gameoverScreen = new GameOver(graphicsDevice, ref _player);
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            //Load backgrounds
            if (_player.Character == Character.Kazuma)
                tetrisBackground = content.Load<Texture2D>("textures/background/back1Refined"); //gameplaybg
            if (_player.Character == Character.Aqua)
                tetrisBackground = content.Load<Texture2D>("textures/background/back2Refined"); //gameplaybg
            if (_player.Character == Character.Megumin)
                tetrisBackground = content.Load<Texture2D>("textures/background/back4Refined"); //gameplaybg
            if (_player.Character == Character.Darkness)
                tetrisBackground = content.Load<Texture2D>("textures/background/back5Refined"); //gameplaybg

            //Load 2D textures
            tetrisTextures = content.Load<Texture2D>("textures/tetris"); //the 7 pieces

            //top bar
            _topBar = content.Load<Texture2D>("textures/gameplay/topbar");

            //board backgrounds
            _boardSingle = content.Load<Texture2D>("textures/gameplay/board_single");


            // Load individual characters sound effects
            _meguminInGameSE.Add(content.Load<SoundEffect>("audios/soundEffects/explosion"));
            _meguminInGameSE.Add(content.Load<SoundEffect>("audios/soundEffects/meguminSE2"));
            _aquaInGameSE.Add(content.Load<SoundEffect>("audios/soundEffects/blessing"));

            // Load game font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");
            timerFont = content.Load<SpriteFont>("spritefonts/timerFont");

            // Load board sound effects
            _blockSoundEffects.Add(content.Load<SoundEffect>("audios/soundEffects/blockDrop"));
            //_blockSoundEffects.Add(content.Load<SoundEffect>("audios/soundEffects/enter"));
            _blockSoundEffects.Add(content.Load<SoundEffect>("audios/soundEffects/pauseMenu"));

            // Create game field
            if (_player.Character == Character.Kazuma)
            {
                _board = new KazumaBoard(ref tetrisTextures, _pieces);
                _board.needSkillCooldown = false;
            }
            if (_player.Character == Character.Aqua)
            {
                _board = new AquaBoard(ref tetrisTextures, _pieces, ref _aquaInGameSE);
                _board.needSkillCooldown = true;
            }
            if (_player.Character == Character.Megumin)
            {
                _board = new MeguminBoard(ref tetrisTextures, _pieces, ref _meguminInGameSE);
                _board.needSkillCooldown = true;
            }
            if (_player.Character == Character.Darkness)
            {
                _board = new DarknessBoard(ref tetrisTextures, _pieces);
                _board.needSkillCooldown = false;
            }
            
            //put the font as given to the board's font
            _board.font = gameFont;
            _board.timerFont = timerFont;

            _gameoverScreen._character = _player.Character;

            _pauseScreen.LoadContent(content);

            _gameoverScreen.LoadContent(content);

            _board.Initialize(); // init the board

        }

        public override void UnloadContent()
        {

        }

        public Character PlayingWith
        {
            get { return _player.Character; }
            set { _player.Character = value; }
        }

        public bool Paused
        {
            get { return _pause; }
            set { _pause = value; }
        }
        
        // timer for skill and game
        public float _skillCooldown = 15;
        public float _gameplayTime = 200;

        public override void Update(GameTime gameTime)
        {
            // Timer for skill & possibly game
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _gameoverScreen.Board = _board;

            // To allow the board to get this cooldown value to be printed out
            _board._cooldown = _skillCooldown;
            _board._gameplayTime = _gameplayTime;

            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check pause (if esc is pressed)
            bool pauseKey = (oldKeyboardState.IsKeyUp(Keys.Escape) && (keyboardState.IsKeyDown(Keys.Escape)));

            // if it's not game over
            if (!_gameover)
            {
                if (pauseKey || _pauseScreen.Unpause)
                {
                    _pause = !_pause;
                    _pauseScreen.Unpause = false;
                }
                if (pauseKey)
                    _blockSoundEffects[1].Play(0.2f, 0.0f, 0.0f);
                if (!_pause)
                {
                    // Only deduct the cooldown when it's not pause
                    _skillCooldown -= timer;
                    _gameplayTime -= timer;

                    // Find dynamic figure position
                    _board.FindDynamicFigure();

                    // Increase player score
                    int lines = _board.DestroyLines();
                    if (lines > 0)
                    {
                        if (_player.Character != Character.Darkness)
                            _player.Point += (int)((5.0f / 2.0f) * lines * (lines + 3));
                        else
                            _player.Point += (int)((5.0f / 2.0f) * lines * (lines + 7));
                    }

                    if (lines >= 4)
                        _board.Speed += 0.005f;

                    // Create new shape in game, if we cant, it means the the board is full, game over
                    if (!_board.CreateNewFigure())
                    {
                        _gameover = true;
                        _showgameover = true;
                        _gameoverScreen._lose = true;
                    }
                    //if they survive the time limit
                    else if (_gameplayTime <= 0)
                    {
                        _gameover = true;
                        _showgameover = true;
                        _gameoverScreen._win = true;
                        Scoreboard.SaveScore(_player);
                    }
                    else
                    {
                        if (_player.Character == Character.Darkness) //coz darkness always misses
                        {
                            // If right key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyUp(Keys.Right)))
                                _board.MoveFigureLeft();
                            // If left key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyUp(Keys.Left)))
                                _board.MoveFigureRight();
                            // If up key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Up) && (keyboardState.IsKeyUp(Keys.Up)))
                                _board.MoveFigureDown();
                            // Rotate figure
                            if (oldKeyboardState.IsKeyDown(Keys.Down) && (keyboardState.IsKeyUp(Keys.Down)))
                                _board.RotateFigure();
                        }
                        else
                        {
                            // If left key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyUp(Keys.Left)))
                                _board.MoveFigureLeft();
                            // If right key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyUp(Keys.Right)))
                                _board.MoveFigureRight();
                            // If down key is pressed
                            if (oldKeyboardState.IsKeyDown(Keys.Down) && (keyboardState.IsKeyUp(Keys.Down)))
                                _board.MoveFigureDown();
                            // Rotate figure
                            if (oldKeyboardState.IsKeyDown(Keys.Up) && (keyboardState.IsKeyUp(Keys.Up)))
                                _board.RotateFigure();
                        }

                        // Skill
                        if (_skillCooldown <= 0)
                        {
                            if (oldKeyboardState.IsKeyDown(Keys.E) && (keyboardState.IsKeyUp(Keys.E)))
                            {
                                _board.Skill();
                                // Reset the skill cooldown so that it can be used again except Megumin
                                if (_player.Character != Character.Megumin)
                                    _skillCooldown = 15;
                            }
                        }

                        // Hard drop
                        if (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)))
                        {
                            _blockSoundEffects[0].Play(0.2f, 0.0f, 0.0f);
                            _board.HardDrop();
                        }

                        // Moving figure
                        if (_board.Movement >= 1)
                        {
                            _board.Movement = 0;
                            _board.MoveFigureDown();
                        }
                        else
                            _board.Movement += _board.Speed;
                    }
                }
                else
                {
                    //run when paused
                }
            }
            else if (_gameoverScreen.PlayAgain == true)
            {
                _gameoverScreen.PlayAgain = false;
                _board.Initialize();
                _gameplayTime = 200;
                _skillCooldown = 15;
                //_score.Initialize();
                _gameover = false;
                _showgameover = false;
                _gameoverScreen.kazumaSECount = 0;
                _gameoverScreen.aquaSECount = 0;
                _gameoverScreen.meguminSECount = 0;
                _gameoverScreen.darknessSECount = 0;
                _gameoverScreen._lose = false;
                _gameoverScreen._win = false;
                _player.Point = 0;
            }


            oldKeyboardState = keyboardState;

            if (_pause)
                _pauseScreen.Update(gameTime);

            if (_showgameover)
                _gameoverScreen.Update(gameTime);
        }
        
        //public void GameOver()
        //{
        //    _gameover = true;
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw background
            spriteBatch.Draw(tetrisBackground, new Vector2(0, 0), Color.White);

            //draw board texture
            //vector coords calculated based on resolution of board bg
            spriteBatch.Draw(_boardSingle, new Vector2(262, 78), Color.White);

            //draw top bar (MUST BE after board texture)
            spriteBatch.Draw(_topBar, new Vector2(112, 0), Color.White);

            _board.Draw(spriteBatch);
            //_score.Draw(spriteBatch);

            spriteBatch.DrawString(gameFont, "SCORE", new Vector2(150,30), Color.White);
            spriteBatch.DrawString(gameFont, _player.Point.ToString(), new Vector2(170, 60), Color.White);

            if (_pause)
            {
                _pauseScreen.Draw(spriteBatch);
            }

            if(_showgameover)
            {
                _gameoverScreen.Draw(spriteBatch);
            }
        }
    }
}
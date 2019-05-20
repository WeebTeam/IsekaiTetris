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
        private Character _character;

        // Graphics
        private Texture2D tetrisBackground, tetrisTextures;

        //character backgrounds (possibly have different ones for single/multiplayer)
        private Texture2D _boardSingle, _topBar;

        private SpriteFont gameFont;
        private readonly Rectangle[] _pieces = new Rectangle[7];

        // Game
        private Board _board;
        private Score _score;
        private bool _pause = false;
        private PauseScreen _pauseScreen;

        public List<SoundEffect> _aquaInGameSE, _meguminInGameSE;

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
        }

        public Engine(GraphicsDevice graphicsDevice, Character character) : this(graphicsDevice)
        {
            
            _character = character;
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            //Load backgrounds
            if (_character == Character.Kazuma)
                tetrisBackground = content.Load<Texture2D>("textures/background/back1Refined"); //gameplaybg
            if (_character == Character.Aqua)
                tetrisBackground = content.Load<Texture2D>("textures/background/back2Refined"); //gameplaybg
            if (_character == Character.Megumin)
                tetrisBackground = content.Load<Texture2D>("textures/background/back3Refined"); //gameplaybg
            if (_character == Character.Darkness)
                tetrisBackground = content.Load<Texture2D>("textures/background/back5Refined"); //gameplaybg

            //Load 2D textures
            tetrisTextures = content.Load<Texture2D>("textures/tetris"); //the 7 pieces

            //top bar
            _topBar = content.Load<Texture2D>("textures/gameplay/topbar");

            //board backgrounds
            _boardSingle = content.Load<Texture2D>("textures/gameplay/board_single");


            // Load individual characters sound effects
            _meguminInGameSE.Add(content.Load<SoundEffect>("audios/soundEffects/explosion"));
            _aquaInGameSE.Add(content.Load<SoundEffect>("audios/soundEffects/blessing"));

            // Load game font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            // Create game field
            if (_character == Character.Kazuma)
                _board = new KazumaBoard(ref tetrisTextures, _pieces);
            if (_character == Character.Aqua)
                _board = new AquaBoard(ref tetrisTextures, _pieces, ref _aquaInGameSE);
            if (_character == Character.Megumin)
                _board = new MeguminBoard(ref tetrisTextures, _pieces, ref _meguminInGameSE);
            if (_character == Character.Darkness)
                _board = new DarknessBoard(ref tetrisTextures, _pieces);


            _pauseScreen.LoadContent(content);

            _board.Initialize(); // init the board

        }

        public override void UnloadContent()
        {

        }

        public Character PlayingWith
        {
            get { return _character; }
            set { _character = value; }
        }

        public bool Paused
        {
            get { return _pause; }
            set { _pause = value; }
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check pause (if esc is pressed)
            bool pauseKey = (oldKeyboardState.IsKeyUp(Keys.Escape) && (keyboardState.IsKeyDown(Keys.Escape)));

            if (pauseKey || _pauseScreen.Unpause)
            {
                _pause = !_pause;
                _pauseScreen.Unpause = false;
            }
            if (!_pause)
            {
                // Find dynamic figure position
                _board.FindDynamicFigure();

                // Increase player score
                int lines = _board.DestroyLines();
                //if (lines > 0)
                //{
                //    score.Value += (int)((5.0f / 2.0f) * lines * (lines + 3));
                //    board.Speed += 0.005f;
                //}

                //score.Level = (int)(10 * board.Speed);

                // Create new shape in game
                if (!_board.CreateNewFigure())
                    GameOver();
                else
                {
                    if (_character == Character.Darkness) //coz darkness always misses
                    {
                        if (oldKeyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyUp(Keys.Right)))
                            _board.MoveFigureLeft();
                        // If right key is pressed
                        if (oldKeyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyUp(Keys.Left)))
                            _board.MoveFigureRight();
                        // If down key is pressed
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
                    if (oldKeyboardState.IsKeyDown(Keys.E) && (keyboardState.IsKeyUp(Keys.E)))
                    {
                        _board.Skill();
                    }

                    // Hard drop
                    if (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)))
                        _board.HardDrop();

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
            oldKeyboardState = keyboardState;

            _pauseScreen.Update(gameTime);
        }

        public void GameOver()
        {
            //board.Initialize();
            //score.Initialize();
        }

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
            //score.Draw(spriteBatch);

            if (_pause)
            {
                _pauseScreen.Draw(spriteBatch);
            }
        }
    }
}
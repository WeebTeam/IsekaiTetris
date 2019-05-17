using System;
using System.IO;
using Microsoft.Xna.Framework;
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
        private Texture2D _kazumaBackground, _aquaBackground, _meguminBackground, _darknessBackground;

        private SpriteFont gameFont;
        private readonly Rectangle[] pieces = new Rectangle[7];

        // Game
        private Board board;
        private Score score;
        private bool pause = false;

        // Keyboard Input (for debouncing)
        KeyboardState oldKeyboardState = Keyboard.GetState();

        public Engine(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            // rectangles that refers to the tetris.png file
            // each rectangle references one (out of 4) blocks of
            // every piece
            // used to generate the actual pieces in game
            // O figure
            pieces[0] = new Rectangle(312, 0, 24, 24);
            // I figure
            pieces[1] = new Rectangle(0, 24, 24, 24);
            // J figure
            pieces[2] = new Rectangle(120, 0, 24, 24);
            // L figure
            pieces[3] = new Rectangle(216, 24, 24, 24);
            // S figure
            pieces[4] = new Rectangle(48, 96, 24, 24);
            // Z figure
            pieces[5] = new Rectangle(240, 72, 24, 24);
            // T figure
            pieces[6] = new Rectangle(144, 96, 24, 24);
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
            //Load 2D textures
            tetrisBackground = content.Load<Texture2D>("Images/back1Refined"); //gameplaybg
            tetrisTextures = content.Load<Texture2D>("Images/tetris"); //the 7 pieces

            //character backgrounds
            _kazumaBackground = content.Load<Texture2D>("Images/board");
            _aquaBackground = content.Load<Texture2D>("Images/board");
            _meguminBackground = content.Load<Texture2D>("Images/board");
            _darknessBackground = content.Load<Texture2D>("Images/board");

            // Load game font
            gameFont = content.Load<SpriteFont>("gameFont");

            // Create game field
            if (_character == Character.Kazuma)
                board = new KazumaBoard(ref tetrisTextures, pieces);
            if (_character == Character.Aqua)
                board = new AquaBoard(ref tetrisTextures, pieces);
            if (_character == Character.Megumin)
                board = new MeguminBoard(ref tetrisTextures, pieces);
            if (_character == Character.Darkness)
                board = new DarknessBoard(ref tetrisTextures, pieces);

            

            board.Initialize(); // init the board

        }

        public override void UnloadContent()
        {

        }

        public Character PlayingWith
        {
            get { return _character; }
            set { _character = value; }
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check pause
            bool pauseKey = (oldKeyboardState.IsKeyDown(Keys.P) && (keyboardState.IsKeyUp(Keys.P)));

            if (pauseKey)
                pause = !pause;

            if (!pause)
            {
                // Find dynamic figure position
                board.FindDynamicFigure();

                // Increase player score
                int lines = board.DestroyLines();
                //if (lines > 0)
                //{
                //    score.Value += (int)((5.0f / 2.0f) * lines * (lines + 3));
                //    board.Speed += 0.005f;
                //}

                //score.Level = (int)(10 * board.Speed);

                // Create new shape in game
                if (!board.CreateNewFigure())
                    GameOver();
                else
                {

                    // If left key is pressed
                    if (oldKeyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyUp(Keys.Left)))
                        board.MoveFigureLeft();
                    // If right key is pressed
                    if (oldKeyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyUp(Keys.Right)))
                        board.MoveFigureRight();
                    // If down key is pressed
                    if (oldKeyboardState.IsKeyDown(Keys.Down) && (keyboardState.IsKeyUp(Keys.Down)))
                        board.MoveFigureDown();

                    // Hard drop
                    if (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)))
                        board.HardDrop();

                    // Rotate figure
                    if (oldKeyboardState.IsKeyDown(Keys.Up) && (keyboardState.IsKeyUp(Keys.Up)))
                        board.RotateFigure();

                    // Moving figure
                    if (board.Movement >= 1)
                    {
                        board.Movement = 0;
                        board.MoveFigureDown();
                    }
                    else
                        board.Movement += board.Speed;
                }
            }
            oldKeyboardState = keyboardState;
        }

        public void GameOver()
        {
            //board.Initialize();
            //score.Initialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw character background
            if (_character == Character.Kazuma)
                spriteBatch.Draw(_kazumaBackground, Vector2.Zero, Color.White);
            else if (_character == Character.Aqua)
                spriteBatch.Draw(_aquaBackground, Vector2.Zero, Color.White);
            else if (_character == Character.Megumin)
                spriteBatch.Draw(_meguminBackground, Vector2.Zero, Color.White);
            else if (_character == Character.Darkness)
                spriteBatch.Draw(_darknessBackground, Vector2.Zero, Color.White);

            board.Draw(spriteBatch);
            //score.Draw(spriteBatch);
        }
    }
}
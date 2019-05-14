using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : GameState
    {
        // Graphics
        private Texture2D tetrisBackground, tetrisTextures;

        private SpriteFont gameFont;
        private readonly Rectangle[] blockRectangles = new Rectangle[7];

        // Game
        private Board board;
        private Score score;
        private bool pause = false;

        // Input
        KeyboardState oldKeyboardState = Keyboard.GetState();
        public Engine(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            // Create sprite rectangles for each figure in texture file
            // O figure
            blockRectangles[0] = new Rectangle(312, 0, 24, 24);
            // I figure
            blockRectangles[1] = new Rectangle(0, 24, 24, 24);
            // J figure
            blockRectangles[2] = new Rectangle(120, 0, 24, 24);
            // L figure
            blockRectangles[3] = new Rectangle(216, 24, 24, 24);
            // S figure
            blockRectangles[4] = new Rectangle(48, 96, 24, 24);
            // Z figure
            blockRectangles[5] = new Rectangle(240, 72, 24, 24);
            // T figure
            blockRectangles[6] = new Rectangle(144, 96, 24, 24);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // Try to open file if it exists, otherwise create it
            using (FileStream fileStream = File.Open("record.dat", FileMode.OpenOrCreate))
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            //Load 2D textures
            tetrisBackground = content.Load<Texture2D>("Images/gameplaybg");
            tetrisTextures = content.Load<Texture2D>("Images/tetris");

            // Load game font
            gameFont = content.Load<SpriteFont>("gameFont");

            // Create game field
            board = new Board(ref tetrisTextures, blockRectangles);
            board.Initialize();
            //Components.Add(board);

            // Save player's score and game level
            score = new Score(gameFont);
            score.Initialize();
            //Components.Add(score);

            // Load game record
            using (StreamReader streamReader = File.OpenText("record.dat"))
            {
                string player = null;
                if ((player = streamReader.ReadLine()) != null)
                    score.RecordPlayer = player;
                int record = 0;
                if ((record = Convert.ToInt32(streamReader.ReadLine())) != 0)
                    score.RecordScore = record;
            }
        }

        public override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            //if (keyboardState.IsKeyDown(Keys.Escape))  this.Exit();

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
                if (lines > 0)
                {
                    score.Value += (int)((5.0f / 2.0f) * lines * (lines + 3));
                    board.Speed += 0.005f;
                }

                score.Level = (int)(10 * board.Speed);

                // Create new shape in game
                if (!board.CreateNewFigure())
                    GameOver();
                else
                {
                    // If left key is pressed
                    if (keyboardState.IsKeyDown(Keys.Left))
                        board.MoveFigureLeft();
                    // If right key is pressed
                    if (keyboardState.IsKeyDown(Keys.Right))
                        board.MoveFigureRight();
                    // If down key is pressed
                    if (keyboardState.IsKeyDown(Keys.Down))
                        board.MoveFigureDown();

                    // Hard drop
                    if (keyboardState.IsKeyDown(Keys.Space))
                        board.HardDrop();

                    // Rotate figure
                    if (keyboardState.IsKeyDown(Keys.Up))
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
        }

        public void GameOver()
        {
            if (score.Value > score.RecordScore)
            {
                score.RecordScore = score.Value;

                pause = true;

                Record record = new Record();
                //record.ShowDialog ();

                score.RecordPlayer = record.Player;

                using (StreamWriter writer = File.CreateText("record.dat"))
                {
                    writer.WriteLine(score.RecordPlayer);
                    writer.WriteLine(score.RecordScore);
                }

                pause = false;
            }
            board.Initialize();
            score.Initialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tetrisBackground, Vector2.Zero, Color.White);
            board.Draw(spriteBatch);
            score.Draw(spriteBatch);
        }
    }
}
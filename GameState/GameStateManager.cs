using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class GameStateManager
    {

        // Instance of the game state manager     
        private static GameStateManager _instance;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private bool _quit = false;

        // Stack for the screens     
        private Stack<GameState> _screens = new Stack<GameState>();

        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }


        /// <summary>
        /// Sets the content manager
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// Adds a new screen to the stack 
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(GameState screen)
        {
            try
            {
                // Add the screen to the stack
                _screens.Push(screen);
                // Initialize the screen
                _screens.Peek().Initialize();
                // Call the LoadContent on the screen
                if (_content != null)
                {
                    _screens.Peek().LoadContent(_content);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        /// <summary>
        /// Removes the top screen from the stack
        /// </summary>
        public void RemoveScreen()
        {
            if (_screens.Count > 0)
            {
                try
                {
                    var screen = _screens.Peek();
                    _screens.Pop();
                }
                catch (Exception ex)
                {
                    // Log the exception
                }
            }
        }

        // Clears all the screen from the list
        public void ClearScreens()
        {
            while (_screens.Count > 0)
            {
                _screens.Pop();
            }
        }

        /// <summary>
        /// Removes all screens from the stack and adds a new one 
        /// </summary>
        /// <param name="screen"></param>
        public void ChangeScreen(GameState screen)
        {
            try
            {
                ClearScreens();
                AddScreen(screen);
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        /// <summary>
        /// Updates the top screen. 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Renders the top screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Draw(spriteBatch);
                }
            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
            }
        }

        /// <summary>
        /// Unloads the content from the screen
        /// </summary>
        public void UnloadContent()
        {
            foreach (GameState state in _screens)
            {
                state.UnloadContent();
            }
        }

        /// <summary>
        /// Mark the game to be quit
        /// </summary>
        public bool QuitGame
        {
            get { return _quit; }
            set { _quit = value; }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Tetris
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Tetris())
                game.Run();
        }
    }
#endif
}

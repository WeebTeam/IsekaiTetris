using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class MeguminBoard : Board
    {
        public MeguminBoard(ref Texture2D textures, Rectangle[] rectangles) : base(ref textures, rectangles)
        {

        }

        public override void Skill()
        {
            _showNewBlock = true;

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    ClearBoardField(i, j);
                }
            }
        }

    }
}

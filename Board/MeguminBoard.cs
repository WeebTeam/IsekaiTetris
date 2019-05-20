using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class MeguminBoard : Board
    {
        public MeguminBoard(ref Texture2D textures, Rectangle[] rectangles, ref List<SoundEffect> soundEffects) : base(ref textures, rectangles, ref soundEffects)
        {

        }

        public override void Skill()
        {
            _soundEffects[0].Play();

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

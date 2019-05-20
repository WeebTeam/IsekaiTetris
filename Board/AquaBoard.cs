using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class AquaBoard : Board
    {
        public AquaBoard(ref Texture2D textures, Rectangle[] rectangles, ref List<SoundEffect> soundEffects) : base(ref textures, rectangles, ref soundEffects)
        {

        }

        public override void Skill()
        {
            _soundEffects[0].Play();

            _nextFigures.Clear();
            _nextFigures.Enqueue(1);
            _nextFigures.Enqueue(1);
            _nextFigures.Enqueue(1);
            _nextFigures.Enqueue(1);
        }
    }
}

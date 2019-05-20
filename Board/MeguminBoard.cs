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
        //number of times the skill is used
        private int skillCount = 0;

        public MeguminBoard(ref Texture2D textures, Rectangle[] rectangles, ref List<SoundEffect> soundEffects) : base(ref textures, rectangles, ref soundEffects)
        {

        }

        //character's skill
        public override void Skill()
        {
            //if it has not been used once, skill can be activated
            if (skillCount == 0)
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
                skillCount = 1;
            }
            //play sound effect if it has been used
            else
                _soundEffects[1].Play(0.5f,0.0f,0.0f);
        }
    }
}

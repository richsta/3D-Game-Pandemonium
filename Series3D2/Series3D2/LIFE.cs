using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace Series3D2
{
    public class LIFE
    {
        private Vector2 scorePos = new Vector2(450, 10);

        public SpriteFont Font { get; set; }

        public int Score { get; set; }

        public LIFE()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the Score in the top-left of screen
            spriteBatch.DrawString(
                Font,                          // SpriteFont
                "Lives Lost: " + Score.ToString() + " / 5",  // Text
                scorePos,                      // Position
                Color.White);                  // Tint
        }
    }
}

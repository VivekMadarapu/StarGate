using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace StarGate
{
  
    class spaceShip
    {
        public Texture2D tex;
        public Rectangle desRect;
        public Rectangle sourceRecRight;
        public Rectangle sourceRecLeft;
        public Boolean isRight;
        public  GamePadState oldPad = GamePad.GetState(PlayerIndex.One);

        public spaceShip(Texture2D tex)
        {
            this.tex = tex;
            sourceRecRight = new Rectangle((tex.Width / 29*2), (tex.Height / 2) / 10, tex.Width / 21, (tex.Height / 2) / 10);
            sourceRecLeft = new Rectangle((tex.Width / 30 * 9), (tex.Height / 2) / 10, tex.Width / 21, (tex.Height / 2) / 10);
            desRect = new Rectangle(100, 100, 70, 50);
            isRight = true;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(!isRight)
            spriteBatch.Draw(tex, desRect, sourceRecLeft, Color.White);
            else
                spriteBatch.Draw(tex, desRect, sourceRecLeft, Color.White);

        }


    }
}

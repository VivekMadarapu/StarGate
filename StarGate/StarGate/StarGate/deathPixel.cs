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
    public class deathPixel
    {
        public Rectangle pixelRect;
        public Texture2D pixelTexture;
        public Vector2 pos;
        public float rotation;
        public int screenH;
        public int screenW;
        public double mx;
        public double my;
        public const int SPEED = 3;

        public deathPixel(Vector2 pos, float rotation, Texture2D pixelTexture, int w, int h)
        {
            this.pos = pos;
            this.rotation = rotation;
            pixelRect = new Rectangle((int)pos.X, (int)pos.Y, 5, 5);
            this.pixelTexture = pixelTexture;
            screenH = h;
            screenW = w;


            mx = Math.Cos(rotation) * SPEED;
            my = Math.Sin(rotation) * SPEED;



            //  mx = 10 * Math.Tan(rotation);
            // my = 10 * Math.Tan(rotation)+10;

            //if (MathHelper.ToDegrees(rotation) >= 180)
            //    mx *= -1;
            //if (MathHelper.ToDegrees(rotation) >= 90 && MathHelper.ToDegrees(rotation) <= 270)
            //    my *= -1;
        }
        public Boolean isOnScreen()
        {
            Boolean x = false;
            if (pixelRect.X >= screenW - pixelRect.Width || pixelRect.X <= 0 || pixelRect.Y <= 0 || pixelRect.Y >= screenH - pixelRect.Width)
                x = false;
            else
                x = true;
            return x;
        }
        public void Update()
        {
            pos.X += (float)mx;
            pos.Y += (float)my;
            pixelRect.X = (int)pos.X;
            pixelRect.Y = (int)pos.Y;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //        spriteBatch.Draw(pixelTexture, pos, null, Color.White,
            //(float)rotation + MathHelper.ToRadians(90),
            //new Vector2(pixelTexture.Width / 2, pixelTexture.Height / 2), 0.2f,
            //SpriteEffects.None, 0);
            spriteBatch.Draw(pixelTexture, pixelRect, Color.White);

        }
    }
}

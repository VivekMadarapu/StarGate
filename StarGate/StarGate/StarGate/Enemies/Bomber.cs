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

namespace StarGate.Enemies
{
    class Bomber : Enemy
    {
        public const int POINTS = 150;
        //texture
        public Texture2D tex;
        //rectangle
        public Rectangle sourceRect;
        //public List<Rectangle> sourceRects;
        public Rectangle desRect;
        int counter = 0;

        //speed/dimensions
        public const int SIZE = 30;
        public const int SPEED = 5;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        public Bomber(Texture2D tex)
        {
            //tex
            this.tex = tex;
            //Rectangles
            desRect = new Rectangle(rand.Next(0, 5000 - SIZE), rand.Next(0, screenH / 2), SIZE, SIZE);

            sourceRect = new Rectangle(tex.Width / 30 * (10), tex.Height / 29 * 10, tex.Width / 22, tex.Height / 25);
            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;
        }

        public void Update(/*List<Humanoid> humanoids,*/SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            //offcreen update
            relationalUpdate(terrain, ship, newPad);

            if (isOnScreen())
            {
                
            }
            else
            {
                desRect.Y -= SPEED / 2;
                
            }

            //sourceRect = sourceRects[counter];
            //counter++;
        }

        public Boolean isOnScreen()
        {
            return desRect.X >= 0 && desRect.Right <= screenW && desRect.Y >= 0 && desRect.Bottom <= screenH;
        }

        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)//changes landers position in relation to the spaceship
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {

                if (ship.isRight)
                {
                    if (terrain.bound < 4200)
                        desRect.X -= SPEED;
                }
                else
                {
                    if (terrain.bound > 0)
                        desRect.X += SPEED;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, sourceRect, Color.White);
        }

    }
}

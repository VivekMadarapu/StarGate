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
        public Texture2D fireballTex;

        public Rectangle desRect;
        private float rotation;
        int counter = 0;
        float elapsedTime = 0;
        private int oldSec;
        int directionX = 1; 
        int directionY = 1; 

        //fireballs
        public List<Fireball> fireballs;

        //speed/dimensions
        public const int SIZE = 26;
        public const int SPEED = 5;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        private int fireTimer;

        public Bomber(Microsoft.Xna.Framework.Game game)
        {
            //tex
            this.tex = game.Content.Load<Texture2D>("firebomber");
            this.fireballTex = game.Content.Load<Texture2D>("bomb");
            //Rectangles
            desRect = new Rectangle(100, rand.Next(100, 400), SIZE, SIZE);

            fireballs = new List<Fireball>();

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

            fireTimer = 60;
        }

        public void Update(double gameTime, SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            //offcreen update
            relationalUpdate(terrain, ship, newPad);

            elapsedTime += (float)gameTime;
            if (elapsedTime > 1)
            { 
                elapsedTime -= 1;
                directionX = rand.Next(0, 2); 
                rand = new Random();
                directionY = rand.Next(0, 2); 
            }

            if (directionX == 0)
            {
                desRect.X -= 2;
            }
            else if (directionX == 1)
            {
                desRect.X += 2;
            }

            if (directionY == 0 && desRect.Y > 2)
            {
                desRect.Y -= 2;
            }
            else if(directionY == 1 && desRect.Y < 498)
            {
                desRect.Y += 2;
            }

            if (fireTimer == 0)
            {
                fireballs.Add(new Fireball(fireballTex, this, ship));
                fireTimer = 60;
            }
            Console.WriteLine(oldSec + " " + gameTime);

            for (int i = 0; i < fireballs.Count; i++)
            {
                fireballs[i].Update(ship, newPad, terrain);
            }


            rotation += 0.2f;
            fireTimer--;
            //sourceRect = sourceRects[counter];
            //counter++;
        }

        public Boolean isOnScreen()
        {
            return desRect.X >= 0 && desRect.Right <= screenW && desRect.Y >= 0 && desRect.Bottom <= screenH;
        }

        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)
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
            spriteBatch.Draw(tex, desRect, null, Color.White, rotation, new Vector2(9, 9), SpriteEffects.None, 0);
            foreach (Fireball fireball in fireballs)
            {
                spriteBatch.Draw(fireballTex, fireball.desRect, Color.White);
            }
        }

    }
}

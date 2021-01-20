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

        //fireballs
        public List<Fireball> fireballs;

        //speed/dimensions
        public const int SIZE = 26;
        public const int SPEED = 5;
        //random
        public Random rand;
        public int switchDirectionTime = 0;
        public Vector2 speed;

        //screen dimensions
        public int screenW;
        public int screenH;

        private int fireTimer;


        public Bomber(Microsoft.Xna.Framework.Game game, Random r)
        {
            //tex
            this.tex = game.Content.Load<Texture2D>("firebomber");
            this.fireballTex = game.Content.Load<Texture2D>("bomb");
            //Rectangles
            desRect = new Rectangle(r.Next(100, 5000), r.Next(100, 400), SIZE, SIZE);

            fireballs = new List<Fireball>();

            rand = r;

            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;

            speed = new Vector2(2, 2);

            fireTimer = 120;
        }

        public void Update(double gameTime, SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            //offcreen update
            relationalUpdate(terrain, ship, newPad);

            if (switchDirectionTime == 0)
            {
                switchDirectionTime = rand.Next(30, 120);
                int x = rand.Next(1, 101);
                int y = rand.Next(1, 101);
                if (x > 50)
                    speed.X *= -1;
                if (y > 50)
                    speed.Y *= -1;
            }
            else
            {
                switchDirectionTime--;
            }

            if (fireTimer <= 0 && isOnScreen())
            {
                fireballs.Add(new Fireball(fireballTex, this, ship));
                fireTimer = 120;
            }
            

            for (int i = 0; i < fireballs.Count; i++)
            {
                fireballs[i].Update(ship, newPad, terrain);
            }

            rotation += 0.2f;
            fireTimer--;

            if (desRect.Right >= screenW)
                speed.X *= -1;
            else if (desRect.Left <= 0)
                speed.X *= -1; ;

            if (desRect.Y >= screenH - desRect.Height)
                speed.Y *= -1;
            else if (desRect.Y <= 0)
                speed.Y *= -1;

            desRect.X += (int)speed.X;
            desRect.Y += (int)speed.Y;

        }

        public Boolean isOnScreen()
        {
            return desRect.X >= 0 && desRect.Right <= screenW && desRect.Y >= 0 && desRect.Bottom <= screenH;
        }

        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {

                if (ship.isRight && ship.desRect.X == 400)
                {
                    if (terrain.bound < 4200)
                        desRect.X -= SPEED;
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
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

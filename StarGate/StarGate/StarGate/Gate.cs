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
    class Gate
    {
        public Texture2D tex;
        public Rectangle desRect;
        public bool used;

        Random rand = new Random();

        public Gate(Texture2D texture)
        {
            tex = texture;
            desRect = new Rectangle(rand.Next(3000, 4500), 100, 75, 50);
            used = false;
        }

        public void Update(GameScreen gameScreen, Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            relationalUpdate(terrain, ship, newPad);

            if (desRect.Intersects(ship.desRect))
            {
                shiftEverything(gameScreen);
                used = true;
            }
        }

        public void shiftEverything(GameScreen screen)
        {

        }

        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {

                if (ship.isRight && ship.desRect.X == 400)
                {
                    if (terrain.bound < 4200)
                        desRect.X -= 5;
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
                        desRect.X += 5;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, Color.White);
        }


    }
}

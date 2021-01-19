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

using System.Text;


namespace StarGate.Enemies
{
    class Mutant
    {

        //texture
        public Texture2D tex;
        //rectangle 
        Rectangle desRect;//replaces lander texture
        Rectangle sourceRect;
        //speed
        public const int SPEED = 5;
        public Mutant(Rectangle desRect, Microsoft.Xna.Framework.Game game)
        {
            //texture
            tex = game.Content.Load<Texture2D>("starGateAllSprites");
            //Rectangles
            this.desRect = desRect;
            //sourceRect
            sourceRect = new Rectangle((tex.Width / 29 * 2), (tex.Height / 2) / 10*2, tex.Width / 21, (tex.Height / 2) / 10);
        }
        public void Update(SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            //offcreen update
            relationalUpdate(terrain, ship, newPad);

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

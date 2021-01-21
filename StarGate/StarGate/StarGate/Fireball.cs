using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarGate.Enemies;

namespace StarGate
{
    public class Fireball
    {
        
        //texture
        public Texture2D tex;
        //Rectangles
        public Rectangle desRect;

        public const int SIZE = 6;

        //color
        public Color color;
        //random
        public Random rand = new Random();

        //updates
        public int numUpdates;

        private float dx;
        private float dy;

        public Fireball(Texture2D tex, Bomber bomber, SpaceShip ship)
        {
            //textures
            this.tex = tex;
            //rectangles
            desRect = new Rectangle(bomber.desRect.X, bomber.desRect.Y, SIZE, SIZE);
            // rects.Add(new Rectangle());
            //updates
            numUpdates = 0;
            color = Color.White;

            double relShipX = ship.desRect.X - bomber.desRect.X;
            double relShipY = ship.desRect.Y - bomber.desRect.Y;

            double hyp = Math.Sqrt(Math.Pow(relShipX, 2) + Math.Pow(relShipY, 2));
            dx = (float)(relShipX / hyp / 0.2);
            dy = (float)(relShipY / hyp / 0.2);
        }
        

        public void Update(SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            relationalUpdate(terrain, ship, newPad);

            desRect.X += (int)dx;
            desRect.Y += (int)dy;

            if (hits(ship.desRect))
            {
                //todo after we add lives and stuff
            }
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

        public Boolean hits(Rectangle rect2)//returns true if the projectile hits another object
        {
            
            if (rect2.Intersects(desRect))
                return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, color);
        }

    }
    
}

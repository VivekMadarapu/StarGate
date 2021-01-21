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
using StarGate.Enemies;

namespace StarGate
{
    class Gate
    {
        public Texture2D tex;
        public Rectangle desRect;
        public bool used;

        private Type lander;
        private Type bomber;
        private Type mutant;

        Random rand = new Random();

        public Gate(Texture2D texture)
        {
            tex = texture;
            desRect = new Rectangle(rand.Next(3000, 4500), 100, 75, 50);
            used = false;
            lander = typeof(Lander);
            bomber = typeof(Bomber);
            mutant = typeof(Mutant);
        }

        public void Update(List<Enemy> enemies, List<Humanoid> humanoids, Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            relationalUpdate(terrain, ship, newPad);

            if (desRect.Intersects(ship.desRect))
            {
                shiftEverything(terrain, enemies, humanoids);
                used = true;
            }
        }

        public void shiftEverything(Terrain terrain, List<Enemy> enemies, List<Humanoid> humanoids)
        {
            terrain.bound = terrain.bound % 2500;

            for (int i = 0; i < enemies.Count; i++)
            {
                Type a = enemies[i].GetType();
                
                if (a.Equals(lander))
                {
                    Lander l = (Lander)enemies[i];
                    
                    if (l.desRect.X + 2500 >= 5000)
                    {
                        l.desRect.X = l.desRect.X - 2500;
                    }
                    else
                    {
                        l.desRect.X = l.desRect.X + 2500;
                    }
                    enemies[i] = l;
                }
            
                else if (a.Equals(bomber))
                {
                    Bomber l = (Bomber)(enemies[i]);
                    if (l.desRect.X + 2500 >= 5000)
                    {
                        l.desRect.X = l.desRect.X - 2500;
                    }
                    else
                    {
                        l.desRect.X = l.desRect.X + 2500;
                    }
                    enemies[i] = l;
                }
                else if (a.Equals(mutant))
                {
                    Mutant l = (Mutant)(enemies[i]);
                    if (l.desRect.X + 2500 >= 5000)
                    {
                        l.desRect.X = l.desRect.X - 2500;
                    }
                    else
                    {
                        l.desRect.X = l.desRect.X + 2500;
                    }
                    enemies[i] = l;
                }
            }

            for (int i = 0; i < humanoids.Count; i++)
            {
                Humanoid h = (Humanoid)(humanoids[i]);
                if (h.container.X + 2500 >= 5000)
                {
                    h.container.X = h.container.X - 2500;
                }
                else
                {
                    h.container.X = h.container.X + 2500;
                }
                humanoids[i] = h;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, Color.White);
        }


    }
}

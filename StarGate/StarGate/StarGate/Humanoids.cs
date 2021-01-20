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
    class Humanoid
    {
        public static Random random = new Random();
        public static Texture2D humanoidTex;

        public bool caught;
        public Terrain terrain;
        public HumanoidCarryer carryer;

        public bool facingRight;
        public bool alive;
        public int spriteNumber;
        public int distanceFallen;
        int counter = 0;
        public int x, y;

        public Humanoid(GraphicsDevice graphics, Terrain terrain, HumanoidCarryer carryer)
        {
            caught = carryer != null;
            this.terrain = terrain;
            this.carryer = carryer;
            spriteNumber = 0;
            alive = true;
            distanceFallen = 0;

            x = random.Next(10, 4190);

            y = random.Next(terrain.terrainContour[x] + 5, graphics.Viewport.Height - 5);
        }

        public void setCarryer(HumanoidCarryer carryer)
        {
            this.carryer = carryer;
            caught = carryer != null;
        }

        public static void loadContent(Microsoft.Xna.Framework.Game game)
        {
            humanoidTex = game.Content.Load<Texture2D>("Humanoids");
        }

        public void Update(GraphicsDevice graphics, Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            relationalUpdate(terrain, ship, newPad);

            if (caught)
            {
                x = carryer.desRect.X + 3;
                y = carryer.desRect.Top + 15;
                spriteNumber = 0;
            }
            else if (y < terrain.terrainContour[Math.Abs(terrain.bound + x)])
            {
                y += 1;
                distanceFallen += 1;
            }
            else
            {
                if (distanceFallen > 50) alive = false;
                distanceFallen = 0;

                if (counter % 20 == 0)
                {
                    int xChange = random.Next(-1, 2) * 3;
                    int yChange = random.Next(-1, 2) * 3;
                    if (!(terrain.bound == 0 && x + xChange <= 10 || terrain.bound == 4200 && x + xChange <= graphics.Viewport.Width - 30)) x += xChange;
                    if (y + yChange >= terrain.terrainContour[Math.Abs(terrain.bound + x)] + 5 && y + yChange <= graphics.Viewport.Height - 15) y += yChange;
                    spriteNumber = (spriteNumber + 1) % 3;
                    if (xChange < 0 && facingRight || xChange > 0 && !facingRight) facingRight = !facingRight;
                }
                counter++;
            }
        }

        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)//changes landers position in relation to the spaceship
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {
                if (ship.isRight && ship.desRect.X == 400)
                {
                    if (terrain.bound < 4200)
                    {
                        x -= SpaceShip.SPEED;
                    }
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
                    {
                        x += SpaceShip.SPEED;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive) spriteBatch.Draw(humanoidTex, new Rectangle(x, y, 25, 25), new Rectangle(((facingRight) ? 3 + spriteNumber : spriteNumber) * 15, 0, 15, 15), Color.White);
        } 
    }
}

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
    public class Mutant:Enemy
    {

        //use later for scoring
        public const int POINTS = 150;
        //texture
        public Texture2D tex;
        public Texture2D projectileTex;

        //rectangle
        public Rectangle sourceRect;
        public Rectangle desRect;//uses the original lander rectangle 
        //projectiles
        public List<Rectangle> projectileList = new List<Rectangle>();
        public List<Vector2> projectileSpeeds = new List<Vector2>();
        public int fire =60;
        public int switchDirectionTime = 0;
       

        
        //speed/dimensions
        public const int SIZE = 30;
        public Vector2 speed;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        public Mutant(Rectangle desRect, Microsoft.Xna.Framework.Game game)
        {
            //texture
            tex = game.Content.Load<Texture2D>("starGateAllSprites");
            //Rectangles
            this.desRect = desRect;
            desRect.Y = 5; 
            //sourceRect
            sourceRect = new Rectangle(0,0, tex.Width / 33, (tex.Height / 2) / 15);
            //screendimension
            screenW = 800;
            screenH = 500;
            //projectiles
            projectileTex = game.Content.Load<Texture2D>("projectileTex");
            // projectileTex.SetData(new Color[] { Color.White});

            //speed
            speed = new Vector2(3, 3);
        }
        public void Update(SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            fire--;
            //offcreen update
            relationalUpdate(terrain, ship, newPad);
            //random movements
            if(switchDirectionTime==0)
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
            keepMutantOnScreen();

            
            locateDefender(ship);
            updateProjectiles();
            desRect.X += (int)speed.X;
            desRect.Y += (int)speed.Y;

        }
        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)//changes landers position in relation to the spaceship
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {

                if (ship.isRight && ship.desRect.X == 400)
                {
                    if (terrain.bound < 4200)
                    {
                        desRect.X -= 5;
                        for(int i=0; i<projectileList.Count;i++)
                        {
                            projectileList[i] = new Rectangle(projectileList[i].X-5, projectileList[i].Y, 2,2);
                        }
                    }
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
                    {
                        desRect.X += 5;
                        for(int i=0; i<projectileList.Count; i++)
                        projectileList[i] = new Rectangle(projectileList[i].X + 5, projectileList[i].Y, 2, 2);

                    }
                }
            }

        }
        public void keepMutantOnScreen()
        {
            if (desRect.Right >= screenW && speed.X>0)
                speed.X *= -1;
            else if (desRect.Left <= 0 && speed.X<0)
                speed.X *= -1; 

            if (desRect.Y >= screenH - desRect.Height  && speed.Y>0)
            {
                speed.Y *= -1;
                desRect.Y = screenH - desRect.Height;
            }
            else if (desRect.Y <= 0 && speed.Y<0)
            {
                speed.Y *= -1;
                desRect.Y = 0;
            }
        }
        public void locateDefender(SpaceShip ship)
        {
            double hyp = Math.Sqrt(Math.Pow(ship.desRect.X-desRect.X, 2) + Math.Pow(ship.desRect.Y-desRect.Y, 2));
            if (fire == 0)
            {
                int DESIREDSPEED = 5;
                int numUpdates =(int) (hyp / DESIREDSPEED);
                double dx = (ship.desRect.X-desRect.X)/ (double)numUpdates;
                double dy = (ship.desRect.Y-desRect.Y) / (double)numUpdates;
                projectileList.Add(new Rectangle(desRect.X + desRect.Width / 2, desRect.Y + desRect.Height / 2, 2, 2));
                projectileSpeeds.Add(new Vector2((int)dx, (int)dy));
                fire = rand.Next(60, 200);


            }
        }
        public void updateProjectiles()
        {
            for(int i=0; i<projectileList.Count; i++)
            {
                projectileList[i] = new Rectangle((int)(projectileList[i].X+projectileSpeeds[i].X),(int)(projectileList[i].Y + projectileSpeeds[i].Y),2,2);

              
                    if (projectileList[i].X >= screenW || projectileList[i].X <= 0 || projectileList[i].Y >= screenH || projectileList[i].Y <= 0)
                    {
                        projectileList.RemoveAt(i);
                    projectileSpeeds.RemoveAt(i);
                        i--;
                    }
                
            }
          
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, sourceRect, Color.White);
            for(int i=0; i<projectileList.Count; i++)
            {
                spriteBatch.Draw(projectileTex, projectileList[i], Color.White);
            }
        }
        
    }
}

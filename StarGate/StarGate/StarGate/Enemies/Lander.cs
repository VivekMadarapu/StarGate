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

    class Lander : Enemy, HumanoidCarryer
    {
        //use later for scoring
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
        public Vector2 landerSpeed;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        //booleans
        public Boolean hasHumanoid;
        public Boolean transform;// if true, the lander needs to transform into a mutant

        //projectiles
        public List<Rectangle> projectileList = new List<Rectangle>();
        public List<Vector2> projectileSpeeds = new List<Vector2>();
        public int fire = 60;
        public Texture2D projectileTex;

        //humanoids
       public  Humanoid caughtHumanoid = null;
        public int humanTarget;
        Rectangle HumanoidCarryer.desRect
        {
            get
            {
                return desRect;
            }
        }



        public Lander(Microsoft.Xna.Framework.Game game, Random r)
        {
            //tex
            this.tex = game.Content.Load<Texture2D>("starGateAllSprites");
            projectileTex = game.Content.Load<Texture2D>("projectileTex");
            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;
            //speed
            landerSpeed = new Vector2(5, 0);

            //Rectangles
            desRect = new Rectangle(r.Next(0,5000-SIZE), r.Next(0, screenH / 2), SIZE, SIZE);
          
            sourceRect = new Rectangle(tex.Width / 30 * (10), tex.Height / 29 * 10, tex.Width / 22, tex.Height / 25);
            
            //humanoid interaction
             hasHumanoid = false;
            transform = false;
             humanTarget = -1;


        }
        public void Update(List<Humanoid> humanoids,SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            fire--;
            //offcreen update
            relationalUpdate(terrain, ship, newPad);
     
            if (!hasHumanoid && !transform /*&& isOnScreen()*/)
            {
                if (humanTarget == -1)
                {
                    locateHumanoid(humanoids);
                }
                else if (humanoids[humanTarget].caught || !(humanoids[humanTarget].alive))
                {
                    humanTarget = -1;
                }
                else
                {
                    desRect.X += (int)landerSpeed.X;
                    desRect.Y += (int)landerSpeed.Y;
                    catchHumanoid(humanoids);
                }
            }
            else if(hasHumanoid && !transform)
            {
                if (desRect.Y <= 0)
                    transform = true;
                desRect.Y -= 1;
            }
             
            
      


            //projectiles
            locateDefender(ship);
            updateProjectiles();
            //dimensions
            keepLanderOnScreen(terrain, humanoids);

            }
        
        public void relationalUpdate(Terrain terrain, SpaceShip ship, GamePadState newPad)//changes landers position in relation to the spaceship
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {

                if (ship.isRight && ship.desRect.X==400)
                {
                    if (terrain.bound < 4200)
                    {
                        for (int i = 0; i < projectileList.Count; i++)
                        {
                            projectileList[i] = new Rectangle(projectileList[i].X - 5, projectileList[i].Y, 2, 2);
                        }
                        desRect.X -= SPEED;
                    }
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
                    {
                        for (int i = 0; i < projectileList.Count; i++)
                        {
                            projectileList[i] = new Rectangle(projectileList[i].X + 5, projectileList[i].Y, 2, 2);
                        }
                        desRect.X += SPEED;
                    }
                }
            }

        }
        public void locateDefender(SpaceShip ship)
        {
            double hyp = Math.Sqrt(Math.Pow(ship.desRect.X - desRect.X, 2) + Math.Pow(ship.desRect.Y - desRect.Y, 2));
            if (fire == 0)
            {
                int DESIREDSPEED = 5;
                int numUpdates = (int)(hyp / DESIREDSPEED);
                double dx = (ship.desRect.X - desRect.X) / (double)numUpdates;
                double dy = (ship.desRect.Y - desRect.Y) / (double)numUpdates;
                projectileList.Add(new Rectangle(desRect.X + desRect.Width / 2, desRect.Y + desRect.Height / 2, 2, 2));
                projectileSpeeds.Add(new Vector2((int)dx, (int)dy));
                fire = rand.Next(200, 400);


            }
        }
        public void updateProjectiles()
        {
            for (int i = 0; i < projectileList.Count; i++)
            {
                projectileList[i] = new Rectangle((int)(projectileList[i].X + projectileSpeeds[i].X), (int)(projectileList[i].Y + projectileSpeeds[i].Y), 2, 2);


                if (projectileList[i].X >= screenW || projectileList[i].X <= 0 || projectileList[i].Y >= screenH || projectileList[i].Y <= 0)
                {
                    projectileList.RemoveAt(i);
                    projectileSpeeds.RemoveAt(i);
                    i--;
                }

            }

        }
        public Boolean isOnScreen()
        {
            return desRect.X >= 0 && desRect.Right <= screenW && desRect.Y >= 0 && desRect.Bottom <= screenH;
        }
        public void locateHumanoid(List<Humanoid> humanoids)
        {
            double minDistance =  -1;
            int minIndex = -1;
            if (humanoids.Count > 0 && !(humanoids[0].caught))
            {
                minDistance = Math.Sqrt(Math.Pow(desRect.X - humanoids[0].x, 2) + Math.Pow(desRect.Y - humanoids[0].y, 2));
                minIndex = 0;
            }
            if (minIndex != -1)
            {
                for (int i = 1; i < humanoids.Count; i++)
                {
                    double temp = Math.Sqrt(Math.Pow(desRect.X - humanoids[i].x, 2) + Math.Pow(desRect.Y - humanoids[i].y, 2));
                    if (temp < minDistance && !(humanoids[i].caught))
                    {
                        minDistance = temp;
                        minIndex = i;
                    }
                }
            }
            if(minIndex!=-1)
            {
                int DESIREDSPEED = 2;
                int numUpdates = (int)(minDistance / DESIREDSPEED);
                double dx = (humanoids[minIndex].x - desRect.X) / (double)numUpdates;
                double dy = (humanoids[minIndex].y - desRect.Y) / (double)numUpdates;
                landerSpeed = new Vector2((float)dx,(float)dy);
                humanTarget = minIndex; 
            }
         


            minIndex = humanTarget;
        }
        public Boolean catchHumanoid(List<Humanoid> humanoids)
        {
            for(int i=0; i<humanoids.Count; i++)
            {
                if(desRect.Intersects(new Rectangle(humanoids[i].x, humanoids[i].y, 25,25)) && !humanoids[i].caught)
                {
                    humanoids[i].caught = true;
                    hasHumanoid = true;
                    humanoids[i].setCarryer(this);
                    caughtHumanoid = humanoids[i];
                    return true;
                }
            
            }
            return false;
        }
        public void keepLanderOnScreen(Terrain terrain, List<Humanoid>humanoids)
        {

            if (desRect.Right >= screenW && terrain.bound == 4200)
            {
                if(landerSpeed.X>0)
                landerSpeed.X *= -1;
                locateHumanoid(humanoids);
            }
            else if (desRect.Left <= 0 && terrain.bound == 0)
            {
                if(landerSpeed.X<0)
                landerSpeed.X *= -1; ;
                desRect.X = 2;
                locateHumanoid(humanoids);
            }

            if (desRect.Y >= screenH - desRect.Height)
            {
                landerSpeed.Y *= -1;
                locateHumanoid(humanoids);
            }
            else if (desRect.Y <= 0)
            {
                landerSpeed.Y *= -1;
                locateHumanoid(humanoids);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, sourceRect, Color.White);
            for (int i = 0; i < projectileList.Count; i++)
            {
                spriteBatch.Draw(projectileTex, projectileList[i], Color.White);
            }
        }


    }
}

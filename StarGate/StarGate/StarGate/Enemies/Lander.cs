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

    class Lander : Enemy 
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
        //direction
        //public Boolean isRight;
        //speed/dimensions
        public const int SIZE = 30;
        public const int SPEED = 5;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        //booleans
        Boolean hasHumanoid;
        Boolean transform;// if true, the lander needs to transform into a mutant

        public Lander(Microsoft.Xna.Framework.Game game)
        {
            //tex
            loadLanderImage(game);
            //Rectangles
            desRect = new Rectangle(rand.Next(0,5000-SIZE), rand.Next(0, screenH / 2), SIZE, SIZE);
          
            sourceRect = new Rectangle(tex.Width / 30 * (10), tex.Height / 29 * 10, tex.Width / 22, tex.Height / 25);
            //screen dimensions
            this.screenW = 800;
            this.screenH = 500;
            //humanoid interaction
            hasHumanoid = false;
            transform = false;

           
            
        }
        public  void loadLanderImage(Microsoft.Xna.Framework.Game game)
        {
            tex = game.Content.Load<Texture2D>("starGateAllSprites");
        }
        public void Update(/*List<Humanoid> humanoids,*/SpaceShip ship, GamePadState newPad, Terrain terrain)
        {
            //offcreen update
            relationalUpdate(terrain, ship, newPad);
            
           if(isOnScreen() && !hasHumanoid && !transform)
            {
                /*int humanTarget = locateHumanoid(humanoids);
                 * if(humanoids[humanTarget].rect.X>=desRect.X+5)
                 * {
                 *   desRect.X+=SPEED;
                   }
                   else if(humanoids[humanTarget].rect.X<=desRect.X-5)
                   {
                    desRect.X-=SPEED;
                    }

                 * if(humanoids[humanTarget].rect.Y>=desRect.Y+5)
                 * {
                 *   desRect.Y+=SPEED;
                   }
                   else if(humanoids[humanTarget].rect.Y<=desRect.X-5)
                   {
                    desRect.Y-=SPEED;
                    }
                  
                  if(Math.Sqrt(Math.Pow(desRect.X-humanoids[humanTarget].rect.X,2) + Math.Pow(desRect.Y-humanoids[humanTarget].rect.Y,2))<=5);
                    hasHumanoid=true;
                   
                 **/

                
            }
            else if(hasHumanoid && !transform)
           {
                desRect.Y -= SPEED/2;
                if(desRect.Y<=0)
                {
                    transform = true;
                }
           }

           


            //sourceRect = sourceRects[counter];
            //counter++;
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
        public Boolean isOnScreen()
        {
            return desRect.X >= 0 && desRect.Right <= screenW && desRect.Y >= 0 && desRect.Bottom <= screenH;
        }
        //public int locateHumanoid(List<Humanoid>humanoids)
        //{
        //    //double minDistance = Math.Sqrt(Math.Pow(rect.X-humanoids[0].rect.X,2) + Math.Pow(rect.Y-humanoids[0].rect.Y,2));
        //    /*
        //     * int minIndex = 0
        //     * for(int i=1; i<humanoids.Count; i++)
        //     * {
        //        //make sure to check if humanoid is already attatched to another lander
        //     *    double check =  Math.Sqrt(Math.Pow(rect.X-humanoids[i].rect.X,2) + Math.Pow(rect.Y-humanoids[i].rect.Y,2));
        //     *    if(check<minDistance)
        //     *    {
        //     *    minDistance = check;
        //     *    minIndex = i;
        //     *    }

        //     }
        //     */
        //return minIndex;
        //}
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, sourceRect, Color.White);
        }




        
    }
}

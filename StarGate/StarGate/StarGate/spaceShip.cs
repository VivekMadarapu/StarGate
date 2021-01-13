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
  
    class spaceShip
    {
        //textures
        public Texture2D tex;
        public Texture2D projectileTex;
        //rectangles
        public Rectangle desRect;
        public Rectangle sourceRecRight;
        public Rectangle sourceRecLeft;

        //booleans
        public Boolean isRight;

        //screen dimensions
        int screenW;
        int screenH;

        //movment
        public const int SPEED = 5;

        //Random
        Random rand = new Random();

        //flickering video game effect
        public int animator = 1;

        //projectile management
       public  List<Projectile> projectileList;
        public int projectileCooldown;

        //smartbombs (B) and (A) inviso cloaking
        public int cloakingTime = 180;
        public Boolean isCloaked;

        public int smartBombs = 3;

        //score?
        //public int score;
       
       

           

        public spaceShip(Texture2D tex, int screenW,int screenH, Texture2D projectileTex)
        {
            //textures
            this.tex = tex;
            this.projectileTex = projectileTex;

            //screen dimensions
            this.screenW = screenW;
            this.screenH = screenH;

            //rectangles
            sourceRecRight = new Rectangle((tex.Width / 29*2), (tex.Height / 2) / 10, tex.Width / 21, (tex.Height / 2) / 10);
            sourceRecLeft = new Rectangle((tex.Width / 30 * 9), (tex.Height / 2) / 10, tex.Width / 21, (tex.Height / 2) / 10);
            desRect = new Rectangle(rand.Next(0,screenW-50), rand.Next(0,screenH/2), 50, 30);

            //booleans
            if (desRect.X > screenW / 2)
                isRight = false;
            else
                isRight = true;

            //projectiles
            projectileList = new List<Projectile>();
            projectileCooldown = 21;

        }
        public void Update(GamePadState oldPad, GamePadState newPad)
        {
           
            //reverse
            if(oldPad.Buttons.X == ButtonState.Released && newPad.Buttons.X == ButtonState.Pressed)
            {
                isRight = !isRight;
            }
            //thrust
            if(newPad.Triggers.Left != 0)
            {
                if(isRight)
                {
                    desRect.X += SPEED;
                }
                else
                {
                    desRect.X -= SPEED;
                }
            }

            //thumbstick (up and down motion)
            if(newPad.ThumbSticks.Left.Y>0)
            {
                desRect.Y -= SPEED;
            }
            else if(newPad.ThumbSticks.Left.Y<0)
            {
                desRect.Y += SPEED;
            }
            //firing
            if (oldPad.Buttons.Y == ButtonState.Released && newPad.Buttons.Y == ButtonState.Pressed && projectileCooldown == 21)
            {
                projectileList.Add(new Projectile(projectileTex, desRect, isRight));
                projectileCooldown--;
            }
            if (projectileCooldown < 21)
            {
                projectileCooldown--;
                if (projectileCooldown == 0)
                    projectileCooldown = 21;
            }
            //projectiles

            for (int i=0; i<projectileList.Count; i++)
            {
                projectileList[i].Update();
                projectileList[i].removeOffScreenRects(screenW, screenH);
                if (projectileList[i].rects.Count() == 0)
                {
                    projectileList.RemoveAt(i);
                    i--;
                }


            }
            //collision detections
            keepShipOnScreen();

            //flicker effect of spaceship
            sourceRecLeft.Width += animator;
          
           
            sourceRecRight.X += animator;
            sourceRecRight.Width -= animator;
            animator *= -1;
          

        }
        public void keepShipOnScreen()//keeps the spaceship from going off screen
        {
            if (desRect.Right >= screenW)
                desRect.X -= SPEED;
            else if (desRect.Left <= 0)
                desRect.X += SPEED;

            if (desRect.Y >= screenH)
                desRect.Y -= SPEED;
            else if (desRect.Y <= 0)
                desRect.Y += SPEED;
        }
        public Boolean collidesWithShip(Rectangle rect2)//checks if ship collides with anything
        {
            return desRect.Intersects(rect2);
        }
        public void checkCloaking(GamePadState newPad)//manages cloaking device)
        {
            if (cloakingTime > 0 && newPad.Buttons.A == ButtonState.Pressed)
            {
               isCloaked = true;
                cloakingTime--;

            }
            else
            {
                isCloaked = false;
            }
        }
        public void startSmartBomb(GamePadState newPad, GamePadState oldPad)//preliminary code to help with smartBombs
        {
            //maybe add a list for the parameter
           if(smartBombs>0 && oldPad.Buttons.B == ButtonState.Released && newPad.Buttons.B == ButtonState.Pressed)
           {
                smartBombs--;
                //loop through list of enemies and search for those in visibility range 
                //destroy enemies in visibility range and add points to scoring
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw projectiles
            for(int i=0; i<projectileList.Count; i++)
            {
                projectileList[i].Draw(spriteBatch);
            }
            
            //draw ship
            if(!isRight)
            spriteBatch.Draw(tex, desRect, sourceRecLeft, Color.White);
            else
                spriteBatch.Draw(tex, desRect, sourceRecRight, Color.White);

        }


    }
}

﻿using System;
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
  
    public class SpaceShip : HumanoidCarryer
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
        public int screenW;
        public int screenH;

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
        // dead
        public Boolean isDead = false;
        public int deathTimer = 600;
        public List<deathPixel> deathPixels = new List<deathPixel>();
        public int deathPixelCount = 0;
        public float deathAngle = 100;
     
        //humanoids
        public List<Humanoid> humanoids;
        Rectangle HumanoidCarryer.desRect
        {
            get
            {
                return desRect;
            }
        }

        public SpaceShip(Texture2D tex, int screenW,int screenH, Texture2D projectileTex)
        {
            //textures
            this.tex = tex;
            this.humanoids = new List<Humanoid>();

            this.projectileTex = projectileTex;
            projectileTex.SetData(new Color[] { Color.White });
            //screen dimensions
            this.screenW = 800;
            this.screenH = screenH;

            //rectangles
            sourceRecRight = new Rectangle((tex.Width / 29*2), (tex.Height / 2) / 10, tex.Width / 21-1, (tex.Height / 2) / 12);
            sourceRecLeft = new Rectangle((tex.Width / 30 * 9), (tex.Height / 2) / 10, tex.Width / 21, (tex.Height / 2) / 12);
            desRect = new Rectangle(395, rand.Next(0,screenH/2), 50, 30);

            //booleans
            if (desRect.X > screenW / 2)
                isRight = false;
            else
                isRight = true;

            //projectiles
            projectileList = new List<Projectile>();
            projectileCooldown = 21;

        }

        public void addHumanoid(Humanoid humanoid)
        {
            this.humanoids.Add(humanoid);
            humanoid.setCarryer(this);
        }

        public void Update(GamePadState oldPad, GamePadState newPad, Terrain terrain)
        {

            //reverse
            if (oldPad.Buttons.X == ButtonState.Released && newPad.Buttons.X == ButtonState.Pressed)
            {
                isRight = !isRight;
            }

            if (humanoids.Count != 0 && newPad.Buttons.B == ButtonState.Pressed)
            {
                for (int i = 0; i < humanoids.Count; i++)
                {
                    humanoids[i].setCarryer(null);
                    humanoids[i].droppedByHumanoids = true;
                    humanoids.RemoveAt(i);
                    i--;
                }
            }

            //thrust
            if ((terrain.bound == 0) || (terrain.bound>=4200/*-desRect.Width*/))
            {
                if (newPad.Triggers.Left != 0)
                {
                    if (isRight)
                    {
                        desRect.X += SPEED;
                    }
                    else
                    {
                        desRect.X -= SPEED;
                    }
                }
            }

            //thumbstick (up and down motion)
            if(newPad.ThumbSticks.Left.Y>0)
            {
                desRect.Y -= 2;
            }
            else if(newPad.ThumbSticks.Left.Y<0)
            {
                desRect.Y += 2;
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
        public void updateDeath()
        {
    
            
            if(/*deathTimer>0 &&*/ deathPixelCount<50)
            {
                double multiplier = deathPixelCount / 600.0 * 700;
                deathAngle = (float)((double)deathAngle + rand.NextDouble() * multiplier) % 360;
                deathPixels.Add(new deathPixel(new Vector2(desRect.X, desRect.Y), MathHelper.ToRadians(deathAngle),projectileTex, screenW, screenH));
                deathPixelCount++;
            }
            for (int i = 0; i < deathPixels.Count; i++)
            {
                deathPixels[i].Update();
                if (deathPixels[i].isOnScreen() == false)
                {
                    deathPixels.RemoveAt(i);
                    i--;
                }
            }

            deathTimer--;

        }
        public void keepShipOnScreen()//keeps the spaceship from going off screen
        {
            if (desRect.Right >= screenW)
                desRect.X = screenW - desRect.Width;
            else if (desRect.Left <= 0)
                desRect.X = 0; ;

            if (desRect.Y >= screenH)
                desRect.Y = screenH-desRect.Height;
            else if (desRect.Y <= 0)
                desRect.Y = 0;
        }
        public Boolean collidesWithShip(Rectangle rect2)//checks if ship collides with anything
        {
            Rectangle r2 = new Rectangle(rect2.X + rect2.Width / 3, rect2.Y + rect2.Height / 3, rect2.Width / 2, rect2.Height / 2);
            Rectangle r = new Rectangle(desRect.X, desRect.Y + desRect.Height / 2, desRect.Width, desRect.Height/2);
            //return desRect.Intersects(rect2);
            return rect2.Contains(desRect.X + desRect.Width / 2, desRect.Y + desRect.Height / 2);
        }
        public Boolean collidesWithProjectiles(List<Rectangle>fires)//checks to see if any projectiles hit the ship
        {
            for(int i=0; i<fires.Count; i++)
            {
                if (desRect.Contains(fires[i]))
                    return true;
            }

            return false;
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
            if (!isDead)
            {
                if (!isRight)
                    spriteBatch.Draw(tex, desRect, sourceRecLeft, Color.White);
                else
                    spriteBatch.Draw(tex, desRect, sourceRecRight, Color.White);
            }
            else
            {
              for(int i=0; i<deathPixels.Count; i++)
                {
                    deathPixels[i].Draw(spriteBatch);
                }
            }
            //else if(deathTimer>0)
            //{
            //    for(int i=0; i<deathPixels.Count; i++)
            //    {
            //        spriteBatch.Draw(projectileTex, deathPixels[i], Color.White);
            //    }
            //}

        }


    }
}

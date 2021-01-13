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
    class Projectile
    {
        //texture
        public Texture2D tex;
        //Rectangles
        public List<Rectangle> rects;
        public const int SIZE = 3;
        //list management
        public const int BEAMSIZE = 20;
        //speed
        public int speed = 15;
        //color
        public Color color;
        //boolean
        public Boolean isRight;
        //des rect
        public Rectangle start;
        //random
        public Random rand = new Random();

        //updates
        public int numUpdates;

        public Projectile(Texture2D tex, Rectangle startPoint, Boolean isRight)
        {
            //textures
            this.tex = tex;
            //rectangles
            start = startPoint;
            //boolean
            this.isRight = isRight;
            if (!isRight)
                speed *= -1;
            //rects
            rects = new List<Rectangle>();
           // rects.Add(new Rectangle(start.X + start.Width / 2, start.Y + start.Height / 2, SIZE, SIZE));
            //updates
            numUpdates = 0;
            //color
            generateColor();

        }
        public void generateColor()
        {
         
            color = new Color(rand.Next(20, 255), rand.Next(20, 255), rand.Next(20, 255));
        }
        public void Update()
        {
            numUpdates++;
            int x = rects.Count;
            if (numUpdates < BEAMSIZE)
                x = numUpdates;


      
            for (int i = 0; i < x; i++)
            {
              
                if(i<rects.Count)
                {
                    rects[i] = new Rectangle(rects[i].X + speed, rects[i].Y, rects[i].Width, rects[i].Height);

                }
                else
                {
                    int y = rand.Next(1, 101);
                    if (y > 25)
                    {
                        rects.Add(new Rectangle(start.X + start.Width / 2, start.Y + start.Height / 2, SIZE, SIZE));
                    }
                    else
                    {
                        rects.Add(new Rectangle(start.X + start.Width / 2, start.Y + start.Height / 2, 0, 0));

                    }
                }

            }

        }
        public void removeOffScreenRects(int screenW, int screenH)//removes the projectiles when they go off screen
        {
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i].X >= screenW || rects[i].X <= 0)
                {
                    rects.RemoveAt(i);
                    i--;
                }
            }
            
        }
        public Boolean hits(Rectangle rect2)//returns true if the projectile hits another object (like an alien)
        {
            for(int i=0; i<rects.Count; i++)
            {
                if(rect2.Intersects(rects[i]))
                {
                    rects.Clear();
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                spriteBatch.Draw(tex, rects[i], color);
            }

        }
    }
}

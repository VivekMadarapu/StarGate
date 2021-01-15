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

    class Lander
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
        public Boolean isRight;
        //speed/dimensions
        public const int SIZE = 30;
        public const int SPEED = 5;
        //random
        public Random rand = new Random();

        //screen dimensions
        public int screenW;
        public int screenH;

        //onscreen

        public Lander(Texture2D tex, int screenW, int screenH)
        {
            //tex
            this.tex = tex;
            //Rectangles
            desRect = new Rectangle(rand.Next(0, 100), rand.Next(0, screenH / 2), SIZE, SIZE);
          
            sourceRect = new Rectangle(tex.Width / 30 * (10), tex.Height / 29 * 10, tex.Width / 22, tex.Height / 25);
            //screen dimensions
            this.screenW = screenW;
            this.screenH = screenH;


           
            
        }
        public void Update(/*List<Humanoid> humans*/)
        {
            ////int minDistance;
            //if (counter >= sourceRects.Count)
            //    counter = 0;

            //sourceRect = sourceRects[counter];
            //counter++;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, desRect, sourceRect, Color.White);
        }




        
    }
}

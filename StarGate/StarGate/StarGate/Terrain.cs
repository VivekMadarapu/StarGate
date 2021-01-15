using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarGate
{
    class Terrain
    {
        public int[] terrainContour;
        private Random random;
        public Texture2D lines;
        private int width;
        private int height;
        public int bound;

        public Terrain(int width, int height, Texture2D lines)
        {
            this.random = new Random();
            this.width = width;
            this.height = height;
            bound = 0;
            this.lines = lines;
            lines.SetData(new Color[] { Color.White });
        }

        public void GenerateTerrain()
        {
            terrainContour = new int[width];

            double rand1 = (random.NextDouble()+1);
            double rand2 = (rand1+1);
            double rand3 = (rand2);

            float peakheight = 30;
            float flatness = 150;
            int offset = 200;


            for (int x = 0; x < width; x++)
            {

                double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
                height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
                height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);

                height += offset;

                terrainContour[x] = (int)height*2;
            }
        }

        public void Update(GamePadState newPad, spaceShip ship, int width)
        {
            if (bound >= 0 && bound <= 5000-width)
            {
                if (ship.desRect.Right >= width-5 && newPad.Triggers.Left != 0)
                    bound += 10;
                else if (ship.desRect.Left <= 5 && newPad.Triggers.Left != 0)
                    bound -= 10;
                else if (ship.isRight)
                    bound += 5;
                else
                    bound -= 5;
            }
            if (bound < 0)
                bound = 0;
            else if (bound >= 5000 - width)
                bound = 5000 - width;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            for (int i = 0; i < 800; i += 2)
            {
                spriteBatch.Draw(lines, new Rectangle(i, terrainContour[i+bound], 1, 1), color);
            }
        }

    }
}

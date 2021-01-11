using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarGate
{
    class Terrain
    {
        public int[] terrainContour;
        private Random random;
        public Texture2D lines;
        private int width;
        private int height;

        public Terrain(int width, int height, Texture2D lines)
        {
            this.random = new Random();
            this.width = width;
            this.height = height;
            this.lines = lines;
            lines.SetData(new Color[] { Color.Black });
        }

        public void GenerateTerrain()
        {
            terrainContour = new int[width * height];

            //Make Random Numbers
            double rand1 = random.NextDouble() + 1;
            double rand2 = random.NextDouble() + 2;
            double rand3 = random.NextDouble() + 3;

            //Variables, Play with these for unique results!
            float peakheight = 20;
            float flatness = 50;
            int offset = 30;


            //Generate basic terrain sine
            for (int x = 0; x < width; x++)
            {

                double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
                height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
                height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);

                height += offset;

                terrainContour[x] = (int)height;
                Console.WriteLine(height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < terrainContour.Length; i++)
            {
                spriteBatch.Draw(lines, new Rectangle(i, terrainContour[i], 1, 1), Color.White);
            }
        }
    }
}

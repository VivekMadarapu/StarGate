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
        public bool caught;
        public Terrain terrain;
        int x, y;

        public Humanoid(GraphicsDevice graphics, Terrain terrain)
        {
            caught = false;
            this.terrain = terrain;

            Random random = new Random();
            int x = random.Next(10, graphics.Viewport.Width - 10);
            int y = random.Next(10, terrain.terrainContour[x] - 10);
        }
    }
}

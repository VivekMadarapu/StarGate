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
    class Mutant
    {
        //texture
        public Texture2D tex;
        //rectangle 
        Rectangle desRect;//replaces lander texture
        public Mutant(Rectangle desRect, Microsoft.Xna.Framework.Game game)
        {
            loadMutantTex(game);
            //Rectangles
            this.desRect = desRect;
            //sourceRect

        }
        public void loadMutantTex(Microsoft.Xna.Framework.Game game)
        {
            tex = game.Content.Load<Texture2D>("starGateAllSprites");
        }
    }
}

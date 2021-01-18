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
    class GameScreen
    {
        SpaceShip ship;
        Terrain terrain;

        public void initializeGameObjects(Microsoft.Xna.Framework.Game game)
        {
            ship = new SpaceShip(game.Content.Load<Texture2D>("starGateAllSprites"), 
                800, 500, new Texture2D(game.GraphicsDevice, 1, 1));

            terrain = new Terrain(5000, 500, new Texture2D(game.GraphicsDevice, 1, 1));
            terrain.GenerateTerrain();
        }

        public void Update(GraphicsDevice graphicsDevice, GamePadState newPad, GamePadState oldPad)
        {
            ship.Update(oldPad, newPad, terrain);
            terrain.Update(newPad, ship, graphicsDevice.Viewport.Width);
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            ship.Draw(spriteBatch);
            terrain.Draw(spriteBatch, Color.White, graphicsDevice.Viewport.Width);
        }
    }
}

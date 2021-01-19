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
using StarGate.Enemies;

namespace StarGate
{
    class GameScreen
    {
        SpaceShip ship;
        Terrain terrain;
        Lander lander;
        Bomber bomber;
        Mutant mutant;


        public void initializeGameObjects(Microsoft.Xna.Framework.Game game)
        {
            ship = new SpaceShip(game.Content.Load<Texture2D>("starGateAllSprites"), 
                800, 500, new Texture2D(game.GraphicsDevice, 1, 1));

            terrain = new Terrain(5000, 500, new Texture2D(game.GraphicsDevice, 1, 1));
            terrain.GenerateTerrain();

            lander = new Lander(game);
            bomber = new Bomber(game);
            mutant = new Mutant(new Rectangle(100,100,30,30), game);
        }

        public void Update(GraphicsDevice graphicsDevice, GamePadState newPad, GamePadState oldPad)
        {
            ship.Update(oldPad, newPad, terrain);
            terrain.Update(newPad, ship, graphicsDevice.Viewport.Width);
            lander.Update(ship, newPad, terrain);
            bomber.Update(ship, newPad, terrain);
            mutant.Update(ship, newPad, terrain);
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            ship.Draw(spriteBatch);
            terrain.Draw(spriteBatch, Color.White, graphicsDevice.Viewport.Width);
            lander.Draw(spriteBatch);
            bomber.Draw(spriteBatch);
            mutant.Draw(spriteBatch);
        }
    }
}

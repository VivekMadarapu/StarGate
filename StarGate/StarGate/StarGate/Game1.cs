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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpaceShip ship;
        private Terrain terrain;

        TitleScreen titleScreen;
        GameScreen gameScreen;

        public static GameState gameState;
        //just for testing
      // Enemies.Lander lander;
        //user interface
        GamePadState oldPad;
  
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameState = GameState.START_SCREEN;

            titleScreen = new TitleScreen(graphics);
            terrain = new Terrain(5000, 500, new Texture2D(GraphicsDevice, 1, 1));
            terrain.GenerateTerrain();

            //user interface
            oldPad = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
           
            titleScreen.loadTitleScreenImage(this);
            MousePointer.loadPointerImage(this);
            Button.loadContent(this);
           ship = new SpaceShip(Content.Load<Texture2D>("starGateAllSprites"), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content.Load<Texture2D>("projectileTex"));
          // lander = new Enemies.Lander(Content.Load<Texture2D>("starGateAllSprites"), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState newPad = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (newPad.Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //test lander

            // lander.Update();
            // TODO: Add your update logic here
            if (gameState == GameState.START_SCREEN) titleScreen.Update(graphics, newPad, oldPad);

            //ship and terrain
            if (gameState == GameState.GAME_SCREEN)
            {
                ship.Update(oldPad, newPad, terrain);
                terrain.Update(newPad, ship, GraphicsDevice.Viewport.Width);
               //lander.Update(ship, newPad, terrain);
            }

            oldPad = newPad;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.START_SCREEN) titleScreen.Draw(spriteBatch);
            else if (gameState == GameState.GAME_SCREEN)
            {
                ship.Draw(spriteBatch);
                terrain.Draw(spriteBatch, Color.White, GraphicsDevice.Viewport.Width);
               // lander.Draw(spriteBatch);
            }

           

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }

    public enum GameState
    {
        START_SCREEN, SETTINGS_SCREEN, GAME_SCREEN, END_SCREEN
    }
}

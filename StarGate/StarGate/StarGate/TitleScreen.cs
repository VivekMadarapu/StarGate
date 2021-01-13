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
    class TitleScreen
    {
        public static int TITLESCREEN_SIZE = 500;
        public Texture2D titleImage;
        public Vector2 titleImageContainer;
        public Rectangle sourceRectangle;
        MousePointer mousePointer;
        Button settingsButton, startGameButton;
        
        public TitleScreen(GraphicsDeviceManager graphics)
        {
            titleImage = null;
            titleImageContainer = new Vector2(125, 225);
            sourceRectangle = new Rectangle(0, 0, 0, 50);
            graphics.PreferredBackBufferWidth = TITLESCREEN_SIZE;
            graphics.PreferredBackBufferHeight = TITLESCREEN_SIZE;
            graphics.ApplyChanges();
            mousePointer = new MousePointer(400, 400);
            settingsButton = new Button(new Rectangle(100, 400, 100, 25), "Settings");
            startGameButton = new Button(new Rectangle(300, 400, 100, 25), "Play");
        } 

        public void loadTitleScreenImage(Microsoft.Xna.Framework.Game game)
        {
            titleImage = game.Content.Load<Texture2D>("starGateTitle");
        }

        public void Update(GraphicsDeviceManager graphics, GamePadState gamePad, GamePadState oldPad)
        {
            if (sourceRectangle.Width < titleImage.Width) sourceRectangle.Width++;
            mousePointer.Update(gamePad);
            settingsButton.Update((float)mousePointer.x, (float)mousePointer.y);
            startGameButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (settingsButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed) Game1.gameState = GameState.SETTINGS_SCREEN;
            if (startGameButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {
                Game1.gameState = GameState.GAME_SCREEN;
                graphics.PreferredBackBufferWidth = TITLESCREEN_SIZE;
                graphics.PreferredBackBufferHeight = TITLESCREEN_SIZE;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleImage, titleImageContainer, sourceRectangle, Color.White);
            settingsButton.Draw(spriteBatch);
            startGameButton.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
        }
    }
}

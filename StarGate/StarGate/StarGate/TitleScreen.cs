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
    public class TitleScreen
    {
        public static int TITLESCREEN_SIZE = 500;
        public static Texture2D titleImage;
        public Vector2 titleImageContainer;
        public Rectangle sourceRectangle;
        public MousePointer mousePointer;
        Button settingsButton, startGameButton;
        
        public TitleScreen(GraphicsDeviceManager graphics)
        {
            titleImage = null;
            titleImageContainer = new Vector2(125, 225);
            sourceRectangle = new Rectangle(0, 0, 0, 50);
            graphics.PreferredBackBufferWidth = TITLESCREEN_SIZE;
            graphics.PreferredBackBufferHeight = TITLESCREEN_SIZE;
            graphics.ApplyChanges();
            mousePointer = new MousePointer(400, 400, TITLESCREEN_SIZE, TITLESCREEN_SIZE);
            settingsButton = new Button(new Rectangle(100, 400, 100, 25), "Settings");
            startGameButton = new Button(new Rectangle(300, 400, 100, 25), "Play");
        } 

        public static void loadTitleScreenImage(Microsoft.Xna.Framework.Game game)
        {
            titleImage = game.Content.Load<Texture2D>("starGateTitle");
        }

        public void Update(Game1 game, GraphicsDeviceManager graphics, GamePadState gamePad, GamePadState oldPad)
        {
            if (sourceRectangle.Width < titleImage.Width) sourceRectangle.Width++;
            mousePointer.Update(gamePad);
            settingsButton.Update((float)mousePointer.x, (float)mousePointer.y);
            startGameButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (settingsButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed) game.gameState = GameState.SETTINGS_SCREEN;
            if (startGameButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {
                game.gameState = GameState.GAME_SCREEN;
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 500;
                graphics.ApplyChanges();
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

    

    public class SettingsScreen
    {
        public static String controls =
            "Settings: \n\n" +
            "Left Thumbstick - move up and down\n" +
            "Left Trigger - Thrust\n" +
            "X - Reverse\n" +
            "Y - Fire\n" +
            "A - Inviso Cloaking Device\n" +
            "B - Smart Bombs";

        public LabelPrompt settingsPrompt;
        public Button backButton;
        public MousePointer mousePointer;

        public SettingsScreen(MousePointer mousePointer)
        {
            settingsPrompt = new LabelPrompt(new Vector2(50, 100), controls);
            backButton = new Button(new Rectangle(370, 400, 100, 25), "Back");
            this.mousePointer = mousePointer;
        }

        public void Update(Game1 game, GamePadState gamePad, GamePadState oldPad)
        {
            mousePointer.Update(gamePad);
            backButton.Update((float)mousePointer.x, (float)mousePointer.y);
            if (backButton.overLapping && gamePad.Buttons.A == ButtonState.Pressed) game.gameState = GameState.START_SCREEN;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backButton.Draw(spriteBatch);
            settingsPrompt.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
        }
    }
}

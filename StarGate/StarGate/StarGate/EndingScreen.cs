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
    public class EndingScreen
    {
        public Button playAgain;
        public LabelPrompt score;
        MousePointer mousePointer;

        public EndingScreen(GraphicsDevice graphics, MousePointer mousePointer)
        {
            this.mousePointer = mousePointer;
            mousePointer.screenSize = new Vector2(800, 500);
            String scoreLabel = "Score: ";
            score = new LabelPrompt(new Vector2(400 - scoreLabel.Length * 10, 240), scoreLabel);
            playAgain = new Button(new Rectangle(325, 350, 150, 30), "Play Again");
        }

        public void Update(Game1 game, GraphicsDeviceManager graphics, GamePadState gamePad, GamePadState oldPad)
        {
            mousePointer.Update(gamePad);
            playAgain.Update((float)mousePointer.x, (float)mousePointer.y);
            if (playAgain.overLapping && gamePad.Buttons.A == ButtonState.Pressed)
            {
                game.gameState = GameState.START_SCREEN;

                game.gameScreen = new GameScreen();
                game.gameScreen.initializeGameObjects(game);

                game.titleScreen = new TitleScreen(graphics);
                TitleScreen.loadTitleScreenImage(game);
                game.settingsScreen = new SettingsScreen(game.titleScreen.mousePointer);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Texture2D background = new Texture2D(graphics, 1, 1);
            background.SetData(new Color[] { Color.White });
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White * 0.4f);
            score.Draw(spriteBatch);
            playAgain.Draw(spriteBatch);
            mousePointer.Draw(spriteBatch);
        }

    }
}

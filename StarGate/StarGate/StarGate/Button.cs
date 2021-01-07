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
    class Button
    {
        public static int PIXELS_PER_CHARACTER = 10;
        public static SpriteFont font;
        public static Texture2D buttonImage;

        public static double paddingConstant = 2;

        public Rectangle location;
        public String text;
        public Vector2 textLocation;

        public Button(Rectangle location, String text)
        {
            this.location = location;
            this.text = text;
            double y = location.Y + (location.Height - PIXELS_PER_CHARACTER * paddingConstant) / 2;
            double x = location.X + (location.Width - PIXELS_PER_CHARACTER * text.Length) / 2;
            this.textLocation = new Vector2((float)x, (float)y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonImage, location, Color.Black);
            spriteBatch.DrawString(font, text, textLocation, Color.White);
        }

        public bool checkForMouseIntersect(float x, float y)
        {
            return x > location.Left && x < location.Right && y > location.Top && y < location.Bottom;
        }

    }


    class LabelPrompt
    {
        public static SpriteFont font;

        public Vector2 location;
        public String text;

        public LabelPrompt(Vector2 location, String text)
        {
            this.location = location;
            this.text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, location, Color.Black);

        }


    }
}

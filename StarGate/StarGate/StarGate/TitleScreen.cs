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
        public Texture2D titleImage;
        public Vector2 titleImageContainer;
        public Rectangle sourceRectangle;
        
        public TitleScreen(GraphicsDeviceManager graphics)
        {
            titleImage = null;
            titleImageContainer = new Vector2(125, 225);
            sourceRectangle = new Rectangle(0, 0, 0, 50);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
        } 

        public void loadTitleScreenImage(Microsoft.Xna.Framework.Game game)
        {
            titleImage = game.Content.Load<Texture2D>("starGateTitle");
        }

        public void Update()
        {
            if (sourceRectangle.Width < titleImage.Width) sourceRectangle.Width++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleImage, titleImageContainer, sourceRectangle, Color.White);
        }
    }
}

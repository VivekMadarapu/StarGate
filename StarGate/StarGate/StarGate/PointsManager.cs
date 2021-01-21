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
    public class PointsManager
    {
        public static SpriteFont scoreFont;
        public static SpriteFont mainScoreFont;
        public static Color[] colors = new Color[] { Color.White, Color.Red, Color.Blue, Color.Yellow };
        public int score;
        public List<PointScore> pointValues;

        public PointsManager()
        {
            score = 0;
            pointValues = new List<PointScore>();    
        }

        public static void loadScoreFont(Microsoft.Xna.Framework.Game game)
        {
            scoreFont = game.Content.Load<SpriteFont>("GameScoreFont");
            mainScoreFont = game.Content.Load<SpriteFont>("MainScoreFont");
        }

        public void addPointsScored(int newScore, int x, int y)
        {
            pointValues.Add(new PointScore(newScore, x, y, colors[new Random().Next(0, colors.Length)]));
            score += newScore;
        }

        public void Update(Terrain terrain, SpaceShip ship, GamePadState newPad)
        {
            for (int i = 0; i < pointValues.Count; i++)
            {
                relationalUpdate(pointValues[i], terrain, ship, newPad);
                pointValues[i].timeTicks--;
                if (pointValues[i].timeTicks <= 0)
                {
                    pointValues.RemoveAt(i);
                    i--;
                }
            }
        }

        public void relationalUpdate(PointScore pointScore, Terrain terrain, SpaceShip ship, GamePadState newPad)//changes landers position in relation to the spaceship
        {
            if (newPad.Triggers.Left != 0 && terrain.bound != 0 && terrain.bound != 4200)
            {
                if (ship.isRight && ship.desRect.X == 400)
                {
                    if (terrain.bound < 4200)
                    {
                        pointScore.location.X -= SpaceShip.SPEED;
                    }
                }
                else
                {
                    if (terrain.bound > 0 && ship.desRect.X == 400)
                    {
                        pointScore.location.X += SpaceShip.SPEED;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(mainScoreFont, "Score: " + score, new Vector2(10, 10), Color.White);
            for (int i = 0; i < pointValues.Count; i++)
            {
                spriteBatch.DrawString(scoreFont, "" + pointValues[i].score, pointValues[i].location, pointValues[i].color);
            }
        }

    }

    public class PointScore
    {
        public int score;
        public Vector2 location;
        public int timeTicks;
        public Color color;

        public PointScore(int score, int x, int y, Color color)
        {
            this.score = score;
            this.location = new Vector2(x, y);
            this.timeTicks = 45;
            this.color = color;
        }
    }
}

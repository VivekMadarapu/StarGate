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
    public class GameScreen
    {
        public SpaceShip ship;
        public Terrain terrain;
        Gate gate;

        //enemies
        public List<Enemy> enemies = new List<Enemy>();
        Type lander;
        Type bomber;
        Type mutant;

        //humanoids
        public List<Humanoid> humanoids = new List<Humanoid>();
       //winning losing conditions
        Boolean isWon = false;
        Boolean isLost = false;

        //score
        public PointsManager pointsManager;


        public void initializeGameObjects(Microsoft.Xna.Framework.Game game)
        {

           pointsManager = new PointsManager();

            ship = new SpaceShip(game.Content.Load<Texture2D>("starGateAllSprites"), 
                800, 500, new Texture2D(game.GraphicsDevice, 1, 1));

            terrain = new Terrain(5000, 500, new Texture2D(game.GraphicsDevice, 1, 1));
            terrain.GenerateTerrain();
            gate = new Gate(game.Content.Load<Texture2D>("Stargate"));

            //enemies
            lander = typeof(Lander);
            bomber = typeof(Bomber);
            mutant = typeof(Mutant);

            Random r = new Random();
            for(int i=0; i<10; i++)
            {
                enemies.Add(new Lander(game,r));
            }
            for(int i=0; i<8; i++)
            {
                enemies.Add(new Bomber(game, new Random(i*345)));
            }
            
            //humnoids
            Humanoid.loadContent(game);
            for(int i=0; i<10; i++)
            {
                humanoids.Add(new Humanoid(game.GraphicsDevice, terrain, null));
            }
           
        }

        public void Update(Game1 game, GraphicsDevice graphicsDevice, GamePadState newPad, GamePadState oldPad, double gameTime)
        {

            if (!ship.isDead && isWon == false)
            {

                ship.Update(oldPad, newPad, terrain);
                terrain.Update(newPad, ship, graphicsDevice.Viewport.Width);
                gate.Update(enemies, humanoids, terrain, ship, newPad);
                for (int i = 0; i < humanoids.Count; i++)
                    humanoids[i].Update(graphicsDevice, terrain, ship, newPad, pointsManager);
                //updating enemies
                for (int i = 0; i < enemies.Count; i++)
                {
                    Type a = enemies[i].GetType();
                    if (a.Equals(lander))
                    {
                        Lander l = (Lander)enemies[i];
                        l.Update(humanoids, ship, newPad, terrain);
                        if (l.transform == true)
                        {
                            for (int r = 0; r < humanoids.Count; r++)
                            {
                                if (humanoids[r].carryer == enemies[i])
                                    humanoids[r].alive = false;
                            }
                            enemies[i] = new Mutant(l.desRect, game);
                        }
                        else
                        {
                            enemies[i] = l;

                        }

                    }
                    else if (a.Equals(bomber))
                    {
                        Bomber b = (Bomber)(enemies[i]);
                        b.rand = new Random(b.desRect.X);
                        b.Update(gameTime, ship, newPad, terrain);
                        enemies[i] = b;
                    }
                    else if (a.Equals(mutant))
                    {
                        Mutant m = (Mutant)(enemies[i]);
                        m.Update(ship, newPad, terrain);
                        enemies[i] = m;
                    }
                }

                //checking if shipProjectiles hit anything
                for (int i = 0; i < ship.projectileList.Count; i++)
                {
                    for (int r = 0; r < enemies.Count; r++)
                    {
                        Type a = enemies[r].GetType();
                        if (a.Equals(lander))
                        {
                            Lander l = (Lander)enemies[r];

                            if (ship.projectileList[i].hits(l.desRect))
                            {


                                //score += Lander.POINTS;
                                pointsManager.addPointsScored(Lander.POINTS, l.desRect.X, l.desRect.Y);

                                if (l.caughtHumanoid != null) l.caughtHumanoid.setCarryer(null);
                                enemies.RemoveAt(r);
                                r--;
                            }
                        }
                        else if (a.Equals(bomber))
                        {
                            Bomber b = (Bomber)(enemies[r]);

                            if (ship.projectileList[i].hits(b.desRect))
                            {
                                //score += Bomber.POINTS;
                                pointsManager.addPointsScored(Bomber.POINTS, b.desRect.X, b.desRect.Y);
                                enemies.RemoveAt(r);
                                r--;
                            }
                        }
                        else if (a.Equals(mutant))
                        {
                            Mutant m = (Mutant)(enemies[r]);
                            if (ship.projectileList[i].hits(m.desRect))
                            {
                                //score += Mutant.POINTS;
                                pointsManager.addPointsScored(Mutant.POINTS, m.desRect.X, m.desRect.Y);
                                enemies.RemoveAt(r);
                                r--;
                            }
                        }
                    }
                }

                //check if spaceship catches humanoids
                for (int i = 0; i < humanoids.Count; i++)
                {
                    if (!humanoids[i].caught && humanoids[i].y < terrain.terrainContour[Math.Abs(terrain.bound + humanoids[i].x)] &&
                        humanoids[i].container.Intersects(ship.desRect) && !humanoids[i].droppedByHumanoids)
                    {
                        ship.addHumanoid(humanoids[i]);
                        pointsManager.addPointsScored(500, ship.desRect.X, ship.desRect.Y);
                    }
                }
                //loss conditions
                //checks if spaceship dies from collision with alien
                for (int i = 0; i < enemies.Count; i++)
                {
                    Type a = enemies[i].GetType();
                    if (a.Equals(lander))
                    {
                        Lander l = (Lander)(enemies[i]);
                        if (ship.collidesWithShip(l.desRect))
                        {
                            ship.isDead = true;

                        }
                        if (ship.collidesWithProjectiles(l.projectileList))
                            ship.isDead = true;

                    }
                    else if (a.Equals(bomber))
                    {
                        Bomber b = (Bomber)enemies[i];
                        if (ship.collidesWithShip(b.desRect))
                        {
                            ship.isDead = true;
                        }
                        for (int n = 0; n < b.fireballs.Count; n++)
                        {
                            if (ship.desRect.Contains(b.fireballs[n].desRect))
                            {
                                ship.isDead = true;
                                break;
                            }
                        }

                    }
                    else if (a.Equals(mutant))
                    {
                        Mutant m = (Mutant)enemies[i];
                        if (ship.collidesWithShip(m.desRect))
                        {
                            ship.isDead = true;
                        }
                        if (ship.collidesWithProjectiles(m.projectileList))
                            ship.isDead = true;
                    }
                }
                //win conditions
                if (enemies.Count == 0)
                    isWon = true;
            }
            else if (ship.deathTimer > 0 && ship.isDead)
            {
                if (ship.deathTimer < 400)
                {
                    game.gameState = GameState.END_SCREEN;
                    game.endingScreen.score.text = "Score: " + pointsManager.score;
                }
                isLost = true;
                while (ship.deathPixelCount < 40)
                    ship.updateDeath();

                ship.updateDeath();
            }
            else
            {
                game.gameState = GameState.END_SCREEN;
                if (!ship.isDead && !isLost) game.endingScreen.score.text = "You Won - Score: " + pointsManager.score;
            }

            pointsManager.Update(terrain, ship, newPad);

        }

        public void Draw(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            if (game.gameState != GameState.END_SCREEN) pointsManager.Draw(spriteBatch);
            ship.Draw(spriteBatch);
            terrain.Draw(spriteBatch, Color.White, graphicsDevice.Viewport.Width);
            if (!gate.used)
            {
                gate.Draw(spriteBatch);
            }
    
            //humanoids
            for (int i = 0; i < humanoids.Count; i++)
                humanoids[i].Draw(spriteBatch);
  
            //enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                Type a = enemies[i].GetType();
                if (a.Equals(lander))
                {
                    Lander l = (Lander)enemies[i];
                    l.Draw(spriteBatch);
                
                }
                else if (a.Equals(bomber))
                {
                    Bomber b = (Bomber)(enemies[i]);
                    b.Draw(spriteBatch);
                }
                else if (a.Equals(mutant))
                {
                    Mutant m = (Mutant)(enemies[i]);
                    m.Draw(spriteBatch);
                }
            }
        }
    }
}

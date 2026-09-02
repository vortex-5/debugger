using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace AnyKey
{
    public class Menu
    {
        private Game1 Game;

        public MenuItems SelectedItem;

        private Texture2D Background;
        private Texture2D EscTexture;
        private Texture2D[] MenuItemTextures;
        private Texture2D CheeseTexture;
        private Texture2D TitleTexture;
        private SpriteFont InfoFont;

        private MenuStates State;

        private int SelectedLevel;
        private string[] Levels;
        private SpriteFont LevelFont;

        public bool Paused;

        public enum MenuItems
        {
            Resume=0,
            NewGame,
            LoadLevel,
            Info
        }

        private enum MenuStates
        {
            Menu,
            LoadLevel,
            Info
        }

        public Menu(Game1 game)
        {
            Game = game;

            SelectedItem = MenuItems.NewGame;
            SelectedLevel = 0;

            State = MenuStates.Menu;
            Paused = false;

            Background = Game.Content.Load<Texture2D>("picnic");

            MenuItemTextures = new Texture2D[4];
            MenuItemTextures[(int)MenuItems.Resume] = Game.Content.Load<Texture2D>("resume");
            MenuItemTextures[(int)MenuItems.NewGame] = Game.Content.Load<Texture2D>("new");
            MenuItemTextures[(int)MenuItems.LoadLevel] = Game.Content.Load<Texture2D>("load");
            MenuItemTextures[(int)MenuItems.Info] = Game.Content.Load<Texture2D>("info");
            EscTexture = Game.Content.Load<Texture2D>("esc");
            CheeseTexture = Game.Content.Load<Texture2D>("cheese");
            TitleTexture = game.Content.Load<Texture2D>("debugger");

            InfoFont = Game.Content.Load<SpriteFont>("CourierNew");
            LevelFont = Game.Content.Load<SpriteFont>("CourierNew");
        }

        public Game1.GameState Update()
        {
            if (State == MenuStates.Menu)
            {
                // change selected item if up or down pressed
                if (Input.KeysHit.Contains(Keys.Up))
                    SelectedItem--;
                if (Input.KeysHit.Contains(Keys.Down))
                    SelectedItem++;
                if (Paused)
                {
                    if (SelectedItem > MenuItems.Info)
                        SelectedItem = MenuItems.Resume;
                    if (SelectedItem < MenuItems.Resume)
                        SelectedItem = MenuItems.Info;
                }
                else
                {
                    if (SelectedItem > MenuItems.Info)
                        SelectedItem = MenuItems.NewGame;
                    if (SelectedItem < MenuItems.NewGame)
                        SelectedItem = MenuItems.Info;
                }

                // test for selection choice
                if (Input.KeysHit.Contains(Keys.Enter))
                {
                    if (SelectedItem == MenuItems.Resume)
                    {
                        Paused = false;

                        return Game1.GameState.InGame;
                    }
                    else if (SelectedItem == MenuItems.NewGame)
                    {
                        Levels = Level.all_Levels(Game);

                        Game.level_Name = Levels[0];
                        return Game1.GameState.NewGame;
                    }
                    else if (SelectedItem == MenuItems.LoadLevel)
                    {
                        State = MenuStates.LoadLevel;

                        Levels = Level.all_Levels(Game);
                    }
                    else if (SelectedItem == MenuItems.Info)
                        State = MenuStates.Info;
                }

                if (Input.KeysHit.Contains(Keys.F1))
                {
                    Game.CreditsY = 500;
                    return Game1.GameState.Complete;
                }

                if (Input.KeysHit.Contains(Keys.Escape))
                    return Game1.GameState.Quit;

            }
            else if (State == MenuStates.LoadLevel)
            {
                if (Input.KeysHit.Contains(Keys.Escape))
                    State = MenuStates.Menu;

                // change selected item if up or down pressed
                if (Input.KeysHit.Contains(Keys.Up))
                    SelectedLevel--;
                if (Input.KeysHit.Contains(Keys.Down))
                    SelectedLevel++;
                if (SelectedLevel > Levels.Length-1)
                    SelectedLevel = 0;
                if (SelectedLevel < 0)
                    SelectedLevel = Levels.Length-1;

                // test for selection choice
                if (Input.KeysHit.Contains(Keys.Enter))
                {
                    Game.level_Name = Levels[SelectedLevel];
                    State = MenuStates.Menu;
                    return Game1.GameState.NewGame;
                }
            }
            else if (State == MenuStates.Info)
            {
                if (Input.KeysHit.Contains(Keys.Escape))
                    State = MenuStates.Menu;
            }

            return Game.State;
        }

        public void Draw(bool drawBackground)
        {
            if (drawBackground || State == MenuStates.LoadLevel || State == MenuStates.Info)
            {
                if (Background == null)
                    Game.graphics.GraphicsDevice.Clear(Color.Black);
                else
                {
                    Game.spriteBatch.Begin();
                    Game.spriteBatch.Draw(Background, new Rectangle(0, 0, 1000, 500), Color.White);
                    Game.spriteBatch.End();
                }
            }

            if (State == MenuStates.Menu)
            {
                Game.spriteBatch.Begin();

                Game.spriteBatch.Draw(TitleTexture, new Vector2(150, -10), Color.White);

                Game.spriteBatch.Draw(EscTexture, new Vector2(20.0f, 20.0f), Color.White);
                if (Paused)
                    Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.Resume], new Vector2(200.0f, 100.0f), Color.White);
                Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.NewGame], new Vector2(200.0f, 180.0f), Color.White);
                Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.LoadLevel], new Vector2(200.0f, 260.0f), Color.White);
                Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.Info], new Vector2(200.0f, 340.0f), Color.White);

                Vector2 cheesePos = new Vector2(130f, 80f * (int)SelectedItem + 100f);
                Game.spriteBatch.Draw(CheeseTexture, cheesePos, new Rectangle(64, 0, 64, 64), Color.White);
                
                Game.spriteBatch.End();
            }
            else if (State == MenuStates.LoadLevel)
            {
                Game.spriteBatch.Begin();

                Game.spriteBatch.Draw(EscTexture, new Vector2(20.0f, 20.0f), Color.White);
                Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.LoadLevel], new Vector2(280.0f, 100.0f), Color.White);

                int rows = 9;
                for (int i = 0; i < Levels.Length; i++)
                {
                    int x = i / rows;
                    int y = i % rows;

                    Game.spriteBatch.DrawString(LevelFont, Levels[i], new Vector2(100f + x * 300f + 3f, 203f + 30f * y), new Color(128, 128, 128, 128),
                                            0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.5f);
                    Game.spriteBatch.DrawString(LevelFont, Levels[i], new Vector2(100f + x * 300f, 200f + 30f * y), Color.Yellow,
                                            0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.5f);
                }

                int cx = SelectedLevel / rows;
                int cy = SelectedLevel % rows;
                Vector2 cheesePos = new Vector2(30f + cx*300f, 180f + 30f * cy);
                Game.spriteBatch.Draw(CheeseTexture, cheesePos, new Rectangle(128, 0, 64, 64), Color.White);
                Game.spriteBatch.Draw(TitleTexture, new Vector2(150, -10), Color.White);

                Game.spriteBatch.End();
            }
            else if (State == MenuStates.Info)
            {
                Game.spriteBatch.Begin();

                Game.spriteBatch.Draw(EscTexture, new Vector2(20.0f, 20.0f), Color.White);
                Game.spriteBatch.Draw(MenuItemTextures[(int)MenuItems.Info], new Vector2(280.0f, 100.0f), Color.White);

                string infoText = "Those bugs are everywhere and now they're getting at the cheese! \n\n" +
                                  "Quickly SQUISH the bugs before they eat your cheese using your \n" +
                                  "keyboard!\n\n" +
                                  "-Game Modes-\n" +
                                  "Normal: Protect your cheese from all the bugs in the level \n" +
                                  "Survival: Cheese must survive for a set time period \n" +
                                  "Timed Completion: You must squish all bugs within the alloted time\n\n" +
                                  "-Powerups-\n" + 
                                  "Cheese Restore, Massive Damage, Area of Effect, Bonus Cheese";

                Game.spriteBatch.DrawString(InfoFont, infoText, new Vector2(103f, 203f), new Color(128, 128, 128, 128),
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.5f);
                Game.spriteBatch.DrawString(InfoFont, infoText, new Vector2(100f, 200f), Color.Blue,
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.5f);
                Game.spriteBatch.Draw(TitleTexture, new Vector2(150, -10), Color.White);


                Game.spriteBatch.End();
            }
        }
    }
}

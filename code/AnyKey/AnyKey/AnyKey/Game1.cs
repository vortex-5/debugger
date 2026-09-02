using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;



namespace AnyKey
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private SpriteFont EndTitleFont;
        private SpriteFont ScoreFont;
        private Texture2D Square;
        private Texture2D EndGameBackground;

        public GameState State;

        private Menu Menu;

        public Level Level;
        public String level_Name;
        private Level.WinState LevelState;
        private int LevelScore;
        private bool LevelOver;
        public int CreditsY;

        //XML File location and file name variables to be set
        public string xml_loc = "";
        public XMLSettingsFile xml_file;

        private string EndGameCredits = "Team - Any Key -\n\nJason \"Galaximo\" Bourne\nChris \"Drunk Dormouse\" Brown\nMike \"Something Japanese\" Darmitz\nWilliam \"Goat Voyeur\" Hua\nFanFan \"Vortex\" Huang";


        public enum GameState
        {
            Menu,
            NewGame,
            InGame,
            LevelOver,
            Paused,
            Complete,
            Quit
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 500;

            Content.RootDirectory = "Content";

            Sound.Initialize("..\\..\\..\\Content");

            State = GameState.Menu;
            LevelOver = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Input.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Opens the XML file for settings and level information
            open_XML();
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Square = Content.Load<Texture2D>("square");
            EndGameBackground = Content.Load<Texture2D>("credits");
            CreditsY = 500;

            ScoreFont = Content.Load<SpriteFont>("CourierNew");
            EndTitleFont = Content.Load<SpriteFont>("Impact");

            Menu = new Menu(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            
            Input.Update();

            if (State == GameState.Menu)
            {
                if (!Sound.isSongPlaying())
                    Sound.PlaySong(song.MENU);

                State = Menu.Update();

                if (State != GameState.Menu)
                    Sound.StopSong();
            }
            else if (State == GameState.NewGame)
            {
                Level = new Level(this);
                Level.load_Level(level_Name);
                LevelState = Level.WinState.InProgress;
                LevelOver = false;

                Sound.StopSong();
                Sound.PlaySoundClip(soundClip.TTC);

                State = GameState.InGame;
            }
            else if (State == GameState.InGame)
            {
                if (LevelOver)
                {
                    State = GameState.LevelOver;
                    return;
                }

                // if press ESC, go back to menu
                if (Input.KeysHit.Contains(Keys.Escape))
                {
                    Menu.SelectedItem = Menu.MenuItems.Resume;
                    State = GameState.Paused;
                    Sound.StopSong();
                }

                Level.Update(elapsedTime);

                LevelState = Level.IsComplete();
                if (LevelState == Level.WinState.Success || LevelState == Level.WinState.Perfect || LevelState == Level.WinState.Failure)
                {
                    LevelScore = Level.getScore();
                    State = GameState.LevelOver;
                }
            }
            else if (State == GameState.Paused)
            {
                if (!Sound.isSongPlaying())
                    Sound.PlaySong(song.MENU);

                Menu.Paused = true;
                State = Menu.Update();

                if (State != GameState.Paused)
                    Sound.StopSong();
            }
            else if (State == GameState.LevelOver)
            {
                LevelOver = true;
                Level.Update(elapsedTime);

                // if press ESC, go back to menu
                if(Input.KeysHit.Contains(Keys.Escape)){
                    Menu.SelectedItem = Menu.MenuItems.Resume;
                    State = GameState.Paused;
                    Sound.StopSong();
                }
                if (Input.KeysHit.Contains(Keys.Enter))
                {
                    if (LevelState == Level.WinState.Success || LevelState == Level.WinState.Perfect)
                    {
                        level_Name = Level.NextLevel(level_Name);
                        if (level_Name == "")
                            State = GameState.Menu;
                        else if (level_Name == "complete")
                        {
                            CreditsY = 500;
                            State = GameState.Complete;
                        }
                        else
                            State = GameState.NewGame;
                    }
                    else
                        State = GameState.NewGame;
                }

            }
            else if (State == GameState.Complete)
            {
                if (!Sound.isSongPlaying())
                    Sound.PlaySong(song.MENU);

                if (gameTime.TotalGameTime.Ticks % 10 == 0)
                    CreditsY -= 1;

                if (Input.KeysHit.Contains(Keys.Escape))
                    State = GameState.Menu;

                if (State != GameState.Complete)
                    Sound.StopSong();
            }
            else if (State == GameState.Quit)
            {
                this.Exit();
            }


            base.Update(gameTime);

            //Update the sound engine
            Sound.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (State == GameState.Menu)
            {
                Menu.Draw(true);
            }
            else if (State == GameState.InGame)
            {
                Level.Draw();
            }
            else if (State == GameState.Paused)
            {
                Level.Draw();
                Menu.Draw(false);
            }
            else if (State == GameState.LevelOver)
            {
                string title = "Level Over";
                
                if (LevelState == Level.WinState.Failure)
                    title = "You Failed!";
                else if (LevelState == Level.WinState.Success)
                    title = "You Beat the Level!";
                else if (LevelState == Level.WinState.Perfect)
                    title = "Flawless Debugging!";

                Level.Draw();

                spriteBatch.Begin();
                
                spriteBatch.Draw(Square, new Rectangle(350, 80, 300, 200), new Color(0, 0, 0, 160));

                spriteBatch.DrawString(EndTitleFont, title, new Vector2(500 - ScoreFont.MeasureString(title).Length() / 2, 100f), new Color(0, 255, 0),
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(ScoreFont, "Score: " + LevelScore, new Vector2(370f, 150f), new Color(255, 0, 0),
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);

                //Display a different instructions depending on whether player is a loser or winner 
                if (LevelState == Level.WinState.Failure)
                {
                    spriteBatch.DrawString(ScoreFont, "Press enter to repeat the", new Vector2(360f, 195f), Color.White);
                    spriteBatch.DrawString(ScoreFont, "current level or escape", new Vector2(360f, 215f), Color.White);
                    spriteBatch.DrawString(ScoreFont, "to return to the main menu", new Vector2(360f, 235f), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(ScoreFont, "Press enter to go to the", new Vector2(360f, 195f), Color.White);
                    spriteBatch.DrawString(ScoreFont, "next level or escape to", new Vector2(360f, 215f), Color.White);
                    spriteBatch.DrawString(ScoreFont, "return to the main menu", new Vector2(360f, 235f), Color.White);
                }


                spriteBatch.End();
            }
            else if (State == GameState.Complete)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(EndGameBackground, new Rectangle(0, 0, 1000, 500), Color.White);

                spriteBatch.DrawString(ScoreFont, EndGameCredits, new Vector2(350f+1f, (float)CreditsY+1f), new Color(100, 100, 100),
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(ScoreFont, EndGameCredits, new Vector2(350f, (float)CreditsY), new Color(0, 0, 255),
                                        0, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);

                spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        private void open_XML()
        {
            try
            {
                //Get current application path
                System.IO.DirectoryInfo ProjectPath = System.IO.Directory.GetParent(System.Reflection.Assembly.GetCallingAssembly().Location);

                //sets xml location path and loads the xml file
                xml_loc = "Settings.xml";
                xml_file = new XMLSettingsFile(xml_loc);
            }
            catch (System.NullReferenceException)
            {
                throw new Exception("Settings file cannot be found.\n\nCreate an empty Settings file?");
                {
                    //Revert default setting file to empty settings file
                    //new_XML();
                }
            }
        }

    }
}

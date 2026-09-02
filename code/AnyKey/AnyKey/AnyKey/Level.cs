using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public class Level
    {
        public enum Type
        {
            BugLimit, TimeLimit, BugTimeLimit
        };

        public enum WinState
        {
            Success, Failure, InProgress, Perfect
        };

        
        public Game1 parent;
        private Texture2D bg;
        private Texture2D blank;
        private List<Bug> bugs;
        private AttackMapper input;
        public Type mode;
        public int level_Time = 0;
        int level_Time_Passed = 0;



        private song Song;

        public Collision collideCheck;

        //Counter for the player's score
        private int score;

        //Stores whether the player played a perfect game
        bool cheeseTacular = true;

        public AttackMapper attacks
        {
            get
            {
                return input;
            }
        }

        public string background
        {
            set
            {
                bg = parent.Content.Load<Texture2D>(value);
            }
        }

        private List<SpawnArea> spawns;

        public List<Cheese> cheeses;

        public Level(Game1 g)
        {
            parent = g;
            mode = Type.BugLimit;
            bugs = new List<Bug>();
            cheeses = new List<Cheese>();
            spawns = new List<SpawnArea>();
            input = new AttackMapper(parent.Content,
                                     parent.Window.ClientBounds.Width,
                                     parent.Window.ClientBounds.Height);

            collideCheck = new Collision(g);
            score = 0;

        }

        //Check if the level has been completed (won or lost)
        public WinState IsComplete()
        {
            //TODO: GODMODE CHEATCODE allows level to run to finish
            //return WinState.InProgress;


            // check for failure
            bool failure = false;
            if (mode == Type.BugLimit ||mode == Type.TimeLimit)
            {
                if (cheeses.Count <= 0)
                    failure = true;
            }
            else if ( mode == Type.BugTimeLimit)
            {
                if (cheeses.Count <= 0 || level_Time_Passed >= level_Time)
                    failure = true;
            }

            if (failure)
            {
                //Stop the level music and play the failure music
                Sound.StopSong();
                Sound.PlaySong(song.FAILURE);
                return WinState.Failure;
            }

            //Check if all spawns have finished spawning
            bool allSpawnsDone = true;
            foreach (SpawnArea s in spawns)
            {
                //Check if any spawn area is not yet depleated
                if (!s.IsDone())
                {
                    allSpawnsDone = false;
                    break;
                }
            }
            
            //Check if all remaining bugs are squished
            bool allSquish = true;
            foreach (Bug b in bugs)
            {
                if (!b.IsDying())
                {
                    allSquish = false;
                }
            }

            // check for completion
            bool complete = false;
            if (mode == Type.BugLimit || mode == Type.BugTimeLimit)
            {
                if (allSpawnsDone && (bugs.Count == 0 || allSquish))
                    complete = true;
            }
            else if (mode == Type.TimeLimit)
            {
                if (level_Time_Passed >= level_Time)
                    complete = true;
            }

            //If the level is done, compute the score and return success
            if (complete)
            {   
                //Stop the level sound and play the victory song
                Sound.StopSong();
                Sound.PlaySong(song.VICTORY);

                float factor = 0;
                //Add on the score for each cheese, giving a bonus for any full cheeses
                foreach (Cheese c in cheeses)
                {
                    if (c.getHealth() < 1.0f && c.getHealth() > 0.0f)
                    {
                        cheeseTacular = false;
                        factor += c.getHealth();
                    }
                    //Give double score for a full cheese
                    else if (c.getHealth() >= 1.0f)
                        factor += c.getHealth() * 2.0f;
                }

                score = (int)((factor + 1.0) * score);

                //Return success, or perfect for a perfect game
                if (cheeseTacular)
                {
                    score += 100;
                    return WinState.Perfect;
                }
                else
                {
                    return WinState.Success;
                }
            }
            return WinState.InProgress;
        }

        //Return the game score
        public int getScore()
        {
            return score;
        }

        public void Draw()
        {
            parent.spriteBatch.Begin();

            if (bg == null)
                parent.GraphicsDevice.Clear(Color.Black);
            else
                parent.spriteBatch.Draw(bg, new Vector2(), Color.White);

            foreach (SpawnArea area in spawns)
                area.Draw(parent);

            foreach (Cheese c in cheeses)
                c.Draw(parent.spriteBatch);

            int bosses = 0;

            foreach (Bug b in bugs)
            {
                if (!b.IsAlive())
                    b.Draw(parent.spriteBatch);
            }

            foreach (Bug b in bugs)
            {
                if (b.IsAlive())
                    b.Draw(parent.spriteBatch);

                if (b is Boss)
                {
                    Boss boss = (Boss)b;

                    if (blank == null)
                        blank = parent.Content.Load<Texture2D>("square");

                    int divide = (int)(800 * boss.getHealthPercentage());

                    parent.spriteBatch.Draw(blank, new Rectangle(95, 48 + bosses * 20, 810, 14), new Color(64, 64, 64, 64));
                    parent.spriteBatch.Draw(blank, new Rectangle(100, 50 + bosses * 20, divide, 10), new Color(255, 255, 128, 128));
                    parent.spriteBatch.Draw(blank, new Rectangle(100 + divide, 50 + bosses * 20, 800 - divide, 10), new Color(128, 128, 255, 128));

                    bosses++;
                }
            }

            if (mode == Type.TimeLimit)
            {
                parent.spriteBatch.DrawString(parent.Content.Load<SpriteFont>("CourierNew"), "Survive For:", new Vector2(10f, 415f), Color.Red, 0, new Vector2(0f, 0f), 1.5f, SpriteEffects.None, 0.5f);
            }
            else if (mode == Type.BugTimeLimit)
            {
                parent.spriteBatch.DrawString(parent.Content.Load<SpriteFont>("CourierNew"), "Time Limit:", new Vector2(10f, 415f), Color.Red, 0, new Vector2(0f, 0f), 1.5f, SpriteEffects.None, 0.5f);
            }

            //Puts clock on screen
            if (mode == Type.TimeLimit || mode == Type.BugTimeLimit)
            {
                string temp_time;
                if ((level_Time - level_Time_Passed) < 100)
                {
                    temp_time = "0";
                }
                else
                {
                    int temp_calc_time = level_Time - level_Time_Passed;
                    int temp_result_div = (temp_calc_time/1000)/60;
                    int temp_result_rem = (temp_calc_time/1000) % 60;

                    temp_time = temp_result_div.ToString() + temp_result_rem.ToString().PadLeft(2, '0') + temp_calc_time.ToString().Substring(temp_calc_time.ToString().Length - 3, 2);
                }
                //Add extra 0's to the left side to fill
                temp_time = temp_time.PadLeft(6, '0');
                //adds 0's to time if not using full 6 digits

                temp_time = temp_time.Insert(4, ":");
                temp_time = temp_time.Insert(2, ":");
                parent.spriteBatch.DrawString(parent.Content.Load<SpriteFont>("Impact"), temp_time, new Vector2(10f, 435f), Color.Red, 0, new Vector2(0f, 0f), 2.0f, SpriteEffects.None, 0.5f);
            }
            
            input.Draw(parent.spriteBatch);
            
            parent.spriteBatch.End();
        }

        public void Update(int elapsedTime)
        {
            level_Time_Passed += elapsedTime;

            if (!Sound.isSongPlaying() && parent.State == Game1.GameState.InGame)
                Sound.PlaySong(Song);

            input.Update(elapsedTime);

            List<Attack> atts = input.GetAllAttacks();

            for (int i=0; i < bugs.Count; i++)
            {
                Bug b = bugs[i];

                b.Update(elapsedTime);

                foreach (Attack att in atts)
                    b.TestHit(att);
                
                if (!b.IsAlive()) 
                {
                    score += b.getScoreOnce();

                    if (!b.IsDying())
                    {
                        bugs.Remove(b);
                        i--;
                    }
                }
            }

            foreach (Cheese c in cheeses)
                c.Update();

            for (int i = 0; i < cheeses.Count; i++)
            {
                if (!cheeses[i].IsAlive())
                {
                    cheeses.RemoveAt(i);
                    i--;
                    cheeseTacular = false;
                }
            }

            foreach (SpawnArea area in spawns)
                area.Update(elapsedTime);
        }

        public void AddBug(Bug b)
        {
            bugs.Add(b);
        }

        public void AddSpawn(SpawnArea area)
        {
            spawns.Add(area);
        }

        public string[] all_Levels()
        {
            try
            {
                //returns an array of strings of all levels that exist in the XML file in a window
                return parent.xml_file.GetList("allLevels", "levels");
            }
            catch
            {
                throw new Exception("FAILED TO READ LEVELS");
            }
        }
        public static string[] all_Levels(Game1 game)
        {
            try
            {
                //returns an array of strings of all levels that exist in the XML file in a window
                return game.xml_file.GetList("allLevels", "levels");
            }
            catch
            {
                throw new Exception("FAILED TO READ LEVELS");
            }
        }

        public void load_Level(string level_in)
        {
            //Resets the time past in level to 0
            level_Time_Passed = 0;

            string level_name =  level_in.Trim().Replace(" ", "_");

            //Loads level called "level_name"
            try
            
            {
                bg = parent.Content.Load<Texture2D>(parent.xml_file.GetValue(level_name, "bg"));
                Song = (song)int.Parse(parent.xml_file.GetValue(level_name, "song"));
                mode = (Type)int.Parse(parent.xml_file.GetValue(level_name, "type"));

                try
                {
                    input.Alpha = byte.Parse(parent.xml_file.GetValue(level_name, "keyboardAlpha"));
                }
                catch { }

                if (mode == Type.TimeLimit || mode == Type.BugTimeLimit)
                {
                    level_Time = int.Parse(parent.xml_file.GetValue(level_name, "time"));
                }

                string[] cheese_Health = parent.xml_file.GetList(level_name, "cheeseHealth");
                string[] cheese_RectX = parent.xml_file.GetList(level_name, "cheeseRectX");
                string[] cheese_RectY = parent.xml_file.GetList(level_name, "cheeseRectY");
                
                int cheese_Counter = 0;
                foreach (string health in cheese_Health)
                {
                    cheeses.Add(new Cheese(new Animation (parent.Content.Load<Texture2D>("cheese"), 64, 64, 4) , new Rectangle(int.Parse(cheese_RectX[cheese_Counter]), int.Parse(cheese_RectY[cheese_Counter]), 64,64), long.Parse(cheese_Health[cheese_Counter])));
                    cheese_Counter++;
                }
                 

                string[] spawn_Bug = parent.xml_file.GetList(level_name, "spawnBug");
                string[] spawn_Start = parent.xml_file.GetList(level_name, "spawnStart");
                string[] spawn_Stop = parent.xml_file.GetList(level_name, "spawnStop");
                string[] spawn_Total = parent.xml_file.GetList(level_name, "spawnTotal");
                string[] spawn_RectX = parent.xml_file.GetList(level_name, "spawnRectX");
                string[] spawn_RectY = parent.xml_file.GetList(level_name, "spawnRectY");
                string[] spawn_RectWidth = parent.xml_file.GetList(level_name, "spawnRectWidth");
                string[] spawn_RectHeight = parent.xml_file.GetList(level_name, "spawnRectHeight");

                int spawn_Counter = 0;
                foreach (string bug in spawn_Bug)
                {
                    spawns.Add(new SpawnArea(this, BugFactory.FromString(parent,this, spawn_Bug[spawn_Counter]), int.Parse(spawn_Start[spawn_Counter]), int.Parse(spawn_Stop[spawn_Counter]), int.Parse(spawn_Total[spawn_Counter])));
                    spawns[spawn_Counter].Rect = new Rectangle(int.Parse(spawn_RectX[spawn_Counter]), int.Parse(spawn_RectY[spawn_Counter]), int.Parse(spawn_RectWidth[spawn_Counter]), int.Parse(spawn_RectHeight[spawn_Counter]));
                    spawn_Counter++;
                }

                foreach (SpawnArea area in spawns)
                    if (area.Rect.Width == 0 && area.Rect.Height == 0)
                        input.Disable(area);
            }
            catch (Exception e)
            {
                //Occurs when information cannot properly be read from the xml file
                //ask if user wants to revert the default file to the original OR
                //Change default file to an existing file
                throw e;
                
            }
        }

        public string NextLevel(string currentLevelName)
        {
            string[] allLevs = all_Levels();
            for (int i=0; i < allLevs.Length; i++)
            {
                string lev = allLevs[i];

                if (lev == currentLevelName)
                {
                    if (i + 1 < allLevs.Length)
                        return allLevs[i + 1];
                    else
                        return "complete";
                }
            }

            return "";
        }
    }
}

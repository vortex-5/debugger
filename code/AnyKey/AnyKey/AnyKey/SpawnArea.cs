using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public class SpawnArea
    {
        public Bug BugProto;

        public int Start;
        public int Finish;
        public int TotalBugs;

        public Rectangle Rect;

        private int count;
        private int time;

        private Level level;
        private Random random;

        public SpawnArea(Level lev, Bug bug, int start, int finish, int totalBugs)
        {
            level = lev;
            BugProto = bug;
            Start = start;
            Finish = finish;
            TotalBugs = totalBugs;

            random = new Random();
            count = 0;
            time = 0;
        }

        public void Update(int dt)
        {
            time += dt;

            int target = TotalBugs * (time - Start);

            if (Finish > Start)
                target /= Finish - Start;
            else if (time >= Finish)
                target = TotalBugs;
            else target = 0;

            for (; count < target && count < TotalBugs; count++)
            {
                Bug bug = BugProto.Copy();

                bug.SetOrientation(new Vector2((float)(Rect.X + Rect.Width * random.NextDouble()),
                                               (float)(Rect.Y + Rect.Height * random.NextDouble())),
                                   random.NextDouble() * 2 * Math.PI);

                level.AddBug(bug);
            }
        }

        public void Draw(Game1 g)
        {
            if (BugProto.GetType() == typeof(Ant))
            {
                if (Rect.Width == 0 && Rect.Height == 0)
                    g.spriteBatch.Draw(g.Content.Load<Texture2D>("anthill"), new Vector2(Rect.X - 64, Rect.Y - 64), Color.White);
            }
        }

        public bool IsDone()
        {
            return count == TotalBugs;
        }
    }
}

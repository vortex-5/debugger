using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class MamaRoach : GenericBug, Boss
    {
        private const int MaxHealth = 40;

        private Rectangle ScreenSize;
        private double Curve;
        private int waiting;

        public MamaRoach(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = MaxHealth;
            score = 10000;
            speed = 10;
            angle = 0;

            animTime = 3;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 50, 75);

            ScreenSize = new Rectangle(0, 0, 1000, 500);
            waiting = 0;
            Curve = 0;
        }

        public double getHealthPercentage()
        {
            return (double)health / MaxHealth;
        }

        protected override void Move()
        {
            //TODO: Add move logic for the ant

            switch (state)
            {
                case BugState.Walking:
                    if (pos.X >= 0 && pos.Y >= 0 && pos.X < ScreenSize.Width && pos.Y < ScreenSize.Height)
                    {
                        angle += Curve;
                        waiting++;

                        if (waiting >= GetPeriod())
                        {
                            Bug baby = BugFactory.FromString(level.parent, level, "roach");
                            baby.SetOrientation(new Vector2(pos.X, pos.Y), rnd.NextDouble() * 2 * Math.PI);
                            level.AddBug(baby);
                            waiting = 0;
                        }
                    }

                    pos.X += (float)(speed * Math.Sin(angle));
                    pos.Y -= (float)(speed * Math.Cos(angle));

                    int border = GetBorder();

                    if (pos.X < -border || pos.Y < -border || pos.X > ScreenSize.Width + border || pos.Y > ScreenSize.Height + border)
                    {
                        Curve = rnd.NextDouble() * 0.06 - 0.03;

                        switch (rnd.Next(0, 4))
                        {
                            case 0:
                                pos.X = 10 - border;
                                pos.Y = rnd.Next(50, ScreenSize.Height - 50);
                                angle = Math.Atan2(ScreenSize.Width + border, rnd.Next(50, ScreenSize.Height - 50));
                                break;
                            case 1:
                                pos.X = ScreenSize.Width + border - 10;
                                pos.Y = rnd.Next(50, ScreenSize.Height - 50);
                                angle = Math.Atan2(-ScreenSize.Width - border, rnd.Next(50, ScreenSize.Height - 50));
                                break;
                            case 2:
                                pos.X = rnd.Next(50, ScreenSize.Width - 50);
                                pos.Y = 10 - border;
                                angle = Math.Atan2(rnd.Next(50, ScreenSize.Width - 50), -ScreenSize.Height + border);
                                break;
                            case 3:
                                pos.X = rnd.Next(50, ScreenSize.Width - 50);
                                pos.Y = ScreenSize.Height + border - 10;
                                angle = Math.Atan2(rnd.Next(50, ScreenSize.Width - 50), ScreenSize.Height + border);
                                break;
                        }
                    }

                    break;
            }
        }

        protected override void Eat()
        {
        }

        private int GetBorder()
        {
            if (health < 5)
                return 125;
            else if (health < 15)
                return 250;
            else if (health < 25)
                return 325;
            else if (health < 35)
                return 500;
            else return 750;
        }

        private int GetPeriod()
        {
            if (health < 15)
                return 10;
            else if (health < 25)
                return 15;
            else if (health < 35)
                return 20;
            else return 25;
        }
    }
}

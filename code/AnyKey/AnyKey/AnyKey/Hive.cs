using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    class Hive : Bug, Boss
    {
        public enum HiveState
        {
            Entering, Faster, Constant, Slower, Stopped, Releasing
        };

        private const int MaxHealth = 100;

        private Texture2D Shading;
        private Animation Anim;
        private Level Level;
        private int Health;

        private bool Scored;
        private Vector2 Position;
        private double Angle;

        private Vector2 Target;
        private Vector2 Velocity;
        private double Angular;

        private int Timer;
        private int Released;
        private bool Vulnerable;
        private HiveState State;

        private int Queens;
        private Random Random;

        public Hive(Level l)
        {
            Shading = l.parent.Content.Load<Texture2D>("hivelight");
            Anim = new Animation(l.parent.Content.Load<Texture2D>("hive"), 200, 200, 5);
            Level = l;
            Health = MaxHealth;

            Scored = false;
            Position = new Vector2();
            Angle = 0;

            Target = new Vector2(500, 250);
            Velocity = new Vector2();
            Angular = 0;

            Timer = 0;
            Vulnerable = false;
            State = HiveState.Entering;

            Queens = 3;
            Random = new Random();
        }

        public void InitClass()
        {
        }

        public void Update(int Time)
        {
            if (IsDying())
            {
                Position.X += Velocity.X;
                Position.Y += Velocity.Y;
                Angle += Angular;

                Velocity.X *= 0.9F;
                Velocity.Y *= 0.9F;
                Angular *= 0.9;

                if (Velocity.LengthSquared() < 0.0025)
                    Velocity = new Vector2();
                if (-0.05 < Angular && Angular < 0.05)
                    Angular = 0;

                return;
            }

            switch (State)
            {
                case HiveState.Entering:
                    Vector2 heading = new Vector2(500 - Position.X, 250 - Position.Y);

                    if (heading.LengthSquared() <= 4)
                    {
                        State = HiveState.Stopped;
                        Timer = 3000;
                    }
                    else
                    {
                        if (heading.X < 0)
                            Position.X -= 1;
                        else if (heading.X > 0)
                            Position.X += 1;

                        if (heading.Y < 0)
                            Position.Y -= 1;
                        else if (heading.Y > 0)
                            Position.Y += 1;
                    }

                    break;
                case HiveState.Stopped:
                    Vulnerable = false;
                    Timer -= Time;

                    if (Timer < 0)
                    {
                        Released = 0;
                        State = HiveState.Releasing;
                        Vulnerable = getHealthPercentage() > 0.6;

                        if (getHealthPercentage() < 0.1)
                            Timer = 8000;
                        else if (getHealthPercentage() < 0.2)
                            Timer = 10000;
                        else if (getHealthPercentage() < 0.3)
                            Timer = 8000;
                        else if (getHealthPercentage() < 0.5)
                            Timer = 5000;
                        else
                            Timer = 3000;
                    }

                    break;
                case HiveState.Releasing:
                    Timer -= Time;

                    if (Timer < 0)
                    {
                        Vulnerable = false;
                        State = HiveState.Faster;
                        Angular = Random.Next(2) == 0 ? -0.01 : 0.01;
                    }
                    else
                    {
                        int MaxTimer, TotalBugs;

                        if (getHealthPercentage() < 0.1)
                        {
                            MaxTimer = 8000;
                            TotalBugs = 100;
                        }
                        else if (getHealthPercentage() < 0.2)
                        {
                            MaxTimer = 10000;
                            TotalBugs = 75;
                        }
                        else if (getHealthPercentage() < 0.3)
                        {
                            MaxTimer = 8000;
                            TotalBugs = 48;
                        }
                        else if (getHealthPercentage() < 0.5)
                        {
                            MaxTimer = 5000;
                            TotalBugs = 22;
                        }
                        else
                        {
                            MaxTimer = 3000;
                            TotalBugs = 9;
                        }

                        int Target = TotalBugs * (MaxTimer - Timer) / MaxTimer;

                        for (; Released < Target; Released++)
                            ReleaseBug();
                    }

                    break;
                case HiveState.Faster:
                    if (Velocity.LengthSquared() < 0.01)
                    {
                        double angle = Random.NextDouble() * 2 * Math.PI;
                        Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    }

                    if (Position.X < 450 || Position.X > 550 ||
                        Position.Y < 200 || Position.Y > 300)
                    {
                        Velocity.X = 500 - Position.X;
                        Velocity.Y = 250 - Position.Y;
                    }

                    Velocity.Normalize();
                    Velocity.X *= (float)Math.Abs(Angular) * 6;
                    Velocity.Y *= (float)Math.Abs(Angular) * 6;

                    Position.X += Velocity.X;
                    Position.Y += Velocity.Y;
                    Angle += Angular;

                    if (Angular < -0.3 || Angular > 0.3)
                    {
                        Released = 0;
                        State = HiveState.Constant;
                        Vulnerable = true;

                        if (getHealthPercentage() < 0.1)
                            Timer = 2000;
                        else if (getHealthPercentage() < 0.2)
                            Timer = 2500;
                        else if (getHealthPercentage() < 0.3)
                            Timer = 3000;
                        else if (getHealthPercentage() < 0.5)
                            Timer = 3500;
                        else
                            Timer = 4000;
                    }
                    else if (Angular < 0)
                        Angular -= 0.0025;
                    else
                        Angular += 0.0025;

                    break;
                case HiveState.Constant:
                    if (Velocity.LengthSquared() < 0.01)
                    {
                        double angle = Random.NextDouble() * 2 * Math.PI;
                        Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    }

                    if (Position.X < 450 || Position.X > 550 ||
                        Position.Y < 200 || Position.Y > 300)
                    {
                        Velocity.X = 500 - Position.X;
                        Velocity.Y = 250 - Position.Y;
                    }

                    Velocity.Normalize();
                    Velocity.X *= (float)Math.Abs(Angular) * 6;
                    Velocity.Y *= (float)Math.Abs(Angular) * 6;

                    Position.X += Velocity.X;
                    Position.Y += Velocity.Y;
                    Angle += Angular;
                    Timer -= Time;

                    if (Timer < 0)
                    {
                        Vulnerable = false;
                        State = HiveState.Slower;
                    }
                    else
                    {
                        int MaxTimer, TotalBugs;

                        if (getHealthPercentage() < 0.1)
                        {
                            MaxTimer = 2000;
                            TotalBugs = 30;
                        }
                        else if (getHealthPercentage() < 0.2)
                        {
                            MaxTimer = 2500;
                            TotalBugs = 25;
                        }
                        else if (getHealthPercentage() < 0.3)
                        {
                            MaxTimer = 3000;
                            TotalBugs = 25;
                        }
                        else if (getHealthPercentage() < 0.5)
                        {
                            MaxTimer = 3500;
                            TotalBugs = 20;
                        }
                        else
                        {
                            MaxTimer = 4000;
                            TotalBugs = 20;
                        }

                        int Target = TotalBugs * (MaxTimer - Timer) / MaxTimer;

                        for (; Released < Target; Released++)
                            ReleaseBug();
                    }

                    break;
                case HiveState.Slower:
                    if (Velocity.LengthSquared() < 0.01)
                    {
                        double angle = Random.NextDouble() * 2 * Math.PI;
                        Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    }

                    if (Position.X < 450 || Position.X > 550 ||
                        Position.Y < 200 || Position.Y > 300)
                    {
                        Velocity.X = 500 - Position.X;
                        Velocity.Y = 250 - Position.Y;
                    }

                    Velocity.Normalize();
                    Velocity.X *= (float)Math.Abs(Angular) * 6;
                    Velocity.Y *= (float)Math.Abs(Angular) * 6;

                    Position.X += Velocity.X;
                    Position.Y += Velocity.Y;
                    Angle += Angular;

                    if (Angular < -0.02)
                        Angular += 0.005;
                    else if (Angular > 0.02)
                        Angular -= 0.005;
                    else
                    {
                        State = HiveState.Stopped;
                        Timer = 2000;
                        Angular = 0;
                    }

                    break;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsDying())
                Anim.setCurrentFrame(4);
            else if (Health < 40)
                Anim.setCurrentFrame(Vulnerable ? 3 : 2);
            else
                Anim.setCurrentFrame(Vulnerable ? 1 : 0);

            Anim.Draw(batch, Position, Angle);
            Anim.resetColor();
            batch.Draw(Shading, new Vector2(Position.X - 100, Position.Y - 100), Color.White);
        }

        public void TestHit(Attack att)
        {
            if (!Vulnerable)
                return;

            Rectangle eye = new Rectangle((int) (Position.X - 17.5),
                                          (int) (Position.Y - 17.5),
                                          35,
                                          35);

            if (att.area.Intersects(eye))
            {
                Health -= att.damage;

                Anim.setColor(new Color(50, 0, 0, 255));

                if (Health < 1)
                {
                    for (; Queens > 0; Queens--)
                        ReleaseQueen();

                    Health = 0;
                }
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public bool IsDying()
        {
            return Health < 1;
        }

        public int GetScore()
        {
            return 50000;
        }

        public int getScoreOnce()
        {
            if (Scored)
                return 0;

            Scored = true;
            return GetScore();
        }

        public Bug Copy()
        {
            Hive copy = new Hive(Level);

            copy.Anim = Anim.Copy();
            copy.Health = Health;
            copy.Scored = Scored;
            copy.Position = new Vector2(Position.X, Position.Y);
            copy.Angle = Angle;
            copy.Target = new Vector2(Target.X, Target.Y);
            copy.Velocity = new Vector2(Velocity.X, Velocity.Y);
            copy.Angular = Angular;
            copy.Timer = Timer;
            copy.Released = Released;
            copy.Vulnerable = Vulnerable;
            copy.State = State;

            return copy;
        }

        public void SetOrientation(Vector2 loc, double angle)
        {
            Position.X = loc.X;
            Position.Y = loc.Y;
            Angle = angle;
        }

        public double getHealthPercentage()
        {
            return (double)Health / MaxHealth;
        }

        private void ReleaseBug()
        {
            Bug b = BugFactory.FromString(Level.parent, Level, "wasp");
            double angle = Random.Next(6) * Math.PI / 3 - Angle;

            b.SetOrientation(new Vector2((float)(Position.X + 90 * Math.Sin(angle)),
                                         (float)(Position.Y + 90 * Math.Cos(angle))), angle);
            Level.AddBug(b);
        }

        private void ReleaseQueen()
        {
            Bug b = BugFactory.FromString(Level.parent, Level, "queenwasp");
            double angle = Random.NextDouble() * 2 * Math.PI;

            b.SetOrientation(new Vector2((float)(Position.X + 100 * Random.NextDouble() - 50),
                                         (float)(Position.Y + 100 * Random.NextDouble() - 50)), angle);
            Level.AddBug(b);
        }
    }
}

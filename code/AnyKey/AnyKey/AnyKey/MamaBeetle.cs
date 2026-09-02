using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class MamaBeetle : GenericBug, Boss
    {
        private const int MaxHealth = 40;
        private const int Beetles = 10;

        private bool pregnant;

        public MamaBeetle(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = MaxHealth;
            score = 1000;
            speed = 0.5;
            angle = 0;

            animTime = 6;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 90, 120);

            pregnant = true;
        }

        public double getHealthPercentage()
        {
            return (double)health / MaxHealth;
        }

        public override void TestHit(Attack att)
        {
            base.TestHit(att);

            if (pregnant && state == BugState.Squished)
            {
                for (int i = 0; i < Beetles; i++)
                {
                    Bug baby = BugFactory.FromString(level.parent, level, "beetle");
                    double angle = rnd.NextDouble() * 2 * Math.PI;

                    baby.SetOrientation(new Vector2(pos.X + (float)Math.Sin(angle) * 40, pos.Y - (float)Math.Cos(angle) * 40), angle);
                    level.AddBug(baby);
                }

                pregnant = false;
            }
        }

        protected override void Move()
        {
            if (cheeseRef == null && level.cheeses.Count > 0)
            {
                cheeseRef = level.cheeses[rnd.Next(0, level.cheeses.Count)];
            }

            //TODO: Add move logic for the ant

            if (state == BugState.Walking)
            {
                updateError(0.35, Math.PI / 8);

                angle = getAngleToCheese() + error; //remaps the angle so it reflects the animation angle

                #region performmove
                this.pos.X += (float)(Math.Sin(angle) * speed);
                this.pos.Y -= (float)(Math.Cos(angle) * speed);

                #endregion

                collsionCheck();
            }
        }

        protected override void Eat()
        {
            if (cheeseRef.IsAlive() && cheeseRef != null)
            {
                cheeseRef.eat(5);
            }
            else
            {
                if (state != BugState.Squished)
                {
                    state = BugState.Walking;
                }
            }
        }
    }
}

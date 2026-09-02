using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class QueenWasp : GenericBug, Boss
    {
        private const int MaxHealth = 50;

        public QueenWasp(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = MaxHealth;
            score = 1000;
            speed = 1;
            angle = 0;

            animTime = 1;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 70, 90);
        }

        public double getHealthPercentage()
        {
            return (double)health / MaxHealth;
        }

        protected override void Move()
        {
            if (cheeseRef == null && level.cheeses.Count > 0)
            {
                cheeseRef = level.cheeses[rnd.Next(0, level.cheeses.Count)];
            }

            if (state == BugState.Walking)
            {

                updateError(0, Math.PI / 2); //generates a new error term

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
                cheeseRef.eat(1);
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
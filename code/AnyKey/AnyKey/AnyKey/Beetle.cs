using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class Beetle : GenericBug
    {
        public Beetle(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = 10;
            score = 2;
            speed = 0.75;
            angle = 0;

            animTime = 5;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 30, 40);

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

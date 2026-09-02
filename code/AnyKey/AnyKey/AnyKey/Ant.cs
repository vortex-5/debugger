using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class Ant : GenericBug
    {
        public Ant(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = 1;
            score = 1;
            speed = 2;
            angle = 0;

            animTime = 3;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 35, 40);

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

                updateError(0.35, Math.PI / 2); //generates a new error term

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

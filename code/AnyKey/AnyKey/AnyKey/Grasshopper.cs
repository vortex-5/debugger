using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class Grasshopper : GenericBug
    {
        private const int waitTimeTillMove = 1500;
        private int elapsedMoveWaitTime = 0;

        private const int moveDuration = 200;
        private int elapsedMove = 0;


        public Grasshopper(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = 1;
            score = 5;
            speed = 15;
            angle = 0;

            animTime = 10;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 40, 60);

        }

        public override void Update(int Time)
        {
            base.Update(Time);
            Move(Time);
        }

        protected override void Move()
        {
            //empty stub move we won't be using this in grasshopper
        }


        private void Move(int timeElapsed)
        {
            if (cheeseRef == null && level.cheeses.Count > 0)
            {
                cheeseRef = level.cheeses[rnd.Next(0, level.cheeses.Count)];
            }



            //TODO: Add move logic for the ant

            if (state == BugState.Walking)
            {
                
                


                if (waitTimeTillMove - elapsedMoveWaitTime < 700)
                {
                    updateError(0.35, Math.PI / 2.1); //generates a new error term

                    angle = getAngleToCheese() + error; //remaps the angle so it reflects the animation angle
                }




                #region performmove
                if (elapsedMoveWaitTime > waitTimeTillMove)
                {
                    elapsedMoveWaitTime = 0;
                    elapsedMove = 0;
                }
                else
                {
                    if (elapsedMove < moveDuration)
                    {
                        this.pos.X += (float)(Math.Sin(angle) * speed);
                        this.pos.Y -= (float)(Math.Cos(angle) * speed);
                    }

                    elapsedMove += timeElapsed;
                    elapsedMoveWaitTime += timeElapsed;
                }
                #endregion

                collsionCheck();

            }

        }

        protected override void Eat()
        {
            if (cheeseRef.IsAlive() && cheeseRef != null)
            {
                cheeseRef.eat(4);
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

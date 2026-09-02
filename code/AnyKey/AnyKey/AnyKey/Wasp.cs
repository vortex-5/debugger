using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    class Wasp : GenericBug
    {
        private const int waitDuration = 300;
        private int remainingWaitTime = -1;

        private const int randomDuration = 850;
        private int remainingRandomTime = -1;


        private double baseangle = 0;


        public Wasp(Animation moveAnim, Animation squishedAnim, Animation eatAnim, Animation hitAnim, Level level)
        {
            health = 2;
            score = 5;
            speed = 3;
            angle = 0;

            animTime = 0;

            this.moveAnim = moveAnim;
            this.squishedAnim = squishedAnim;
            this.eatAnim = eatAnim;
            this.hitAnim = hitAnim;

            this.level = level;

            clipRectangle = new Rectangle(0, 0, 35, 45);
        }

        public override void Update(int Time)
        {
            base.Update(Time);
            Move(Time);
        }

        protected override void  Move()
        {
            //Standard move not used for wasps time based move is now used.         	  
        }


        private bool initialBite = true;
        private void Move(int Time)
        {
            if (cheeseRef == null && level.cheeses.Count > 0)
            {
                cheeseRef = level.cheeses[rnd.Next(0, level.cheeses.Count)];
            }

            if (state == BugState.Walking)
            {
                initialBite = true;
                updateError(0.25, Math.PI / 2); //generates a new error term

                if (remainingRandomTime < 0)
                {
                    angle = getAngleToCheese() + error; //remaps the angle so it reflects the animation angle
                    baseangle = angle;
                }
                else
                {
                    angle = baseangle + error;
                    remainingRandomTime -= Time;
                }


                if (remainingWaitTime < 0)
                {
                    #region performmove
                    this.pos.X += (float)(Math.Sin(angle) * speed);
                    this.pos.Y -= (float)(Math.Cos(angle) * speed);
                    #endregion
                }
                else
                {
                    remainingWaitTime -= Time;
                }
                

                if (remainingRandomTime < 0)
                    collsionCheck();

            }
            else if (state == BugState.Eating)
            {
                if (initialBite)
                {
                    remainingWaitTime = waitDuration;
                    remainingRandomTime = randomDuration;
                    initialBite = false;
                }
                else
                {
                    if (remainingWaitTime < 0)
                        state = BugState.Walking;
                }

                remainingWaitTime -= Time;
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
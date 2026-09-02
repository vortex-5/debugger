using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public abstract class GenericBug : Bug
    {
        protected static int deathStay = 5000;
        protected static Random rnd = new Random();

        protected int health;
        protected int score; //how many points this bug is worth

        protected Animation moveAnim;
        protected Animation squishedAnim;
        protected Animation eatAnim;
        protected Animation hitAnim;

        protected Level level;

        protected int timeDead;

        protected int lastAnimTime = 0;
        protected int animTime = 1; //per bug animation delay

        public enum BugState { Walking, Eating, Squished, Hit };

        protected Vector2 pos;
        protected double angle; //Radians
        protected double speed;
        protected BugState state;

        protected static Rectangle clipRectangle;

        protected Cheese cheeseRef; //reference to a cheese we've decided to go to.

        protected double error = 0;

        protected bool hasGottenScore = false;

        public virtual void Update(int Time)
        {
            lastAnimTime += Time;
            if (IsTimeForNextFrame())
            {
                UpdateFrame();

                //Reset the color of the bug
                if (!(state == BugState.Squished))
                {
                    moveAnim.setColor(Color.White);
                    eatAnim.setColor(Color.White);
                }
                else if (state == BugState.Squished)
                {
                    byte strength = (byte)(255 * (1 - (float)timeDead / deathStay));
                    squishedAnim.setColor(new Color(255,255,255,strength));
                }
            }

            clipRectangle.X = (int)(pos.X - clipRectangle.Width/2);
            clipRectangle.Y = (int)(pos.Y - clipRectangle.Height/2);

            Move();

            if (state == BugState.Squished)
                timeDead += Time;
        }

        /// <summary>
        /// Tests to see if a bug has been hit
        /// </summary>
        /// <param name="att">an attack used</param>
        public virtual void TestHit(Attack att)
        {
            
            if (state != BugState.Squished)
            {
                if (att.area.Intersects(clipRectangle))
                {
                    health -= att.damage;

                    //Change the color of the bug temporarily
                    moveAnim.setColor(new Color(50, 0, 0, 255));
                    eatAnim.setColor(new Color(50, 0, 0, 255));


                    if (health <= 0)
                    {
                        state = BugState.Squished;
                        //Play a squish sound
                        Sound.PlaySoundClip(soundClip.SQUISH);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the appropreate animation given the current graphics device
        /// </summary>
        /// <param name="batch">sprite batch from the game drawing window</param>
        public virtual void Draw(SpriteBatch batch)
        {
            switch (state)
            {
                case BugState.Eating:
                    eatAnim.Draw(batch, pos, angle);
                    break;
                case BugState.Walking:
                    moveAnim.Draw(batch, pos, angle);
                    break;
                case BugState.Squished:
                    squishedAnim.Draw(batch, pos, angle);
                    break;
                case BugState.Hit:
                    hitAnim.Draw(batch, pos, angle);
                    break;
            }
        }

        /// <summary>
        /// Updates to the next frame of animation (done for efficiency)
        /// </summary>
        private void UpdateFrame()
        {
            switch (state)
            {
                case BugState.Eating:
                    //eatAnim.nextFrame();
                    Eat();
                    break;
                case BugState.Walking:
                    moveAnim.nextFrame();
                    break;
                case BugState.Squished:
                    //squishedAnim.nextFrame();
                    break;
                case BugState.Hit:
                    //hitAnim.nextFrame();
                    break;
            }
        }

        /// <summary>
        /// Finds out if we are ready to render the next frame
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private bool IsTimeForNextFrame()
        {
            if (lastAnimTime > animTime)
            {
                lastAnimTime = 0;
                return true;
            }
            else
            {
                return false;
            }
        }


        public void InitClass()
        {
        }

        /// <summary>
        /// Checks if bug is alive
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAlive()
        {
            return health > 0;
        }

        /// <summary>
        /// Checks if bug is still dead on screen
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDying()
        {
            switch (state)
            {
                case BugState.Squished: //bugs stay in squished state until X number of gametime has elapsed.
                    if ( timeDead < deathStay) return true;
                    else return false;
                default:
                    return false;
            }
        }

        public int GetScore()
        {
            return score;
        }

        public int getScoreOnce()
        {
            if (!hasGottenScore)
            {
                hasGottenScore = true;
                return score;
            }
            else
            {
                return 0;
            }
        }

        public void SetOrientation(Vector2 loc, double angle)
        {
            this.pos = loc;
            this.angle = angle;
        }

        public Bug Copy()
        {
            GenericBug b = (GenericBug)MemberwiseClone();

            b.moveAnim = moveAnim.Copy();
            b.squishedAnim = squishedAnim.Copy();
            b.eatAnim = eatAnim.Copy();
            b.hitAnim = hitAnim.Copy();

            return b;
        }

        /// <summary>
        /// Checks if the target cheese exists
        /// </summary>
        /// <returns></returns>
        protected bool myCheeseExists()
        {
            // cheese exists section
            bool myCheeseExists = false;
            foreach (Cheese chz in level.cheeses)
            {
                if (chz == cheeseRef)
                {
                    myCheeseExists = true;
                }
            }

            return myCheeseExists;
        }

        /// <summary>
        /// creates an error term within clipping ranges this can be used to perturb the bug
        /// </summary>
        /// <param name="factor">multiplier for the pertubation</param>
        /// <param name="clip">clipping will occur if error exceedes this value</param>
        /// <returns></returns>
        protected double updateError(double factor, double clip)
        {
            // Add a bit of randomness to our angle say up to 45 degrees
            if (cheeseRef != null)
            {
                error += (rnd.NextDouble() - 0.5) * factor;
                if (Math.Abs(error) > clip)
                {
                    if (error < 0)
                    {
                        error = -clip;
                    }
                    else
                    {
                        error = clip;
                    }
                }
            }
            else
            {
                error = (rnd.NextDouble() - 0.5) * 0.1;
            }

            return error;
        }

        /// <summary>
        /// checks to see if you collided with an object (currently limited to cheese)
        /// This will also update the cheeseRef if you collide with a cheese different than your target
        /// </summary>
        protected void collsionCheck()
        {
            if (myCheeseExists())
            {

                foreach (Cheese c in level.cheeses)
                {
                    if (c.bounds.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 1, 1)))
                    {
                        cheeseRef = c;
                        state = BugState.Eating;
                    }
                }


            }
            else
            {
                cheeseRef = null;
            }
        }

        /// <summary>
        /// gets the heading to follow in order to get to the cheese
        /// </summary>
        /// <returns></returns>
        protected double getAngleToCheese()
        {
            if (cheeseRef != null)
            {
                double workingangle = Math.Atan2((this.pos.Y - getCheeseCenter().Y), (getCheeseCenter().X - this.pos.X));
                return (Math.PI / 2) - workingangle;
            }
            else
            {
                angle += error;
                return angle;
            }
        }

        /// <summary>
        /// calculates the center of the cheese
        /// </summary>
        /// <returns></returns>
        private Vector2 getCheeseCenter()
        {
            return new Vector2((float)cheeseRef.bounds.X + cheeseRef.bounds.Width / 2.0f, (float)cheeseRef.bounds.Y + cheeseRef.bounds.Height / 2.0f);
        }

        /// <summary>
        /// bugs eat the cheese by calling this (required to impliment)
        /// </summary>
        protected abstract void Eat();

        /// <summary>
        /// bugs custom move AI is coded in here
        /// </summary>
        protected abstract void Move();
    }
}
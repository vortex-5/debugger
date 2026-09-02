using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public class Cheese
    {
        private long initialhealth;
        private long health;
        private Animation cheeseAnim;
        public Rectangle bounds; //position of the cheese

  
        /// <summary>
        /// The cheese for the level
        /// </summary>
        /// <param name="anim">Animation frames for when the cheese is eaten</param>
        /// <param name="position">Scalable bound box where the cheese will be drawn</param>
        /// <param name="health">Initial health for the cheese</param>
        public Cheese(Animation anim, Rectangle position, long health)
        {
            cheeseAnim = anim;
            this.health = health;
            this.bounds = position;
            this.initialhealth = health;
        }

        public void FullRestore()
        {
            if (health > 0)
                health = initialhealth;
        }

        public void Restore(int Amount)
        {
            if (health > 0)
            {
                health += Amount;

                if (health > initialhealth)
                    health = initialhealth;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (health > 0)
                cheeseAnim.Draw(batch, new Vector2(bounds.X + bounds.Width / 2.0f, bounds.Y + bounds.Height / 2.0f) , 0);
        }

        /// <summary>
        /// called by the bugs to eat the cheese
        /// </summary>
        /// <param name="amount">amount of cheese to decrement</param>
        public void eat(long amount)
        {
            this.health -= amount;

            // TODO: create bound shrinking algorithm here
        }

        public void Update()
        {
            if (getHealth() < 0.25)
            {
                cheeseAnim.setCurrentFrame(3);
            }
            else if (getHealth() < 0.5)
            {
                cheeseAnim.setCurrentFrame(2);
            }
            else if (getHealth() < 0.75)
            {
                cheeseAnim.setCurrentFrame(1);
            }
            else
            {
                cheeseAnim.setCurrentFrame(0);
            }
        }

        /// <summary>
        /// Checks if the cheese still exists
        /// </summary>
        /// <returns>true if the cheese is still in play</returns>
        public bool IsAlive()
        {
            return (health > 0);
        }

        public long getHealthRaw()
        {
            return health;
        }

        public float getHealth()
        {
            return (float)health / (float)initialhealth;
        }
    }
}

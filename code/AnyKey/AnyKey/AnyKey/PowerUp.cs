using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public abstract class PowerUp : GenericBug
    {
        private int InitialTimer;
        private int Timer;

        private bool Flash;

        public void init(Animation a, Level l, int life)
        {
            health = 1;
            score = 0;
            speed = 1;
            angle = rnd.NextDouble() * 2 * Math.PI;
            level = l;

            moveAnim = a;
            squishedAnim = a;
            eatAnim = a;
            hitAnim = a;


            InitialTimer = life;
            Timer = life;

            clipRectangle = new Rectangle(0, 0, 60, 60);
            Flash = false;
        }

        public override bool IsAlive()
        {
            return false;
        }

        public override bool IsDying()
        {
            return health > 0 && Timer > 0;
        }

        public override void TestHit(Attack att)
        {
            if (IsDying() && att.area.Intersects(new Rectangle((int)(pos.X - clipRectangle.Width / 2),
                                                               (int)(pos.Y - clipRectangle.Height / 2),
                                                               clipRectangle.Width,
                                                               clipRectangle.Height)))
            {
                Flash = true;
                health--;
                Hit();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!IsDying())
                return;

            const int Beginning = 20;
            const int Fading = 100;
            const int FadePeriod = 20;
            int a = 255;

            if (InitialTimer - Timer < Beginning)
                a = 255 * (InitialTimer - Timer) / Beginning;
            else if (Timer < Fading)
                a = 255 * (Timer % FadePeriod) / FadePeriod;

            moveAnim.setColor(new Color(255, 255, 255, (byte)a));

            if (Flash)
            {
                moveAnim.setColor(new Color(255, 0, 0, 255));
                Flash = false;
            }

            moveAnim.Draw(batch, pos, 0);
        }

        protected override void Move()
        {
            Timer--;

            if (Timer < 0)
                Timer = 0;

            angle += (rnd.NextDouble() - 0.5) * 0.2;
            pos.X += (float)(speed * Math.Cos(angle));
            pos.Y += (float)(speed * Math.Sin(angle));

            if (pos.X < -25 || pos.X > 1025 || pos.Y < -25 || pos.Y > 525)
                health = 0;
        }

        protected override void Eat()
        {
            return;
        }

        protected abstract void Hit();
    }
}

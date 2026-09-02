using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    class NewCheese : PowerUp
    {
        public NewCheese(Level l)
        {
            init(new Animation(l.parent.Content.Load<Texture2D>("newcheese"), 60, 60, 1), l, 150);
            health = 20;
        }

        protected override void Hit()
        {
            if (health < 1)
                level.cheeses.Add(new Cheese(new Animation(level.parent.Content.Load<Texture2D>("cheese"), 64, 64, 4), new Rectangle((int)pos.X - 32, (int)pos.Y - 32, 64, 64), 1000));
        }
    }
}

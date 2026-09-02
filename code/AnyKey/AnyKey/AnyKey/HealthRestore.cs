using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    class HealthRestore : PowerUp
    {
        public HealthRestore(Level l)
        {
            init(new Animation(l.parent.Content.Load<Texture2D>("health"), 60, 60, 1), l, 200);
            health = 10;
        }

        protected override void Hit()
        {
            foreach (Cheese c in level.cheeses)
                c.Restore(100);
        }
    }
}

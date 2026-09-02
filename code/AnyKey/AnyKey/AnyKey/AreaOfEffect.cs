using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    class AreaOfEffect : PowerUp
    {
        public AreaOfEffect(Level l)
        {
            init(new Animation(l.parent.Content.Load<Texture2D>("bigger"),60,60,1), l, 200);
            health = 10;
        }

        protected override void Hit()
        {
            if (health < 1)
            {
                level.attacks.ModeChange(AttackMapper.AttackMode.Bigger);
            }
        }
    }
}

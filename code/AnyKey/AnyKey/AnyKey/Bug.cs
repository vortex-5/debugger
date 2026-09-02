using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    public interface Bug
    {
        void InitClass();

        void Update(int Time);

        void Draw(SpriteBatch batch);

        void TestHit(Attack att);

        bool IsAlive();

        bool IsDying();

        int GetScore();

        int getScoreOnce();

        Bug Copy();

        void SetOrientation(Vector2 loc, double angle);
    }
}

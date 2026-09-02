using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnyKey
{
    public class AttackMapper
    {
        private int elapsedTime = 0;
        private const int FOGTime = 5000;
        private const int BiggerTime = 5000;

        public enum AttackMode {
            Normal,
            Bigger,
            FistOfGod
        };

        public byte Alpha;

        private Dictionary<Keys, Rectangle> mapping;
        private AttackMode mode;
        private Texture2D up, down;
        private Texture2D keyboard;
        private Texture2D square;

        private Keys[] previous;

        public AttackMapper(ContentManager manager, int w, int h)
        {
            Alpha = 128;
            mapping = new Dictionary<Keys, Rectangle>();

            Keys[] numberRow = { Keys.D1, Keys.D2, Keys.D3,
                                 Keys.D4, Keys.D5, Keys.D6,
                                 Keys.D7, Keys.D8, Keys.D9,
                                 Keys.D0, Keys.OemMinus, Keys.OemPlus };
            Keys[] qwertyRow = { Keys.Q, Keys.W, Keys.E,
                                 Keys.R, Keys.T, Keys.Y,
                                 Keys.U, Keys.I, Keys.O,
                                 Keys.P, Keys.OemOpenBrackets, Keys.OemCloseBrackets };
            Keys[] asdfRow = { Keys.A, Keys.S, Keys.D,
                               Keys.F, Keys.G, Keys.H,
                               Keys.J, Keys.K, Keys.L,
                               Keys.OemSemicolon, Keys.OemQuotes };
            Keys[] zxcvRow = { Keys.Z, Keys.X, Keys.C,
                               Keys.V, Keys.B, Keys.N,
                               Keys.M, Keys.OemComma,
                               Keys.OemPeriod, Keys.OemQuestion };

            double wd = w / (numberRow.Length + 0.5);

            for (int k = 0; k < numberRow.Length; k++)
                mapping[numberRow[k]] = new Rectangle((int) (k * wd),
                                                      0,
                                                      (int) wd,
                                                      h / 4);

            for (int k = 0; k < qwertyRow.Length; k++)
                mapping[qwertyRow[k]] = new Rectangle((int) ((k + 0.5) * wd),
                                                      h / 4,
                                                      (int) wd,
                                                      h / 4);

            for (int k = 0; k < asdfRow.Length; k++)
                mapping[asdfRow[k]] = new Rectangle((int)((k + 0.75) * wd),
                                                    h / 2,
                                                    (int)wd,
                                                    h / 4);

            for (int k = 0; k < zxcvRow.Length; k++)
                mapping[zxcvRow[k]] = new Rectangle((int)((k + 1.25) * wd),
                                                    h * 3 / 4,
                                                    (int)wd,
                                                    h / 4);

            mode = AttackMode.Normal;
            up = manager.Load<Texture2D>("up");
            down = manager.Load<Texture2D>("down");
            keyboard = manager.Load<Texture2D>("keyboard");
            square = manager.Load<Texture2D>("square");
            previous = Keyboard.GetState().GetPressedKeys();
        }

        public List<Attack> GetAllAttacks()
        {
            List<Attack> atts = new List<Attack>();
            Attack att;

            foreach (Keys k in Input.KeysHit)
            {
                if (!mapping.ContainsKey(k))
                    continue;

                att = null;

                switch (mode)
                {
                    case AttackMode.Normal:
                        att = new Attack();
                        att.area = new Rectangle(mapping[k].X,
                                                 mapping[k].Y,
                                                 mapping[k].Width,
                                                 mapping[k].Height);
                        att.damage = 1;
                        atts.Add(att);
                        break;
                    case AttackMode.Bigger:
                        att = new Attack();
                        att.area = new Rectangle(mapping[k].X - mapping[k].Width,
                                                 mapping[k].Y - mapping[k].Height,
                                                 mapping[k].Width * 3,
                                                 mapping[k].Height * 3);
                        att.damage = 1;
                        atts.Add(att);
                        break;
                    case AttackMode.FistOfGod:
                        att = new Attack();
                        att.area = new Rectangle(mapping[k].X,
                                                 mapping[k].Y,
                                                 mapping[k].Width,
                                                 mapping[k].Height);
                        att.damage = 10;
                        atts.Add(att);
                        break;
                }

            }

            previous = Keyboard.GetState().GetPressedKeys();

            return atts;
        }

        public void Disable(SpawnArea area)
        {
            List<Keys> disabled = new List<Keys>();

            foreach (Keys k in mapping.Keys)
            {
                if (mapping[k].Intersects(area.Rect))
                    disabled.Add(k);
            }

            for (int i = 0; i < disabled.Count; i++)
                mapping.Remove(disabled[i]);
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Keys k in mapping.Keys)
            {
                Rectangle dst = mapping[k];
                Rectangle src = new Rectangle(dst.X, dst.Y, dst.Width, dst.Height);

                if (Keyboard.GetState().IsKeyDown(k))
                    src.Y += keyboard.Height / 2;

                switch (mode)
                {
                    case AttackMode.FistOfGod:
                        batch.Draw(keyboard, dst, src, new Color(255, 0, 0, 255));
                        break;
                    case AttackMode.Bigger:
                        batch.Draw(keyboard, dst, src, new Color(100, 100, 255, 255));

                        if (Keyboard.GetState().IsKeyDown(k))
                            batch.Draw(down, new Rectangle(dst.X - dst.Width,
                                                             dst.Y - dst.Height,
                                                             dst.Width * 3,
                                                             dst.Height * 3),
                                       new Color(100, 100, 255, 255));

                        break;
                    default:
                        batch.Draw(keyboard, dst, src, new Color(255, 255, 255, Alpha));
                        break;
                }
                
            }
        }

        public void ModeChange(AttackMode newMode)
        {
            switch (newMode)
            {
                case AttackMode.FistOfGod:
                    mode = newMode;
                    elapsedTime = 0;
                    break;
                case AttackMode.Bigger:
                    mode = newMode;
                    elapsedTime = 0;
                    break;
                default:
                    mode = newMode;
                    break;
            }

        }

        public void Update(int Time)
        {
            switch (mode)
            {
                case AttackMode.FistOfGod:
                    elapsedTime += Time;
                    if (elapsedTime > FOGTime)
                        mode = AttackMode.Normal;
                    break;
                case AttackMode.Bigger:
                    elapsedTime += Time;
                    if (elapsedTime > BiggerTime)
                        mode = AttackMode.Normal;
                    break;
            }
        }
    }
}

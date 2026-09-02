using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AnyKey
{
    public class BugFactory
    {
        public static Bug FromString(Game1 g, Level l, string type)
        {
            if (type.ToLower().Equals("ant"))
                return MakeAnt(g, l);
            else if (type.ToLower().Equals("beetle"))
                return MakeBeetle(g, l);
            else if (type.ToLower().Equals("ladybug"))
                return MakeLadybug(g, l);
            else if (type.ToLower().Equals("grasshopper"))
                return MakeGrasshopper(g, l);
            else if (type.ToLower().Equals("wasp"))
                return MakeWasp(g, l);
            else if (type.ToLower().Equals("roach"))
                return MakeRoach(g, l);
            else if (type.ToLower().Equals("mamabeetle"))
                return MakeMamaBeetle(g, l);
            else if (type.ToLower().Equals("queenwasp"))
                return MakeQueenWasp(g, l);
            else if (type.ToLower().Equals("mamaroach"))
                return MakeMamaRoach(g, l);
            else if (type.ToLower().Equals("hive"))
                return MakeHive(l);
            else if (type.ToLower().Equals("hammerofgod"))
                return MakeHammerOfGod(l);
            else if (type.ToLower().Equals("areaofeffect"))
                return MakeAreaOfEffect(l);
            else if (type.ToLower().Equals("healthrestore"))
                return MakeHealthRestore(l);
            else if (type.ToLower().Equals("newcheese"))
                return MakeNewCheese(l);
            else return null;
        }

        private static Ant MakeAnt(Game1 g, Level l)
        {
            return new Ant(new Animation(g.Content.Load<Texture2D>("ant"), 35, 40, 8),
                           new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                           new Animation(g.Content.Load<Texture2D>("ant"), 35, 40, 1),
                           new Animation(g.Content.Load<Texture2D>("ant"), 35, 40, 1), l);
        }

        private static Beetle MakeBeetle(Game1 g, Level l)
        {
            return new Beetle(new Animation(g.Content.Load<Texture2D>("purple"), 30, 40, 8),
                              new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                              new Animation(g.Content.Load<Texture2D>("purple"), 30, 40, 1),
                              new Animation(g.Content.Load<Texture2D>("purple"), 30, 40, 1), l);
        }

        private static Ladybug MakeLadybug(Game1 g, Level l)
        {
            return new Ladybug(new Animation(g.Content.Load<Texture2D>("ladybug"), 40, 40, 8),
                               new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                               new Animation(g.Content.Load<Texture2D>("ladybug"), 40, 40, 1),
                               new Animation(g.Content.Load<Texture2D>("ladybug"), 40, 40, 1), l);
        }

        private static Grasshopper MakeGrasshopper(Game1 g, Level l)
        {
            return new Grasshopper(new Animation(g.Content.Load<Texture2D>("grasshopper"), 40, 60, 8),
                                   new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                                   new Animation(g.Content.Load<Texture2D>("grasshopper"), 40, 60, 1),
                                   new Animation(g.Content.Load<Texture2D>("grasshopper"), 40, 60, 1), l);
        }

        private static Wasp MakeWasp(Game1 g, Level l)
        {
            return new Wasp(new Animation(g.Content.Load<Texture2D>("flyingwasp"), 40, 50, 2),
                            new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                            new Animation(g.Content.Load<Texture2D>("wasp"), 40, 50, 1),
                            new Animation(g.Content.Load<Texture2D>("wasp"), 40, 50, 1), l);
        }

        private static Roach MakeRoach(Game1 g, Level l)
        {
            return new Roach(new Animation(g.Content.Load<Texture2D>("roach"), 40, 60, 8),
                             new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                             new Animation(g.Content.Load<Texture2D>("roach"), 40, 60, 1),
                             new Animation(g.Content.Load<Texture2D>("roach"), 40, 60, 1), l);
        }

        private static MamaBeetle MakeMamaBeetle(Game1 g, Level l)
        {
            return new MamaBeetle(new Animation(g.Content.Load<Texture2D>("mamapurple"), 120, 160, 16),
                                  new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                                  new Animation(g.Content.Load<Texture2D>("mamapurple"), 120, 160, 1),
                                  new Animation(g.Content.Load<Texture2D>("mamapurple"), 120, 160, 1), l);
        }

        private static QueenWasp MakeQueenWasp(Game1 g, Level l)
        {
            return new QueenWasp(new Animation(g.Content.Load<Texture2D>("flyingqueen"), 80, 90, 2),
                                 new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                                 new Animation(g.Content.Load<Texture2D>("queen"), 80, 90, 1),
                                 new Animation(g.Content.Load<Texture2D>("queen"), 80, 90, 1), l);
        }

        private static MamaRoach MakeMamaRoach(Game1 g, Level l)
        {
            return new MamaRoach(new Animation(g.Content.Load<Texture2D>("mamaroach"), 100, 150, 8),
                                 new Animation(g.Content.Load<Texture2D>("squish"), 50, 50, 1),
                                 new Animation(g.Content.Load<Texture2D>("mamaroach"), 100, 150, 1),
                                 new Animation(g.Content.Load<Texture2D>("mamaroach"), 100, 150, 1), l);
        }

        private static Hive MakeHive(Level l)
        {
            return new Hive(l);
        }

        private static HammerOfGod MakeHammerOfGod(Level l)
        {
            return new HammerOfGod(l);
        }

        private static AreaOfEffect MakeAreaOfEffect(Level l)
        {
            return new AreaOfEffect(l);
        }

        private static HealthRestore MakeHealthRestore(Level l)
        {
            return new HealthRestore(l);
        }

        private static NewCheese MakeNewCheese(Level l)
        {
            return new NewCheese(l);
        }
    }
}

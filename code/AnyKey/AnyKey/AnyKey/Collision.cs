using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AnyKey
{
    public class Collision
    {
        //Collision code taken from: http://creators.xna.com/

        private Game1 parent;
        private Boolean collided;

        public Collision(Game1 g)
        {
            parent = g;
        }


        public struct collision_struct
        {
            public collision_type collidestatus;
            public Cheese cheeseref;

            public collision_struct(collision_type col_in)
            {
                collidestatus = col_in;
                cheeseref = null;
            }

            public collision_struct(collision_type col_in, Cheese cheese_in)
            {
                collidestatus = col_in;
                cheeseref = cheese_in;
            }
        }


        //Enumeration type for return
        public enum collision_type
        {
            Off_Screen,
            Cheese,
            None
        }

        public collision_struct collision_check(Rectangle bug_rect, Cheese c)
        {
            //Check for bug off screen
            if ((bug_rect.X > parent.Window.ClientBounds.X)||(bug_rect.X < 0) || (bug_rect.Y > parent.Window.ClientBounds.Y) || (bug_rect.Y < 0))
            {
                return new collision_struct(collision_type.Off_Screen);
            }

            
            //foreach (Cheese c in parent.Level.cheeses)
            //foreach (Cheese c in cheeses)
            {
                if (bug_rect.Intersects(c.bounds))
                {
                    return new collision_struct(collision_type.Cheese, c);
                    //collided = true;

                    
                }
            }
            return new collision_struct(collision_type.None);
        }

    }
}

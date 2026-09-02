using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnyKey
{
    public static class Input
    {
        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;
        public static List<Keys> KeysHit;
        private static List<Keys> KeysDown;

        public static MouseState LastMouseState;
        public static MouseState MouseState;

        public static void Initialize()
        {
            LastKeyboardState = Keyboard.GetState();
            KeyboardState = Keyboard.GetState();
            KeysDown = new List<Keys>();

            LastMouseState = Mouse.GetState();
            MouseState = Mouse.GetState();
        }

        public static void Update()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            DetermineHitKeys();

            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
        }

        private static void DetermineHitKeys()
        {
            KeysHit = new List<Keys>();

            Keys[] pressedKeys = KeyboardState.GetPressedKeys();

            // for each pressed key, determine if it was just hit
            for(int i=0; i < pressedKeys.Length; i++)
            {
                bool keyIsDown = false;
                foreach (Keys keyDown in KeysDown)
                {
                    if (keyDown == pressedKeys[i])
                        keyIsDown = true;
                }

                if (!keyIsDown)
                {
                    KeysHit.Add(pressedKeys[i]);
                    KeysDown.Add(pressedKeys[i]);
                }
            }

            // clear any keys that are lifted from keysdown
            for (int i = 0; i < KeysDown.Count; i++)
            {
                if (KeyboardState.IsKeyUp(KeysDown[i]))
                {
                    KeysDown.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

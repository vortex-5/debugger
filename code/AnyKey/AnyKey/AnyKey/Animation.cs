using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnyKey
{
    public class Animation
    {
        //The texture for the animation
        protected Texture2D texture;
        protected int currentFrame;
        protected Color color;
        protected int frameCount;
        protected int frameWidth;
        protected int frameHeight;
        
        
        

        
        /// <summary>
        /// Constructor to assign values to all fields
        /// </summary>
        /// <param name="aTexture"></param>
        /// <param name="fWidth"></param>
        /// <param name="fHeight"></param>
        /// <param name="numFrames"></param>
        public Animation(Texture2D aTexture, int fWidth, int fHeight, int numFrames)
        {
            texture = aTexture;
            frameWidth = fWidth;
            frameHeight = fHeight;
            frameCount = numFrames;
            currentFrame = 0;
            color = Color.White;
        }

        //Move to the next frame of animation
        //Animation loops by default
        public void nextFrame()
        {
            currentFrame++;
            if (currentFrame >= frameCount)
            {
                currentFrame = 0;
            }
        }

        /// <summary>
        /// Draw a frame of the animation to the screen at the given location.
        /// dest specifies the rectangle to be filled by the frame
        /// Direction value orients the frame, and is given in radians
        /// </summary>
        public void Draw(SpriteBatch sBatch, Rectangle dest, double direction)
        {
            sBatch.Draw(texture, dest,
                new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight), color,
                (float)direction, new Vector2(frameWidth / 2, frameHeight / 2), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a frame of the animation to the screen at the given location.
        /// Location specifies the CENTER of the frame
        /// Direction value orients the frame, and is given in radians
        /// </summary>
        public void Draw(SpriteBatch sBatch, Vector2 location, double direction)
        {
            sBatch.Draw(texture, new Rectangle((int) (location.X), (int) (location.Y), frameWidth, frameHeight),
                new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight), color,
                (float)direction, new Vector2(frameWidth / 2, frameHeight / 2), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Changes the color of the animation permanently
        /// </summary>
        /// <param name="newColor"></param>
        public void setColor(Color newColor)
        {
            color = newColor;
        }

        /// <summary>
        /// Reset the color of the animation to what is was originally
        /// </summary>
        public void resetColor()
        {
            color = Color.White;
        }
        /// <summary>
        /// Sets the current frame of the animation
        /// Frame numbers start at 0 and go to frameCount - 1
        /// </summary>
        /// <param name="newFrame"></param>
        public void setCurrentFrame(int newFrame)
        {
            if (newFrame > frameCount - 1)
            {
                currentFrame = frameCount - 1;
            }
            else if (newFrame < 0)
            {
                currentFrame = 0;
            }
            else{
                currentFrame = newFrame;
            }
        }

        public Animation Copy()
        {
            return (Animation)MemberwiseClone();
        }
    }
}

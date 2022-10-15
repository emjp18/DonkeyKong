using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DonkeyKong
{
    internal class UserControlledSprite : Sprite
    {
        private MouseState m_prevMouseState;
        private Vector2 m_destination;
        private bool m_moving = false;
        
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
                Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
                Vector2 speed, int millisecondsPerFrame)
                : base(textureImage, position, frameSize, collisionOffset, currentFrame,
                sheetSize, speed, millisecondsPerFrame)
        {
        }
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X = 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y = 1;
                
                return inputDirection;
            }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (!m_moving)
            {
                Vector2 dir = direction;
                if(dir!=Vector2.Zero)
                {
                    ChangeDirection(dir);
                }
                else
                {
                    m_position += (direction * m_speed *
                (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (Vector2.Distance(m_position, m_destination) < 1)
                    {
                        m_position = m_destination;
                        m_moving = false;
                    }

                }
                
            }
              
            
            // If player moved the mouse, move the sprite
            MouseState currMouseState = Mouse.GetState();
            if (currMouseState.X != m_prevMouseState.X ||
            currMouseState.Y != m_prevMouseState.Y)
            {
                //m_position = new Vector2(currMouseState.X, currMouseState.Y);
            }
            m_prevMouseState = currMouseState;
            // If sprite is off the screen, move it back within the game window
            if (m_position.X < 0)
                m_position.X = 0;
            if (m_position.Y < 0)
                m_position.Y = 0;
            if (m_position.X > clientBounds.Width - m_frameSize.X)
                m_position.X = clientBounds.Width - m_frameSize.X;
            if (m_position.Y > clientBounds.Height - m_frameSize.Y)
                m_position.Y = clientBounds.Height - m_frameSize.Y;
            base.Update(gameTime, clientBounds);
        }
        public void ChangeDirection(Vector2 dir)
        {
            
            Vector2 newDestination = m_position + dir * SpriteManager.g_tilesize;
            
            if (SpriteManager.GetTileLadderAtPosition(newDestination))
            {
                if(dir.Y>0||dir.X!=0)
                {
                    m_destination = newDestination;
                    m_moving = true;
                }
            }
            else
            {
                if (dir.Y==0)
                {
                    m_destination = newDestination;
                    m_moving = true;
                }
            }
        }
    }
}

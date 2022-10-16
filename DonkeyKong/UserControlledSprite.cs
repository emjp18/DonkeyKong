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
        
        private Vector2 m_destination;
        private bool m_moving = false;
        private Vector2 m_dir;
        private void ClampWindow(Rectangle clientBounds, ref Vector2 position)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - m_frameSize.X)
                position.X = clientBounds.Width - m_frameSize.X;
            if (position.Y > clientBounds.Height - m_frameSize.Y)
                position.Y = clientBounds.Height - m_frameSize.Y;
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
                Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
                float speed, int millisecondsPerFrame)
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
                {
                    inputDirection.X = -1;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X = 1;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    inputDirection.Y = -1;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    inputDirection.Y = 1;

                }


                return inputDirection;
            }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
            
            if (!m_moving)
            {
                m_dir = direction;
                ChangeDirection(m_dir, clientBounds);
            }
            else
            {
                m_position += (m_dir * m_speed *
            (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (Vector2.Distance(m_position, m_destination) < 1)
                {
                    m_position = m_destination;
                    m_moving = false;
                }
            }

           
            
            base.Update(gameTime, clientBounds);
        }
        public void ChangeDirection(Vector2 dir, Rectangle clientBounds)
        {
            
            Vector2 newDestination = m_position + dir * SpriteManager.g_tilesize;

            ClampWindow(clientBounds, ref newDestination);
            if (SpriteManager.GetTileLadderAtPosition(newDestination)==SpriteManager.TILE_TYPE.LADDER)
            {
                if((dir.Y<0 && dir.X == 0)||(dir.X<0&&dir.Y==0) || (dir.X > 0 && dir.Y == 0)|| (dir.Y > 0 && dir.X == 0))
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

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
        private bool m_climbingLadder = false;
        private Vector2 m_destination;
        private bool m_moving = false;
        private Vector2 m_dir;
        private Texture2D m_marioBackTex;
        private Texture2D m_marioFrontTex;
        public int g_lives = 5;
        private bool m_knocked = false;
        public UserControlledSprite(Texture2D textureImage, Texture2D marioBack, Vector2 position,
                Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
                float speed, int millisecondsPerFrame)
                : base(textureImage, position, frameSize, collisionOffset, currentFrame,
                sheetSize, speed, millisecondsPerFrame)
        {
            m_marioBackTex = marioBack;
            m_marioFrontTex = textureImage;
        }
        public override void SetPosition(Vector2 pos)
        {
            m_destination = pos;
            m_moving = false;
            base.SetPosition(pos);
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
        public bool KnockBack(Vector2 direction, Rectangle clientBounds)
        {
            m_knocked = true;
            Vector2 newDestination = m_position + direction * SpriteManager.g_tilesizeY;
            if(!ClampWindow(clientBounds, ref newDestination)&& !m_climbingLadder)
            {
                m_destination = newDestination;
                m_moving = true;
                m_dir = direction;
                
                return true;
            }
            
            return false;

        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            
            if (!m_moving&&!m_knocked)
            {
                m_dir = direction;
                ChangeDirection(m_dir, clientBounds);
            }
            else
            {
                m_position += (m_dir * m_speed *
            (float)gameTime.ElapsedGameTime.TotalSeconds);
                ClampWindow(clientBounds, ref m_position);
                if (Vector2.Distance(m_position, m_destination) < 1)
                {
                    m_position = m_destination;
                    m_moving = false;
                    if (m_knocked)
                        g_lives--;
                    m_knocked = false;
                }
            }
           
            
            base.Update(gameTime, clientBounds);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(m_climbingLadder)
            {
                m_textureImage = m_marioBackTex;
            }
            else
            {
                m_textureImage = m_marioFrontTex;
            }
            base.Draw(gameTime, spriteBatch);
        }
        public void ChangeDirection(Vector2 dir, Rectangle clientBounds)
        {




            if (m_climbingLadder)
            {
                Vector2 newDestinationY = m_position + dir * SpriteManager.g_tilesizeY;
                ClampWindow(clientBounds, ref newDestinationY);
                SpriteManager.TILE_TYPE typeY = SpriteManager.GetTileTypeAtPosition(newDestinationY);

                if (typeY == SpriteManager.TILE_TYPE.LADDER)
                {
                    if ((dir.X != 0 && dir.Y == 0) || (dir.Y != 0 && dir.X == 0))
                    {
                        m_destination = newDestinationY;
                        m_moving = true;
                        m_climbingLadder = true;
                    }

                }
                else if (typeY == SpriteManager.TILE_TYPE.WALL)
                {
                    if (dir.Y == 0)
                    {
                        m_destination = newDestinationY;
                        m_moving = true;
                        m_climbingLadder = false;
                    }



                }
                

            }
            else
            {
                Vector2 newDestination = m_position + dir * SpriteManager.g_tilesize;
                ClampWindow(clientBounds, ref newDestination);
                SpriteManager.TILE_TYPE type = SpriteManager.GetTileTypeAtPosition(newDestination);
                if (type == SpriteManager.TILE_TYPE.LADDER)
                {
                    if ((dir.X != 0 && dir.Y == 0) || (dir.Y != 0 && dir.X == 0))
                    {
                        m_destination = newDestination;
                        m_moving = true;
                        m_climbingLadder = true;
                    }

                }
                else if (type == SpriteManager.TILE_TYPE.WALL)
                {
                    if (dir.Y == 0)
                    {
                        m_destination = newDestination;
                        m_moving = true;
                        m_climbingLadder = false;
                    }

                }
            }
        }
    }
}

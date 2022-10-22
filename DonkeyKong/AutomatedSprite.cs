using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DonkeyKong
{
    internal class AutomatedSprite : Sprite
    {
        private bool m_ismoving = false;
        public enum DK_ANI_STATE { LEFT,RIGHT,FALL, UP, DOWN };
        private DK_ANI_STATE m_dkAniState;
        Vector2 m_direction = Vector2.Zero;
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, float speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            
        }
        public override Vector2 direction
        {
            get { return m_direction; }
        }
        public void SetDirection(Vector2 direction)
        {
            m_direction = direction;
        }
        public void RandomizeSpeed(int min=30, int max = 90) { Random random = new Random(); m_speed = (float)random.Next(min, max); }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            m_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (m_timeSinceLastFrame > m_millisecondsPerFrame)
            {
                m_timeSinceLastFrame = 0;
                ++m_currentFrame.X;
                if (m_currentFrame.X >= m_sheetSize.X)
                {
                    m_currentFrame.X = 0;
                    
                }
            }
            
        }
        public void SetMass(float mass)
        {
            m_mass = mass;
        }
        public void UpdateTile(GameTime gameTime, Rectangle clientBounds)
        {
            m_velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds * m_speed;
            m_position += m_velocity;

            if (m_position.Y > SpriteManager.g_tilesizeY * 17)
            {
                g_update = false;
                g_draw = false;
            }
        }
        public void UpdateEnemyFire(GameTime gameTime, Rectangle clientBounds, Sprite DK)
        {
            if (m_velocity != Vector2.Zero)
            {
                m_velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds * m_speed;
                m_position += m_velocity;
            }
            else
            {
                m_direction.Y = 0;
                bool collide = Collide(DK);
                if (ClampWindow(clientBounds, ref m_position) || collide)
                {
                    m_direction.X *= -1;
                    if (collide)
                    {
                        m_position += m_direction * DK.GetTex().Width * 0.25f;
                        if (ClampWindow(clientBounds, ref m_position) || m_direction.Equals(DK.direction))
                        {
                            m_direction.X *= -1;
                            m_position += m_direction * DK.GetTex().Width * 1.25f;
                            if (GetSpriteEffect() == SpriteEffects.None)
                            {
                                SetSpriteEffect(SpriteEffects.FlipHorizontally);
                            }
                            else
                            {
                                SetSpriteEffect(SpriteEffects.None);
                            }
                        }
                    }
                    if (GetSpriteEffect() == SpriteEffects.None)
                    {
                        SetSpriteEffect(SpriteEffects.FlipHorizontally);
                    }
                    else
                    {
                        SetSpriteEffect(SpriteEffects.None);
                    }
                }
                m_position += m_direction * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //base.Update(gameTime, clientBounds);
            }
            if (m_position.Y > SpriteManager.g_tilesizeY * 17)
            {
                g_update = false;
                g_draw = false;
            }
        }
        public void UpdateDK(GameTime gameTime, Rectangle clientBounds)
        {
            
            
            if (m_velocity != Vector2.Zero)
            {
                m_velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds * m_speed;
                m_position += m_velocity;
                m_ismoving = false;
                m_dkAniState = DK_ANI_STATE.FALL;
            }
            else
            {
                if(m_direction.X!=0)
                {
                    m_ismoving = true;
                }
                else
                {
                    m_ismoving = false;
                }
                m_direction.Y = 0;
                m_position += m_direction * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ClampWindow(clientBounds, ref m_position))
                {
                    m_direction.X *= -1;

                }
            }
            if (m_position.Y > SpriteManager.g_tilesizeY * 17)
            {
                g_update = false;
                g_draw = false;
            }

            m_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            
            switch (m_dkAniState)
            {
                case DK_ANI_STATE.LEFT:
                    {
                        if(m_direction.X>0)
                        {
                            m_dkAniState = DK_ANI_STATE.RIGHT;
                            m_currentFrame.X = 1;
                            m_currentFrame.Y = 2;
                        }
                        else if(m_direction.X==0)
                        {
                            m_dkAniState = DK_ANI_STATE.DOWN;
                            m_currentFrame.X = 3;
                            m_currentFrame.Y = 0;
                        }
                        
                            
                        break;
                    }
                case DK_ANI_STATE.RIGHT:
                    {
                        if (m_direction.X < 0)
                        {
                            m_dkAniState = DK_ANI_STATE.LEFT;
                            m_currentFrame.X = 1;
                            m_currentFrame.Y = 2;
                        }
                        else if (m_direction.X == 0)
                        {
                            m_dkAniState = DK_ANI_STATE.DOWN;
                            m_currentFrame.X = 3;
                            m_currentFrame.Y = 0;
                        }
                        break;
                    }
                case DK_ANI_STATE.FALL:
                    {
                        m_effect = SpriteEffects.FlipVertically;
                        m_dkAniState = DK_ANI_STATE.DOWN;
                        m_currentFrame.X = 3;
                        m_currentFrame.Y = 0;
                        break;
                    }
                case DK_ANI_STATE.UP:
                    {
                        if (m_timeSinceLastFrame > m_millisecondsPerFrame)
                        {
                            m_timeSinceLastFrame = 0;
                            if (m_ismoving)
                            {
                                if(m_direction.X>0)
                                {
                                    m_dkAniState = DK_ANI_STATE.RIGHT;
                                    m_currentFrame.X = 1;
                                    m_currentFrame.Y = 2;
                                }
                                else
                                {
                                    m_dkAniState = DK_ANI_STATE.LEFT;
                                    m_currentFrame.X = 0;
                                    m_currentFrame.Y = 2;
                                }
                                
                            }
                            else
                            {
                                m_dkAniState = DK_ANI_STATE.DOWN;
                                m_currentFrame.X = 3;
                                m_currentFrame.Y = 0;
                            }


                        }
                        break;
                    }
                case DK_ANI_STATE.DOWN:
                    {
                        if (m_timeSinceLastFrame > m_millisecondsPerFrame)
                        {
                            m_timeSinceLastFrame = 0;
                            if (m_ismoving)
                            {
                                if (m_direction.X > 0)
                                {
                                    m_dkAniState = DK_ANI_STATE.RIGHT;
                                    m_currentFrame.X = 1;
                                    m_currentFrame.Y = 2;
                                }
                                else
                                {
                                    m_dkAniState = DK_ANI_STATE.LEFT;
                                    m_currentFrame.X = 0;
                                    m_currentFrame.Y = 2;
                                }

                            }
                            else
                            {
                                m_dkAniState = DK_ANI_STATE.UP;
                                m_currentFrame.X = 2;
                                m_currentFrame.Y = 0;
                            }


                        }
                        break;
                    }
            }
            
            

        }
        public void UpdatePeach(GameTime gameTime, Rectangle clientBounds, Sprite player, SpriteManager.AVATAR ava)
        {
            
            m_direction = player.GetPos() - m_position;
            
            m_direction.Normalize();
            m_direction.Y = 0;

            if (ava == SpriteManager.AVATAR.PAULINE)
            {
                m_currentFrame.Y = 1;
            }
            else
            {
                m_currentFrame.Y = 0;
            }
            if(player.GetCurrentFrame().X==0)
            {
                m_currentFrame.X = 0;
                m_direction = Vector2.Zero;
            }
            else
            {
                m_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (m_timeSinceLastFrame > m_millisecondsPerFrame)
                {
                    m_timeSinceLastFrame = 0;
                    ++m_currentFrame.X;
                    if (m_currentFrame.X >= 9)
                    {
                        m_currentFrame.X = 0;

                    }
                }
            }
            if(m_direction.X>0)
            {
                m_effect = SpriteEffects.None;
            }
            else if (m_direction.X < 0)
            {
                m_effect = SpriteEffects.FlipHorizontally;
            }

            m_position += m_direction * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ClampWindow(clientBounds, ref m_position);


        }
    }
}

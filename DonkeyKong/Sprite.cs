using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DonkeyKong
{
    internal abstract class Sprite
    {
        protected Vector2 m_velocity = Vector2.Zero;
        protected float m_r;
        protected float m_mass = 10.0f;
        public bool g_draw = true;
        public bool g_update = true;
        protected Texture2D m_textureImage;
        protected Vector2 m_position;
        protected Point m_frameSize;
        protected Point m_currentFrame;
        protected Point m_sheetSize;
        protected int m_timeSinceLastFrame;
        protected int m_millisecondsPerFrame;
        protected float m_speed;
        protected int m_collisionOffset = 0;
        protected SpriteEffects m_effect = SpriteEffects.None;
        public void SetSpriteEffect(SpriteEffects effect)
        {
            m_effect = effect;
        }
        public SpriteEffects GetSpriteEffect() { return m_effect; }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, float speed,
                    int millisecondsPerFrame)
        {
            m_textureImage = textureImage;
            m_position = position;
            m_frameSize = frameSize;
            m_collisionOffset = collisionOffset;
            m_currentFrame = currentFrame;
            m_sheetSize = sheetSize;
            m_speed = speed;
            m_millisecondsPerFrame = millisecondsPerFrame;
            float a = (MathF.PI * (float)m_textureImage.Width * (float)m_textureImage.Height) / 4.0f;
            //m_r = MathF.Sqrt(a / MathF.PI);
            //m_r = frameSize.X * MathF.Sqrt(MathF.PI)*0.5f;
            m_r = (float)Math.Sqrt(MathF.Pow(m_frameSize.X, 2) + MathF.Pow(m_frameSize.Y, 2)) * 0.5f;
        }
        public void SetVelocity(Vector2 vel)
        {
            m_velocity = vel;
        }
        public bool Collide(Sprite other)
        {
            return GetBounds().Intersects(other.GetBounds());
        }
        public void PhysicsCollide(Sprite other)
        {
            Vector2 directionV = m_position - other.m_position;
            float d = MathF.Sqrt(MathF.Pow(directionV.X, 2) + MathF.Pow(directionV.Y, 2)); //get direction between asteroid length.

            if ((m_r + other.m_r) >= d) //if the two r are smaller than the distance between them, they collide.
            {
                Vector2 dirV = direction - other.direction;
                if (Vector2.Dot(dirV, directionV) > 0)
                {
                    return;
                }
                else
                {
                    int e = 1;
                    Vector2 WA = m_mass * direction + other.m_mass * other.direction + e * other.m_mass * (other.direction - direction) / (m_mass + other.m_mass);
                    Vector2 WB = m_mass * direction + other.m_mass * other.direction + e * m_mass * (direction - other.direction) / (m_mass + other.m_mass);
                    if(WA==Vector2.Zero)
                    {
                        int x = 1;
                    }
                    SetVelocity(WA);
                    other.SetVelocity(WB);
                    return;
                }
            }
            else
            {
                return;
            }
        }
        Rectangle GetBounds() { return new Rectangle(
                    (int)m_position.X + m_collisionOffset,
                    (int)m_position.Y + m_collisionOffset,
                    m_frameSize.X - (m_collisionOffset * 2),
                    m_frameSize.Y - (m_collisionOffset * 2));
        }
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            m_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (m_timeSinceLastFrame > m_millisecondsPerFrame)
            {
                m_timeSinceLastFrame = 0;
                ++m_currentFrame.X;
                if (m_currentFrame.X >= m_sheetSize.X)
                {
                    m_currentFrame.X = 0;
                    ++m_currentFrame.Y;
                    if (m_currentFrame.Y >= m_sheetSize.Y)
                        m_currentFrame.Y = 0;
                }
            }
        }
        public bool ClampWindow(Rectangle clientBounds, ref Vector2 position)
        {
            bool clamped = false;
            if (position.X < 0)
            {
                clamped = true;
                position.X = 0;
            }
                
            if (position.Y < 0)
            {
                clamped = true;
                position.Y = 0;
            }
               
            if (position.X > clientBounds.Width - m_frameSize.X)
            {
                clamped = true;
                position.X = clientBounds.Width - m_frameSize.X;
            }
                
            if (position.Y > clientBounds.Height - m_frameSize.Y)
            {
                clamped = true;
                position.Y = clientBounds.Height - m_frameSize.Y;
            }
             
            return clamped;
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_textureImage,
            m_position,
            new Rectangle(m_currentFrame.X * m_frameSize.X,
            m_currentFrame.Y * m_frameSize.Y,
            m_frameSize.X, m_frameSize.Y),
            Color.White, 0, Vector2.Zero,
            1f, m_effect, 0);
        }
        public void DrawStill(SpriteBatch spriteBatch)
        {
         
            spriteBatch.Draw(m_textureImage, m_position, m_textureImage.Bounds,
                Color.White, 0, Vector2.Zero,
            1f, m_effect, 0);
        }
        public void DrawStill(Texture2D texture, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
            m_position, texture.Bounds,
            Color.White, 0, Vector2.Zero,
            1f, m_effect, 0);
        }
        public Point GetCurrentFrame() { return m_currentFrame; }
        public abstract Vector2 direction
        {
            get;
        }
        
        public virtual void SetPosition(Vector2 pos)
        {
            m_position = pos;
           
        }
        public Texture2D GetTex() { return m_textureImage; }
        public Vector2 GetPos() { return m_position; }
        public void SetTex(Texture2D tex) { m_textureImage = tex; } 
    }
}

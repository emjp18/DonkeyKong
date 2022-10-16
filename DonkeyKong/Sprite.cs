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

        }
        public bool Collide(Sprite other)
        {
            return GetBounds().Intersects(other.GetBounds());
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
        protected bool ClampWindow(Rectangle clientBounds, ref Vector2 position)
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
        
        public abstract Vector2 direction
        {
            get;
        }
        public void Gravity(ref Vector2 position,ref bool isGrounded, GameTime gameTime)
        {
            isGrounded = position.Y + m_frameSize.Y % SpriteManager.g_tilesizeY == 0;

            if(!isGrounded)
            {
                m_position.Y = (int)(m_position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}

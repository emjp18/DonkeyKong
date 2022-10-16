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
        Vector2 m_direction;
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, float speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            m_direction = new Vector2(Math.Clamp(m_speed,0,1), Math.Clamp(m_speed, 0, 1));
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
           
            m_position += direction;
            base.Update(gameTime, clientBounds);
        }
        public void UpdateEnemyFire(GameTime gameTime, Rectangle clientBounds, Sprite DK)
        {
            m_direction.Y = 0;
            bool collideWithDK = Collide(DK);
            if (ClampWindow(clientBounds, ref m_position)|| collideWithDK)
            {
                m_direction.X *= -1;
                if(collideWithDK)
                {
                    m_position += m_direction * DK.GetTex().Width;
                    if(ClampWindow(clientBounds, ref m_position))
                    {
                        m_direction.X *= -1;
                        m_position += m_direction * DK.GetTex().Width*2;
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
                if (GetSpriteEffect()==SpriteEffects.None)
                {
                    SetSpriteEffect(SpriteEffects.FlipHorizontally);
                }
                else
                {
                    SetSpriteEffect(SpriteEffects.None);
                }
            }
            m_position += m_direction * m_speed*(float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime, clientBounds);
        }
        public void UpdateDK(GameTime gameTime, Rectangle clientBounds)
        {
            m_direction.Y = 0;
            m_position+= m_direction * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ClampWindow(clientBounds, ref m_position))
            {
                m_direction.X *= -1;

            }
            
            base.Update(gameTime, clientBounds);
        }
        public void UpdatePeach(GameTime gameTime, Rectangle clientBounds, Sprite player)
        {
            
            m_direction = player.direction;
            m_direction.Y = 0;
           
            if(m_direction.X>0)
            {
                SetSpriteEffect(SpriteEffects.FlipHorizontally);
            }
            else if(m_direction.X<0)
            {
                SetSpriteEffect(SpriteEffects.None);
            }


            m_position += m_direction * m_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ClampWindow(clientBounds, ref m_position))
            {
                m_direction.X *= -1;

            }
            base.Update(gameTime, clientBounds);
        }
    }
}

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
        public void RandomizeSpeed(int min=10, int max = 90) { Random random = new Random(); m_speed = (float)random.Next(min, max); }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
           
            m_position += direction;
            base.Update(gameTime, clientBounds);
        }
        public void UpdateEnemyFire(GameTime gameTime, Rectangle clientBounds)
        {
            m_direction.Y = 0;
            if (ClampWindow(clientBounds, ref m_position))
            {
                m_direction.X *= -1;
                if(GetSpriteEffect()==SpriteEffects.None)
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
    }
}

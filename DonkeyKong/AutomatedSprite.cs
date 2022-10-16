using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DonkeyKong
{
    internal abstract class AutomatedSprite : Sprite
    {
      
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, float speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }
        public override Vector2 direction
        {
            get { return new Vector2(m_speed,m_speed); }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            m_position += direction;
            base.Update(gameTime, clientBounds);
        }
    }
}

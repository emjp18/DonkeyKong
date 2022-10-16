using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static DonkeyKong.SpriteManager;

namespace DonkeyKong
{
    internal class Tile : AutomatedSprite
    {
        public TILE_TYPE g_type;
        public Tile(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, float speed,
            int millisecondsPerFrame, TILE_TYPE type)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            g_type = type;
        }
        
    }
}

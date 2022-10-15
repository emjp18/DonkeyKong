using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
namespace DonkeyKong
{
    internal class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
       
       public static int g_tilesize = 50;
       private SpriteBatch spriteBatch;
       private UserControlledSprite player = null;
       private List<Sprite> spriteList = new List<Sprite>();
       private List<string> m_text;
       private static Tile[,] m_tiles;
       private Texture2D m_wallTex;
        private Texture2D m_floorTex;
        private Texture2D m_ladderTex;
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            if(player != null)
                player.Draw(gameTime, spriteBatch);
            
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            foreach (Tile tile in m_tiles)
            {
                tile.Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            m_wallTex = Game.Content.Load<Texture2D>("empty");
            m_floorTex = Game.Content.Load<Texture2D>("bridge");
            m_ladderTex = Game.Content.Load<Texture2D>("bridgeLadder");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            StreamReader sr = new StreamReader("Content/map.txt");
            m_text = new List<string>();
            while (!sr.EndOfStream)
            {
                m_text.Add(sr.ReadLine());
            }
            sr.Close();
            m_tiles = new Tile[m_text[0].Length, m_text.Count];

            for (int i = 0; i < m_tiles.GetLength(0); i++)
            {

                for (int j = 0; j < m_tiles.GetLength(1); j++)
                {

                    if (m_text[j][i] == 'w')
                    {

                        m_tiles[i, j] = new Tile(m_wallTex, new
                        Vector2(m_wallTex.Width * i, m_wallTex.Height
                        * j), new Point(m_wallTex.Width, m_wallTex.Height), 0, new Point(0, 0), new Point(0, 0), new Vector2(0, 0), 0, false);

                    }

                    else if (m_text[j][i] == 'f')
                    {

                        m_tiles[i, j] = new Tile(m_floorTex, new
                        Vector2(m_floorTex.Width * i, m_floorTex.Height
                        * j), new Point(m_floorTex.Width, m_floorTex.Height), 0, new Point(0, 0), new Point(0, 0), new Vector2(0, 0), 0, false);

                    }

                    else if (m_text[j][i] == 'l')
                    {

                        m_tiles[i, j] = new Tile(m_ladderTex, new
                        Vector2(m_ladderTex.Width * i, m_ladderTex.Height
                        * j), new Point(m_ladderTex.Width, m_ladderTex.Height), 0, new Point(0, 0), new Point(0, 0), new Vector2(0, 0), 0, true);

                    }

                }

            }
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (player != null)
                player.Update(gameTime, Game.Window.ClientBounds);
           
            foreach (Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
            }
            base.Update(gameTime);
        }
        public SpriteManager(Game game)
            : base(game)
        {
            
        }
        public static bool GetTileLadderAtPosition(Vector2 vec)
        {
            return m_tiles[(int)vec.X / g_tilesize, (int)vec.Y / g_tilesize].g_ladder;
        }
        
    }
}

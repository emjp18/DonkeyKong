using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using static DonkeyKong.GameStateManager;

namespace DonkeyKong
{
    internal class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private bool collision = false;
        private Random m_random = new Random();
        private float m_playerSpeed = 100;
        private float m_enemySpeed = 50;
        private int m_lives = 3;
       public enum TILE_TYPE { BRIDGE, LADDER, WALL};
        public static int g_tilesize;
        public static int g_tilesizeY;
       private SpriteBatch spriteBatch;
       private UserControlledSprite player = null;
       private List<AutomatedSprite> spriteList = new List<AutomatedSprite>();
       private List<string> m_text;
       private static Tile[,] m_tiles;
       private Texture2D m_wallTex;
       private Texture2D m_floorTex;
       private Texture2D m_ladderTex;
       private Texture2D m_marioFrontTex;
        private Texture2D m_marioBackTex;
        private Texture2D m_menuScreenTex;
        private Texture2D m_enemyTex;
        private const int ENEMYCOUNT = 7;
        AutomatedSprite m_menuSprite;
        public override void Draw(GameTime gameTime)
        {
            switch (Instance.GetCurrentGameState())
            {
                case GAMESTATE.WIN:
                    {
                        break;
                    }
                case GAMESTATE.LOSE:
                    {
                        
                        break;
                    }
                case GAMESTATE.MENU:
                    {
                        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(gameTime, spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case GAMESTATE.NONE:
                    {
                        break;
                    }
                case GAMESTATE.GAME:
                    {
                        spriteBatch.Begin();

                        foreach (Tile tile in m_tiles)
                        {
                            tile.DrawStill(gameTime, spriteBatch);
                        }

                        foreach (Sprite s in spriteList)
                            s.DrawStill(gameTime, spriteBatch);
                        if (player != null)
                            player.Draw(gameTime, spriteBatch);

                        spriteBatch.End();
                        break;
                    }
            }


           
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            m_enemyTex = Game.Content.Load<Texture2D>("enemy");
            m_menuScreenTex = Game.Content.Load<Texture2D>("start");
            m_menuSprite = new AutomatedSprite(m_menuScreenTex, new Vector2(0, 0), new Point(m_menuScreenTex.Width, m_menuScreenTex.Height)
                , 0, new Point(0, 0), new Point(0, 0), 0, 0);
            m_marioBackTex = Game.Content.Load<Texture2D>("SuperMarioBack");
            m_marioFrontTex = Game.Content.Load<Texture2D>("SuperMarioFront");
            m_wallTex = Game.Content.Load<Texture2D>("empty");
            m_floorTex = Game.Content.Load<Texture2D>("bridge");
            m_ladderTex = Game.Content.Load<Texture2D>("ladder");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            g_tilesize = m_wallTex.Width;
            g_tilesizeY = m_wallTex.Height;
            StreamReader sr = new StreamReader("../../../Content/map.txt");
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
                        * j), new Point(m_wallTex.Width, m_wallTex.Height), 0, new Point(0, 0), new Point(0, 0), 0, 0, TILE_TYPE.WALL);

                    }

                    else if (m_text[j][i] == 'f')
                    {

                        m_tiles[i, j] = new Tile(m_floorTex, new
                        Vector2(m_floorTex.Width * i, m_floorTex.Height
                        * j), new Point(m_floorTex.Width, m_floorTex.Height), 0, new Point(0, 0), new Point(0, 0), 0, 0, TILE_TYPE.BRIDGE);

                    }

                    else if (m_text[j][i] == 'l')
                    {

                        m_tiles[i, j] = new Tile(m_ladderTex, new
                        Vector2(m_ladderTex.Width * i, m_ladderTex.Height
                        * j), new Point(m_ladderTex.Width, m_ladderTex.Height), 0, new Point(0, 0), new Point(0, 0), 0.0f, 0, TILE_TYPE.LADDER);

                    }

                }

            }
            
            player = new UserControlledSprite(m_marioFrontTex,m_marioBackTex,
                       new Vector2(Game1.G_W-m_marioFrontTex.Width, Game1.G_H-m_marioFrontTex.Height-m_wallTex.Height), 
                       new Point(m_marioFrontTex.Width, m_marioFrontTex.Height), 0, new Point(1, 1), new Point(0, 0), m_playerSpeed, 0);

            for(int i=0; i< ENEMYCOUNT; i++)
            {
                spriteList.Add(new AutomatedSprite(m_enemyTex, new Vector2(m_random.Next(Game1.G_W- m_enemyTex.Width), m_enemyTex.Height * 2*i + m_enemyTex.Height),
                    new Point(m_enemyTex.Width, m_enemyTex.Height), 0, new Point(1, 1), new Point(0, 0)
                    , m_enemySpeed, 0));
            }

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            switch (Instance.GetCurrentGameState())
            {
                case GAMESTATE.WIN:
                    {
                        break;
                    }
                case GAMESTATE.LOSE:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            Instance.SetCurrentGameState(GAMESTATE.MENU);
                        }
                        break;
                    }
                case GAMESTATE.MENU:
                    {
                        m_lives = 3;
                        player.SetPosition(new Vector2(Game1.G_W - m_marioFrontTex.Width, Game1.G_H - m_marioFrontTex.Height - m_wallTex.Height));
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            Instance.SetCurrentGameState(GAMESTATE.GAME);
                        }
                        break;
                    }
                case GAMESTATE.NONE:
                    {
                        break;
                    }
                case GAMESTATE.GAME:
                    {
                        if (player != null)
                            player.Update(gameTime, Game.Window.ClientBounds);
                        foreach (AutomatedSprite s in spriteList)
                        {
                            s.UpdateEnemyFire(gameTime, Game.Window.ClientBounds);
                            if (player.Collide(s))
                            {
                                player.KnockBack();
                                
                            }
                        }
                            
                        if(m_lives<=0)
                        {
                            Instance.SetCurrentGameState(GAMESTATE.LOSE);
                        }
                        
                        break;
                    }
            }
            
            base.Update(gameTime);
        }
        public SpriteManager(Game game)
            : base(game)
        {
            
        }
        public static TILE_TYPE GetTileTypeAtPosition(Vector2 vec)
        {
            
            return m_tiles[(int)vec.X / g_tilesize, (int)vec.Y / g_tilesize].g_type;
        }
        public static Tile GetTileAtPosition(Vector2 vec)
        {
            return m_tiles[(int)vec.X / g_tilesize, (int)vec.Y / g_tilesize];

        }
    }
}

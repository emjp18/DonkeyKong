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
        private int m_sprintNumber = 0;
        private bool m_sprintLoose1 = false;
        private bool m_sprintLoose0 = false;
        private bool m_DKFALL = false;
        public enum HAMMER_STATE { DOWN = 1 << 2, UP = 1 << 1,PICKED_UP = 1<<0, DEFAULT = 0}
        private HAMMER_STATE m_hammerState = HAMMER_STATE.DEFAULT;
        public enum AVATAR { MARIO, PAULINE};
        private AVATAR m_ava = AVATAR.MARIO;
        private const int M_ANISHEETLENGTHX = 21;
        private const int M_ANISHEETLENGTHY = 5;
        private const int M_LIVES = 5;
        private int m_randomTimeMax = 5;
        private int m_randomTimeMin = 1;
        private Timer m_timer;
        private Timer m_timer2;
        private Random m_random = new Random();
        private float m_playerSpeed = 100;
        private float m_enemySpeed = 50;
        private bool m_canStart = false;
        public enum TILE_TYPE { BRIDGE, LADDER, WALL, SPRINT };
        public static int g_tilesize;
        public static int g_tilesizeY;
        private SpriteBatch m_spriteBatch;
        private UserControlledSprite m_player = null;
        private List<AutomatedSprite> m_spriteList = new List<AutomatedSprite>();
        private List<string> m_text;
        private static Tile[,] m_tiles;
        private Texture2D m_wallTex;
        private Texture2D m_floorTex;
        private Texture2D m_ladderTex;
        private Texture2D m_marioFrontTex;
        private Texture2D m_marioBackTex;
        private Texture2D m_menuScreenTex;
        private Texture2D m_gameOverScreenTex;
        private Texture2D m_enemyTex;
        private Texture2D m_winTex;
        private Texture2D m_peachTex;
        private Texture2D m_DKTex;
        private Texture2D m_starAniTex;
        private Texture2D m_mushroomTex;
        private const int M_ENEMYCOUNT = 7;
        private AutomatedSprite m_menuSprite;
        private AutomatedSprite m_peachSprite;
        private AutomatedSprite m_DKSprite;
        private AutomatedSprite m_startAnimation;
        private Texture2D m_heartTex;
        private List<AutomatedSprite> m_hearts;
        private Texture2D m_scoreTex;
        private Texture2D m_paulineAvaTex;
        private AutomatedSprite m_scoreSprites;
        private AutomatedSprite m_hammer;
        private Texture2D m_hammerTex;
        private Texture2D m_hammerDownTex;
        private AutomatedSprite[] m_pickupSprites;
        private  int m_score = 0;
        private bool m_doOnce = false;
        private bool m_doOnceMenu = false;
        private SpriteFont m_font;
        private Texture2D m_sprintTex;
        string m_scoreString= "";
        const int M_PICKUPS = 3;
        int m_pickupscore = 0;
        Vector2 m_dir = Vector2.Zero;
        Timer m_hammerTimer;
        const double M_HAMMERDURATION = 10.0;
        public override void Draw(GameTime gameTime)
        {
            switch (Instance.GetCurrentGameState())
            {
                case GAMESTATE.WIN:
                    {
                        m_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(m_winTex, m_spriteBatch);
                        string name = "Mario";
                        int score = m_score + m_pickupscore;
                        if (m_ava == AVATAR.PAULINE)
                        {
                            name = "Pauline";
                        }
                        m_spriteBatch.DrawString(m_font, "HIGHSCORE\n"+m_scoreString+"\n"+ name+" "+ score, new Vector2(0, 0), Color.White);
                        m_spriteBatch.End();
                        break;
                    }
                case GAMESTATE.LOSE:
                    {
                        string name = "Mario";
                        int score = m_score + m_pickupscore;
                        if (m_ava == AVATAR.PAULINE)
                        {
                            name = "Pauline";
                        }

                        m_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(m_gameOverScreenTex, m_spriteBatch);
                        m_spriteBatch.DrawString(m_font, "HIGHSCORE\n" + m_scoreString + "\n" + name + " " + score, new Vector2(0, 0), Color.White);
                        m_spriteBatch.End();
                        break;
                    }
                case GAMESTATE.MENU:
                    {
                        m_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(m_menuScreenTex, m_spriteBatch);
                        m_startAnimation.SetPosition(new Vector2(0, Game1.G_H - m_starAniTex.Height));
                        m_startAnimation.Draw(gameTime, m_spriteBatch);
                        m_startAnimation.SetPosition(new Vector2(m_starAniTex.Width*2/ M_ANISHEETLENGTHX, Game1.G_H - m_starAniTex.Height));
                        m_startAnimation.Draw(gameTime, m_spriteBatch);
                        m_startAnimation.SetPosition(new Vector2(m_starAniTex.Width*4/ M_ANISHEETLENGTHX, Game1.G_H - m_starAniTex.Height));
                        m_startAnimation.Draw(gameTime, m_spriteBatch);
                        m_startAnimation.SetPosition(new Vector2(m_starAniTex.Width*6/ M_ANISHEETLENGTHX, Game1.G_H - m_starAniTex.Height));
                        m_startAnimation.Draw(gameTime, m_spriteBatch);
                        m_startAnimation.SetPosition(new Vector2(m_starAniTex.Width*8/ M_ANISHEETLENGTHX, Game1.G_H - m_starAniTex.Height));
                        m_startAnimation.Draw(gameTime, m_spriteBatch);
                        m_spriteBatch.End();
                        break;
                    }
                case GAMESTATE.NONE:
                    {
                        break;
                    }
                case GAMESTATE.GAME:
                    {
                        m_spriteBatch.Begin();
                        if (m_DKSprite.g_draw)
                            m_DKSprite.Draw(gameTime, m_spriteBatch);
                        foreach (Tile tile in m_tiles)
                        {
                            if(tile.g_draw)
                                tile.DrawStill(m_spriteBatch);
                        }

                        foreach (Sprite s in m_spriteList)
                            s.DrawStill(m_spriteBatch);
                        
                        m_player.Draw(gameTime, m_spriteBatch);
                        m_peachSprite.Draw(gameTime, m_spriteBatch);


                        foreach (Sprite heart in m_hearts)
                        {
                            heart.DrawStill(m_spriteBatch);
                        }
                        switch (m_player.g_levelReached)
                        {
                            case UserControlledSprite.LEVELHEIGHT.ONE:
                                {
                                    
                                    m_score = 0;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.TWO:
                                {
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    
                                    m_score = 1;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.THREE:
                                {
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 2, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                   
                                    m_score = 2;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.FOUR:
                                {
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 2, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 3, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    
                                    m_score = 3;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.FIVE:
                                {
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 2, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 3, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 4, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    
                                    m_score = 4;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.SIX:
                                {
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 2, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 3, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 4, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width * 5, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    
                                    m_score = 5;
                                    break;
                                }
                            case UserControlledSprite.LEVELHEIGHT.SEVEN:
                                {
                                    m_score = 6;
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width*2, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width*3, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width*4, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width*5, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);
                                    m_scoreSprites.SetPosition(new Vector2(Game1.G_W - m_scoreTex.Width*6, 0));
                                    m_scoreSprites.DrawStill(m_spriteBatch);

                                    break;
                                }
                        }
                        for (int i = 0; i < M_PICKUPS; i++)
                        {
                            if (m_pickupSprites[i].g_draw)
                            {
                                m_pickupSprites[i].DrawStill(m_spriteBatch);
                            }
                        }
                        if(m_hammer.g_draw)
                            m_hammer.DrawStill(m_spriteBatch);
                        m_spriteBatch.End();
                        break;
                    }
            }


           
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            m_sprintTex = Game.Content.Load<Texture2D>("bridgeLadder");
            m_hammerDownTex = Game.Content.Load<Texture2D>("hammerDown");
            m_hammerTex = Game.Content.Load<Texture2D>("hammer");
            m_mushroomTex = Game.Content.Load<Texture2D>("mushroom");
            m_font = Game.Content.Load<SpriteFont>("File");
            m_paulineAvaTex = Game.Content.Load<Texture2D>("paulineAva");
            m_scoreTex = Game.Content.Load<Texture2D>("cheese");
            m_heartTex = Game.Content.Load<Texture2D>("heart");
            m_starAniTex = Game.Content.Load<Texture2D>("mario-pauline");
            m_DKTex = Game.Content.Load<Texture2D>("DK_mod_mario");
            m_peachTex = Game.Content.Load<Texture2D>("pauline");
            m_winTex = Game.Content.Load<Texture2D>("win");
            m_gameOverScreenTex = Game.Content.Load<Texture2D>("loose");
            m_enemyTex = Game.Content.Load<Texture2D>("enemy");
            m_menuScreenTex = Game.Content.Load<Texture2D>("start");
            m_menuSprite = new AutomatedSprite(m_menuScreenTex, new Vector2(0, 0), new Point(m_menuScreenTex.Width, m_menuScreenTex.Height)
                , 0, new Point(0, 0), new Point(0, 0), 0, 0);
            m_marioBackTex = Game.Content.Load<Texture2D>("SuperMarioBack");
            m_marioFrontTex = Game.Content.Load<Texture2D>("SuperMarioFront");
            m_wallTex = Game.Content.Load<Texture2D>("empty");
            m_floorTex = Game.Content.Load<Texture2D>("bridge");
            m_ladderTex = Game.Content.Load<Texture2D>("ladder");
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            g_tilesize = m_wallTex.Width;
            g_tilesizeY = m_wallTex.Height;
            StreamReader sr = new StreamReader("../../../Content/map.txt");
            m_text = new List<string>();
            m_timer = new Timer();
            m_hammerTimer = new Timer();
            m_timer2 = new Timer();
            m_startAnimation = new AutomatedSprite(m_starAniTex, new Vector2(0,0),
                new Point(m_starAniTex.Width/ M_ANISHEETLENGTHX, m_starAniTex.Height/ M_ANISHEETLENGTHY), 0, new Point(0, 0),
               new Point(M_ANISHEETLENGTHX, M_ANISHEETLENGTHY), 75.0f, 250);

            m_hammer = new AutomatedSprite(m_hammerTex, new Vector2(0, 0),
                new Point(m_hammerTex.Width, m_hammerTex.Height), 0, new Point(0, 0),
               new Point(0, 0), 0, 0);

            m_pickupSprites = new AutomatedSprite[M_PICKUPS];
            for(int i=0; i<M_PICKUPS; i++)
            {
                m_pickupSprites[i] = new AutomatedSprite(m_mushroomTex, new Vector2(0, 0),
                new Point(m_mushroomTex.Width, m_mushroomTex.Height), 0, new Point(0, 0),
               new Point(0, 0), 0, 0);
            }
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
                        * j), new Point(m_wallTex.Width, m_wallTex.Height), 0, new Point(0, 0), new Point(0, 0), 0.1f*j, 0, TILE_TYPE.WALL);
                        m_tiles[i, j].g_draw = false;
                    }

                    else if (m_text[j][i] == 'f')
                    {

                        m_tiles[i, j] = new Tile(m_floorTex, new
                        Vector2(m_floorTex.Width * i, m_floorTex.Height
                        * j), new Point(m_floorTex.Width, m_floorTex.Height), 0, new Point(0, 0), new Point(0, 0), 10, 0, TILE_TYPE.BRIDGE);

                    }

                    else if (m_text[j][i] == 'l')
                    {

                        m_tiles[i, j] = new Tile(m_ladderTex, new
                        Vector2(m_ladderTex.Width * i, m_ladderTex.Height
                        * j), new Point(m_ladderTex.Width, m_ladderTex.Height), 0, new Point(0, 0), new Point(0, 0), 0.1f * j, 0, TILE_TYPE.LADDER);

                    }
                    else if (m_text[j][i] == 's')
                    {

                        m_tiles[i, j] = new Tile(m_sprintTex, new
                        Vector2(m_sprintTex.Width * i, m_sprintTex.Height
                        * j), new Point(m_sprintTex.Width, m_sprintTex.Height), 0, new Point(0, 0), new Point(0, 0), 0.1f * j, 0, TILE_TYPE.SPRINT);

                    }
                    m_tiles[i, j].SetDirection(new Vector2(0, 1));
                    m_tiles[i, j].g_update = false;
                    m_tiles[i, j].SetMass(m_random.Next(5, 20));
                    
                }

            }

            m_player = new UserControlledSprite(m_starAniTex, m_starAniTex,
                       new Vector2(Game1.G_W- (m_starAniTex.Width / M_ANISHEETLENGTHX), Game1.G_H- (m_starAniTex.Height / M_ANISHEETLENGTHY )- m_wallTex.Height), 
                       new Point(m_starAniTex.Width / M_ANISHEETLENGTHX, m_starAniTex.Height / M_ANISHEETLENGTHY), 0, new Point(0, 0),
                       new Point(M_ANISHEETLENGTHX, M_ANISHEETLENGTHY), m_playerSpeed, 250);
            m_peachSprite = new AutomatedSprite(m_starAniTex,
                       new Vector2(Game.Window.ClientBounds.Center.X, m_wallTex.Height * 2),
                       new Point(m_starAniTex.Width / M_ANISHEETLENGTHX, m_starAniTex.Height / M_ANISHEETLENGTHY), 0, new Point(0, 0),
                       new Point(M_ANISHEETLENGTHX, M_ANISHEETLENGTHY), m_playerSpeed*0.75f, 250);
            m_DKSprite = new AutomatedSprite(m_DKTex, new Vector2((Game1.G_W/2)- m_DKTex.Width, (Game1.G_H / 2)- m_DKTex.Height),
                new Point(m_DKTex.Width/4, m_DKTex.Height/3), 0, new Point(0, 0),
               new Point(4, 3), 75.0f, 500);
            m_DKSprite.SetMass(20.0f);
            
            for (int i=0; i< M_ENEMYCOUNT; i++)
            {
                if (i == 0||i==1)
                    continue;
                m_spriteList.Add(new AutomatedSprite(m_enemyTex, new Vector2(m_random.Next(Game1.G_W- m_enemyTex.Width), m_enemyTex.Height * 2*i + m_enemyTex.Height),
                    new Point(m_enemyTex.Width, m_enemyTex.Height), 0, new Point(1, 1), new Point(0, 0)
                    , m_enemySpeed, 0));
                if(m_spriteList[i - 2].Collide(m_DKSprite))
                {
                    m_spriteList[i - 2].SetPosition(new Vector2(m_spriteList[i - 2].GetPos().X + m_DKSprite.GetTex().Width, m_spriteList[i - 2].GetPos().Y));
                    Vector2 temp = m_spriteList[i - 2].GetPos();
                    if (m_spriteList[i - 2].ClampWindow(Game.Window.ClientBounds, ref temp))
                    {
                        m_spriteList[i - 2].SetPosition(new Vector2(m_spriteList[i - 2].GetPos().X - m_DKSprite.GetTex().Width*2, m_spriteList[i - 2].GetPos().Y));
                    }
                }
                m_spriteList[i-2].SetSpriteEffect(SpriteEffects.FlipHorizontally);
                m_spriteList[i - 2].SetDirection(new Vector2(1, 0));


            }
            m_hearts = new List<AutomatedSprite>();
            m_scoreSprites = new AutomatedSprite(m_scoreTex, new Vector2(0, 0),
                    new Point(m_scoreTex.Width, m_scoreTex.Height), 0, new Point(1, 1), new Point(0, 0), 0, 0);
            for (int i=0; i<m_player.g_lives; i++)
            {
                m_hearts.Add(new AutomatedSprite(m_heartTex, new Vector2(i* m_heartTex.Width, 0),
                    new Point(m_heartTex.Width, m_heartTex.Height), 0, new Point(1, 1), new Point(0, 0), 0, 0));
            }
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            switch (Instance.GetCurrentGameState())
            {
                case GAMESTATE.WIN:
                    {
                        m_doOnceMenu = false;
                        if (!m_doOnce)
                        {
                            StreamReader sr = new StreamReader("../../../Content/score.txt");
                            m_scoreString = sr.ReadToEnd();
                            
                            sr.Close();
                            StreamWriter sw = new StreamWriter("../../../Content/score.txt");
                            string name = "Mario";
                            int score = m_pickupscore + m_score;
                            if (m_ava == AVATAR.PAULINE)
                            {
                                name = "Pauline";
                            }
                            if (m_scoreString.Length == 0)
                            {
                                sw.Write(name + " " + score);
                            }
                            else
                            {
                                sw.Write(m_scoreString + "\n" + name + " " + score);
                            }
                            sw.Close();
                            m_doOnce = true;
                        }
                        
                        m_canStart = false;
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            Instance.SetCurrentGameState(GAMESTATE.MENU);
                        }
                        break;
                    }
                case GAMESTATE.LOSE:
                    {
                        m_doOnceMenu = false;
                        if (!m_doOnce)
                        {
                            StreamReader sr = new StreamReader("../../../Content/score.txt");
                            m_scoreString = sr.ReadToEnd();
                            sr.Close();
                            StreamWriter sw = new StreamWriter("../../../Content/score.txt");
                            string name = "Mario";
                            int score = m_pickupscore + m_score;
                            if (m_ava == AVATAR.PAULINE)
                            {
                                name = "Pauline";
                            }
                            if(m_scoreString.Length==0)
                            {
                                sw.Write(name + " " + score);
                            }
                            else
                            {
                                sw.Write(m_scoreString + "\n" + name+" "+ score);
                            }
                            
                            sw.Close();
                            m_doOnce = true;
                        }
                        m_canStart = false;
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            Instance.SetCurrentGameState(GAMESTATE.MENU);
                        }
                        break;
                    }
                case GAMESTATE.MENU:
                    {
                        m_doOnce = false;
                        if(!m_doOnceMenu)
                        {
                            for (int i = 0; i < m_tiles.GetLength(0); i++)
                            {

                                for (int j = 0; j < m_tiles.GetLength(1); j++)
                                {

                                    if (m_tiles[i, j].g_type == TILE_TYPE.WALL)
                                    {
                                        m_tiles[i, j].SetPosition(new Vector2(m_wallTex.Width * i, m_wallTex.Height
                                        * j));


                                    }

                                    else if (m_tiles[i, j].g_type == TILE_TYPE.BRIDGE)
                                    {
                                        m_tiles[i, j].SetPosition(new Vector2(m_floorTex.Width * i, m_floorTex.Height
                                        * j));


                                    }

                                    else if (m_tiles[i, j].g_type == TILE_TYPE.LADDER)
                                    {
                                        m_tiles[i, j].SetPosition(new Vector2(m_ladderTex.Width * i, m_ladderTex.Height
                                        * j));


                                    }
                                    else if (m_tiles[i, j].g_type == TILE_TYPE.SPRINT)
                                    {
                                        m_tiles[i, j].SetPosition(new Vector2(m_sprintTex.Width * i, m_sprintTex.Height
                                        * j));
                                       

                                    }
                                    m_tiles[i, j].SetDirection(new Vector2(0, 1));
                                    m_tiles[i, j].SetMass(m_random.Next(5,15));
                                    m_tiles[i, j].RandomizeSpeed();
                                    m_tiles[i, j].g_update = false;
                                }

                            }
                            m_DKSprite.SetPosition(new Vector2((Game1.G_W / 2) - m_DKTex.Width, (Game1.G_H / 2) - m_DKTex.Height));
                            for (int i = 0; i < M_ENEMYCOUNT; i++)
                            {
                                if (i == 0 || i == 1)
                                    continue;
                                m_spriteList[i-2].SetPosition(new Vector2(m_random.Next(Game1.G_W - m_enemyTex.Width), m_enemyTex.Height * 2 * i + m_enemyTex.Height));
                                if (m_spriteList[i - 2].Collide(m_DKSprite))
                                {
                                    m_spriteList[i - 2].SetPosition(new Vector2(m_spriteList[i - 2].GetPos().X + m_DKSprite.GetTex().Width, m_spriteList[i - 2].GetPos().Y));
                                    Vector2 temp = m_spriteList[i - 2].GetPos();
                                    if (m_spriteList[i - 2].ClampWindow(Game.Window.ClientBounds, ref temp))
                                    {
                                        m_spriteList[i - 2].SetPosition(new Vector2(m_spriteList[i - 2].GetPos().X - m_DKSprite.GetTex().Width * 2, m_spriteList[i - 2].GetPos().Y));
                                    }
                                }
                                m_spriteList[i - 2].SetSpriteEffect(SpriteEffects.FlipHorizontally);
                                m_spriteList[i - 2].SetDirection(new Vector2(1, 0));


                            }
                            foreach (AutomatedSprite s in m_spriteList)
                            {
                                s.RandomizeSpeed();
                            }
                            m_sprintNumber = 0;
                            m_hammer.SetPosition(new Vector2(g_tilesize * 2, g_tilesizeY * 11));
                            m_hammer.Update(gameTime, Game.Window.ClientBounds);
                            m_pickupscore = 0;
                            m_pickupSprites[0].SetPosition(new Vector2(g_tilesize * 18, g_tilesizeY * 15));
                            m_pickupSprites[1].SetPosition(new Vector2(g_tilesize * 11, g_tilesizeY * 9));
                            m_pickupSprites[2].SetPosition(new Vector2(g_tilesize * 10, g_tilesizeY * 11));
                            m_pickupSprites[0].Update(gameTime, Game.Window.ClientBounds);
                            m_pickupSprites[1].Update(gameTime, Game.Window.ClientBounds);
                            m_pickupSprites[2].Update(gameTime, Game.Window.ClientBounds);
                            
                            m_timer.ResetAndStart(m_randomTimeMin);
                            m_timer2.ResetAndStart(m_randomTimeMin);
                            m_player.g_lives = M_LIVES;
                            m_peachSprite.SetPosition(new Vector2(0, m_wallTex.Height * 3));
                            m_player.SetPosition(new Vector2(Game1.G_W - (m_starAniTex.Width / M_ANISHEETLENGTHX), Game1.G_H - (m_starAniTex.Height / M_ANISHEETLENGTHY) - m_wallTex.Height));
                            m_doOnceMenu = true;
                        }
                        
                        if(Keyboard.GetState().IsKeyUp(Keys.Enter))
                        {
                            m_canStart = true;
                            
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter)&& m_canStart)
                        {
                            m_hammerTimer.ResetAndStart(M_HAMMERDURATION);
                            Instance.SetCurrentGameState(GAMESTATE.GAME);
                            m_ava = AVATAR.MARIO;
                            
                           
                            m_player.SetAva(m_ava);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Space) && m_canStart)
                        {
                            m_hammerTimer.ResetAndStart(M_HAMMERDURATION);
                            Instance.SetCurrentGameState(GAMESTATE.GAME);
                            m_ava = AVATAR.PAULINE;
                           
                            m_player.SetAva(m_ava);
                            
                        }
                        m_startAnimation.Update(gameTime, Game.Window.ClientBounds);
                        break;
                    }
                case GAMESTATE.NONE:
                    {
                        break;
                    }
                case GAMESTATE.GAME:
                    {
                        if(m_sprintLoose1 && m_sprintLoose0)
                        {
                            m_DKFALL = true;
                        }
                        if(m_DKFALL)
                        {
                            m_DKSprite.SetVelocity(new Vector2(0, 1));
                            foreach (Tile t in m_tiles)
                            {
                                if(t.g_type==TILE_TYPE.BRIDGE&&t.GetPos().Y>g_tilesizeY*4&&(
                                    t.GetPos().X > g_tilesize*3&& t.GetPos().X < g_tilesize * 18))
                                {
                                    t.g_update = true;
                                }
                            }
                            foreach(AutomatedSprite e in m_spriteList)
                            {
                                e.g_update = true;
                                e.SetVelocity(new Vector2(0, 1));
                            }
                            for(int p=0; p<M_PICKUPS; p++)
                            {
                                if (m_pickupSprites[p].g_draw)
                                {
                                    m_pickupSprites[p].g_draw = false;
                                }
                            }
                            if((m_player.GetPos().X>g_tilesize*3 && m_player.GetPos().X < g_tilesize * 18) && 
                                m_player.GetPos().Y > g_tilesizeY * 4
                               )
                            {
                                m_player.SetVelocity(new Vector2(0, 1));
                            }
                        }
                        int countI = 0;
                        
                        foreach (Tile t in m_tiles)
                        {
                            ;
                            if(t.g_update)
                            {
                                t.UpdateTile(gameTime, Game.Window.ClientBounds);
                                int countj = 0;
                                if(!m_sprintLoose0)
                                {
                                    m_sprintNumber = countI;
                                    m_sprintLoose0 = (t.g_type == TILE_TYPE.SPRINT);
                                    
                                } 
                                else if(m_sprintNumber != countI&&!m_sprintLoose1)
                                {
                                    m_sprintLoose1 = (t.g_type == TILE_TYPE.SPRINT);
                                }
                                if(m_DKFALL)
                                {
                                    if (m_DKSprite.Collide(t))
                                    {
                                        m_DKSprite.PhysicsCollide(t);
                                    }
                                    if (m_player.Collide(t))
                                    {
                                        m_player.PhysicsCollide(t);
                                    }
                                    foreach (AutomatedSprite e in m_spriteList)
                                    {
                                        if (e.g_update)
                                        {
                                            if (e.Collide(t))
                                            {
                                                e.PhysicsCollide(t);
                                            }
                                            if (e.Collide(m_DKSprite))
                                            {
                                                e.PhysicsCollide(m_DKSprite);
                                            }

                                        }
                                    }
                                }
                                
                               
                                foreach (Tile t1 in m_tiles)
                                {
                                    
                                    countj++;
                                    if (countI == countj)
                                    {
                                        continue;
                                    }
                                    if (t1.Collide(t))
                                    {
                                        t1.PhysicsCollide(t);
                                    }
                                    

                                }
                            }

                            countI++;
                            
                        }
                        if (m_hammerState.HasFlag(HAMMER_STATE.DEFAULT))
                        {
                            if(m_player.Collide(m_hammer))
                            {
                                m_hammerState = HAMMER_STATE.PICKED_UP;
                            }
                        }
                        if (m_hammerState.HasFlag(HAMMER_STATE.PICKED_UP)&&!m_hammerTimer.IsDone())
                        {
                            m_hammerTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

                            if (m_player.direction!=Vector2.Zero)
                            {
                                m_dir = m_player.direction;
                                
                            }
                            m_hammer.SetPosition(m_player.GetPos() + m_dir * m_hammerTex.Width);
                            m_hammer.SetSpriteEffect(m_player.GetSpriteEffect());
                            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                            {
                                m_hammerState = m_hammerState & ~HAMMER_STATE.UP;
                                m_hammerState |= HAMMER_STATE.DOWN;
                                m_hammer.SetTex(m_hammerDownTex);
                                foreach (AutomatedSprite e in m_spriteList)
                                {
                                    if (m_hammer.Collide(e))
                                    {
                                        e.SetDirection(new Vector2(e.direction.X * -1, e.direction.Y));
                                        if(e.GetSpriteEffect()==SpriteEffects.None)
                                        {
                                            e.SetSpriteEffect(SpriteEffects.FlipHorizontally);
                                        }
                                        else
                                        {
                                            e.SetSpriteEffect(SpriteEffects.None);  
                                        }
                                        
                                    }
                                    
                            }
                            }
                            else
                            {
                                m_hammerState = m_hammerState & ~HAMMER_STATE.DOWN;
                                m_hammerState |= HAMMER_STATE.UP;
                                m_hammer.SetTex(m_hammerTex);
                            }
                            
                        }

                        if(m_hammerTimer.IsDone())
                        {
                            m_hammer.g_draw = false;
                        }


                        m_player.Update(gameTime, Game.Window.ClientBounds);
                        if(m_ava==AVATAR.MARIO)
                            m_peachSprite.UpdatePeach(gameTime, Game.Window.ClientBounds, m_player, AVATAR.PAULINE);
                        else
                            m_peachSprite.UpdatePeach(gameTime, Game.Window.ClientBounds, m_player, AVATAR.MARIO);
                        foreach (AutomatedSprite s in m_spriteList)
                        {
                            s.UpdateEnemyFire(gameTime, Game.Window.ClientBounds, m_DKSprite);
                            if (m_player.Collide(s))
                            {
                                m_player.KnockBack(s.direction, Game.Window.ClientBounds);
                                Instance.SetCurrentPlayState(PLAYSTATE.PUSHED);

                            }
                        }
                        for (int i = 0; i < M_PICKUPS; i++)
                        {
                            if(m_pickupSprites[i].g_draw)
                            {
                                if(m_player.Collide(m_pickupSprites[i]))
                                {
                                    m_pickupSprites[i].g_draw = false;
                                    Instance.SetCurrentPlayState(PLAYSTATE.WIN);
                                    
                                    m_pickupscore++;
                                }
                            }
                        }

                        m_DKSprite.UpdateDK(gameTime, Game.Window.ClientBounds);
                        if (!m_timer2.IsDone())
                        {
                            m_timer2.Update(gameTime.ElapsedGameTime.TotalSeconds);
                            
                        }
                        else
                        { 
                            m_DKSprite.SetDirection(new Vector2(0, 0));
                            m_timer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                            if (m_timer.IsDone())
                            {
                                m_timer.ResetAndStart(m_random.Next(m_randomTimeMax));
                                m_DKSprite.SetDirection(new Vector2(m_random.Next(-1, 1), 0));
                                m_timer2.ResetAndStart(m_random.Next(m_randomTimeMax));
                            }
                        }
                       
                        if (m_player.Collide(m_DKSprite)&&!m_DKFALL)
                        {
                            m_player.KnockBack(m_DKSprite.direction, Game.Window.ClientBounds);
                            Instance.SetCurrentPlayState(PLAYSTATE.PUSHED);
                        }
                        if (m_player.g_lives <= 0||m_player.GetPos().Y>g_tilesizeY*17)
                        {
                            Instance.SetCurrentGameState(GAMESTATE.LOSE);
                            Instance.SetCurrentPlayState(PLAYSTATE.LOSE);
                        }

                        if (m_player.g_lives < m_hearts.Count)
                        {
                            m_hearts.RemoveAt(m_player.g_lives);

                        }
                        

                        if (m_player.Collide(m_peachSprite)&&m_score==6)
                        {
                            Instance.SetCurrentPlayState(PLAYSTATE.WIN);
                            Instance.SetCurrentGameState(GAMESTATE.WIN);
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

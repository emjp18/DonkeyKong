﻿using System;
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
        public enum AVATAR { MARIO, PAULINE};
        private AVATAR ava = AVATAR.MARIO;
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
        public enum TILE_TYPE { BRIDGE, LADDER, WALL };
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
        private  int m_score = 0;
        private bool m_doOnce = false;
        private bool m_doOnceMenu = false;
        public override void Draw(GameTime gameTime)
        {
            switch (Instance.GetCurrentGameState())
            {
                case GAMESTATE.WIN:
                    {
                        m_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(m_winTex, m_spriteBatch);
                        m_spriteBatch.End();
                        break;
                    }
                case GAMESTATE.LOSE:
                    {
                        m_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        m_menuSprite.DrawStill(m_gameOverScreenTex, m_spriteBatch);
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

                        foreach (Tile tile in m_tiles)
                        {
                            tile.DrawStill(m_spriteBatch);
                        }

                        foreach (Sprite s in m_spriteList)
                            s.DrawStill(m_spriteBatch);
                        m_DKSprite.DrawStill(m_spriteBatch);
                        m_player.Draw(gameTime, m_spriteBatch);
                        m_peachSprite.DrawStill(m_spriteBatch);


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

                        m_spriteBatch.End();
                        break;
                    }
            }


           
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            m_paulineAvaTex = Game.Content.Load<Texture2D>("paulineAva");
            m_scoreTex = Game.Content.Load<Texture2D>("cheese");
            m_heartTex = Game.Content.Load<Texture2D>("heart");
            m_starAniTex = Game.Content.Load<Texture2D>("mario-pauline");
            m_DKTex = Game.Content.Load<Texture2D>("DonkeyKong");
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
            
            m_timer2 = new Timer();
            m_startAnimation = new AutomatedSprite(m_starAniTex, new Vector2(0,0),
                new Point(m_starAniTex.Width/ M_ANISHEETLENGTHX, m_starAniTex.Height/ M_ANISHEETLENGTHY), 0, new Point(0, 0),
               new Point(M_ANISHEETLENGTHX, M_ANISHEETLENGTHY), 75.0f, 250);
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

            m_player = new UserControlledSprite(m_marioFrontTex,m_marioBackTex,
                       new Vector2(Game1.G_W-m_marioFrontTex.Width, Game1.G_H-m_marioFrontTex.Height-m_wallTex.Height), 
                       new Point(m_marioFrontTex.Width, m_marioFrontTex.Height), 0, new Point(1, 1), new Point(0, 0), m_playerSpeed, 0);
            m_peachSprite = new AutomatedSprite(m_peachTex, new Vector2(Game.Window.ClientBounds.Center.X, m_wallTex.Height*2),
                new Point(m_peachTex.Width, m_peachTex.Height), 0, new Point(m_peachTex.Width, m_peachTex.Height),
               new Point(m_peachTex.Width, m_peachTex.Height), 75.0f, 0);
            m_DKSprite = new AutomatedSprite(m_DKTex, new Vector2((Game1.G_W/2)- m_DKTex.Width, (Game1.G_H / 2)- m_DKTex.Height),
                new Point(m_DKTex.Width, m_DKTex.Height), 0, new Point(m_DKTex.Width, m_DKTex.Height),
               new Point(m_DKTex.Width, m_DKTex.Height), 75.0f, 0);
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
                            string scoreString = sr.ReadToEnd();
                            sr.Close();
                            StreamWriter sw = new StreamWriter("../../../Content/score.txt");
                            string name = "Mario";
                            if (ava == AVATAR.PAULINE)
                            {
                                name = "Pauline";
                            }
                            if (scoreString.Length == 0)
                            {
                                sw.Write(name + " " + m_score);
                            }
                            else
                            {
                                sw.Write(scoreString + "\n" + name + " " + m_score);
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
                            string scoreString = sr.ReadToEnd();
                            sr.Close();
                            StreamWriter sw = new StreamWriter("../../../Content/score.txt");
                            string name = "Mario";
                            if(ava==AVATAR.PAULINE)
                            {
                                name = "Pauline";
                            }
                            if(scoreString.Length==0)
                            {
                                sw.Write(name + " " + m_score);
                            }
                            else
                            {
                                sw.Write(scoreString + "\n" + name+" "+ m_score);
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
                            foreach (AutomatedSprite s in m_spriteList)
                            {
                                s.RandomizeSpeed();
                            }

                            m_timer.ResetAndStart(m_randomTimeMin);
                            m_timer2.ResetAndStart(m_randomTimeMin);
                            m_player.g_lives = M_LIVES;
                            m_peachSprite.SetPosition(new Vector2(Game.Window.ClientBounds.Center.X, m_wallTex.Height * 2));
                            m_player.SetPosition(new Vector2(Game1.G_W - m_marioFrontTex.Width, Game1.G_H - m_marioFrontTex.Height - m_wallTex.Height));
                            m_doOnceMenu = true;
                        }
                        
                        if(Keyboard.GetState().IsKeyUp(Keys.Enter))
                        {
                            m_canStart = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter)&& m_canStart)
                        {
                            Instance.SetCurrentGameState(GAMESTATE.GAME);
                            ava = AVATAR.MARIO;
                            m_player.SetTex(m_marioFrontTex);
                            m_peachSprite.SetTex(m_peachTex);
                            m_peachSprite.SetPosition(new Vector2(Game.Window.ClientBounds.Center.X, m_wallTex.Height * 2));
                            m_player.SetAva(ava);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Space) && m_canStart)
                        {
                            Instance.SetCurrentGameState(GAMESTATE.GAME);
                            ava = AVATAR.PAULINE;
                            m_player.SetTex(m_paulineAvaTex);
                            m_peachSprite.SetTex(m_marioFrontTex);
                            m_player.SetAva(ava);
                            m_peachSprite.SetPosition(new Vector2(Game.Window.ClientBounds.Center.X, m_wallTex.Height * 3));
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
                        
                        m_player.Update(gameTime, Game.Window.ClientBounds);
                        m_peachSprite.UpdatePeach(gameTime, Game.Window.ClientBounds, m_player);
                        foreach (AutomatedSprite s in m_spriteList)
                        {
                            s.UpdateEnemyFire(gameTime, Game.Window.ClientBounds, m_DKSprite);
                            if (m_player.Collide(s))
                            {
                                m_player.KnockBack(s.direction, Game.Window.ClientBounds);


                            }
                        }

                        
                        
                        if (!m_timer2.IsDone())
                        {
                            m_timer2.Update(gameTime.ElapsedGameTime.TotalSeconds);
                            m_DKSprite.UpdateDK(gameTime, Game.Window.ClientBounds);
                        }
                        else
                        {
                            m_timer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                            if (m_timer.IsDone())
                            {
                                m_timer.ResetAndStart(m_random.Next(m_randomTimeMax));
                                m_DKSprite.SetDirection(new Vector2(m_random.Next(-1, 1), 0));
                                m_timer2.ResetAndStart(m_random.Next(m_randomTimeMax));
                            }
                        }
                       
                        if (m_player.Collide(m_DKSprite))
                        {
                            m_player.KnockBack(m_DKSprite.direction, Game.Window.ClientBounds);
                        }
                        if (m_player.g_lives <= 0)
                        {
                            Instance.SetCurrentGameState(GAMESTATE.LOSE);
                        }

                        if (m_player.g_lives < m_hearts.Count)
                        {
                            m_hearts.RemoveAt(m_player.g_lives);

                        }
                        

                        if (m_player.Collide(m_peachSprite)&&m_score==6)
                        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.Utilities.Deflate;

namespace DonkeyKong
{
    internal class UserControlledSprite : Sprite
    {
        public enum LEVELHEIGHT{ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN};
        public LEVELHEIGHT g_levelReached = LEVELHEIGHT.ONE;
        private bool m_climbingLadder = false;
        private Vector2 m_destination;
        private bool m_moving = false;
        private Vector2 m_dir;
        private List<Tile> m_ladders;
        public int g_lives = 5;
        private bool m_knocked = false;
        private SpriteManager.AVATAR m_ava;
        public UserControlledSprite(Texture2D textureImage, Texture2D marioBack, Vector2 position,
                Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
                float speed, int millisecondsPerFrame)
                : base(textureImage, position, frameSize, collisionOffset, currentFrame,
                sheetSize, speed, millisecondsPerFrame)
        {
            
        }
        public void SetLadders(List<Tile> ladders)
        {
            m_ladders = ladders;
        }
        public override void SetPosition(Vector2 pos)
        {
            m_destination = pos;
            m_moving = false;
            base.SetPosition(pos);
        }
        public Vector2 AIDir( List<AutomatedSprite> enemies)
        {
            Vector2 currentPos = m_position;
            int x = m_frameSize.X - SpriteManager.g_tilesize;
            int y = m_frameSize.Y - SpriteManager.g_tilesizeY;
            currentPos.X += x;
            currentPos.Y += y;
            Vector2 dir = Vector2.Zero;
            float shortestLadderPath = float.MaxValue;
            float shortestEnemyPath = float.MaxValue;
            Vector2 ladderPos = Vector2.Zero;
            bool enemyToleft = false;
            foreach (Tile ladder in m_ladders)
            {
                if (ladder.g_marked)
                    continue;
                if(ladder.GetPos().Y==currentPos.Y)
                {
                    float d = Vector2.Distance(ladder.GetPos(), currentPos);
                    if (d < shortestLadderPath)
                    {
                        shortestLadderPath = d;
                        ladderPos = ladder.GetPos();

                    }
                    if (shortestLadderPath == 0)
                    {
                        break;
                    }
                }
                
            }
            foreach (AutomatedSprite enemy in enemies)
            {
                if (enemy.GetPos().Y == currentPos.Y)
                {
                    float d = Vector2.Distance(enemy.GetPos(), currentPos);
                    if(d< shortestEnemyPath)
                    {
                        shortestEnemyPath = d;
                        if (enemy.GetPos().X > currentPos.X) 
                        {
                            enemyToleft = false;
                        }
                        else
                        {
                            enemyToleft = true;
                        }
                    }
                }
            }
            if (shortestLadderPath == 0)
            {
                dir.X = 0;
                

                if (SpriteManager.GetTileTypeAtPosition(m_position) == SpriteManager.TILE_TYPE.LADDER)
                {
                    dir.Y = -1;
                    SpriteManager.GetTileAtPosition(m_position).g_marked = true;
                }
                else
                {
                    dir.Y = 0;
                    dir.X = -1;

                }
            }
            else if(shortestEnemyPath<shortestLadderPath)
            {
                if(enemyToleft)
                {
                    dir.X = 1;
                }
                else
                {
                    dir.X = -1;
                }
                foreach (Tile ladder in m_ladders)
                {
                    if(ladder.g_marked)
                    {
                        ladder.g_marked = false;
                    }
                    if (SpriteManager.GetTileTypeAtPosition(m_position) == SpriteManager.TILE_TYPE.LADDER)
                    {
                        dir.X = 0;
                    }
                }
            }
            else
            {
                
                dir = ladderPos - m_position;
                dir.Y = 0;
                dir.Normalize();
                dir.Round();
            }
            
            return dir;
        }
        public override Vector2 direction
        {
            get
            {
               
                Vector2 inputDirection = Vector2.Zero;
              
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X = -1;
                    m_effect = SpriteEffects.FlipHorizontally;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X = 1;
                    m_effect = SpriteEffects.None;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    inputDirection.Y = -1;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    inputDirection.Y = 1;

                }


                return inputDirection;
            }
        }
        //Add function to correct position if knockback changes it.
        public bool KnockBack(Vector2 direction, Rectangle clientBounds)
        {
            Math.Clamp(direction.X, 0,1);
            Math.Clamp(direction.Y, 0, 1);
            m_position.X = (int)m_position.X;
            m_position.Y = (int)m_position.Y;
            Vector2 newDestination = m_position + direction * SpriteManager.g_tilesizeY;
            if(!ClampWindow(clientBounds, ref newDestination)&& !m_climbingLadder)
            {
                m_destination = newDestination;
                m_moving = true;
                m_dir = direction;
                m_knocked = true;
                return true;
            }
            
            return false;

        }
        public void UpdateMario(GameTime gameTime, Rectangle clientBounds, List<AutomatedSprite> enemies)
        {
            if(m_position.Y > SpriteManager.g_tilesizeY * 2)
            {
                g_levelReached = LEVELHEIGHT.SEVEN;
                if (m_position.Y > SpriteManager.g_tilesizeY * 4)
                {
                    g_levelReached = LEVELHEIGHT.SIX;
                    if (m_position.Y > SpriteManager.g_tilesizeY * 4 + SpriteManager.g_tilesizeY * 2)
                    {
                        g_levelReached = LEVELHEIGHT.FIVE;
                        if (m_position.Y > SpriteManager.g_tilesizeY * 4 + SpriteManager.g_tilesizeY * 4)
                        {
                            g_levelReached = LEVELHEIGHT.FOUR;
                            if (m_position.Y > SpriteManager.g_tilesizeY * 4 + SpriteManager.g_tilesizeY * 6)
                            {
                                g_levelReached = LEVELHEIGHT.THREE;
                                if (m_position.Y > SpriteManager.g_tilesizeY * 4 + SpriteManager.g_tilesizeY * 8)
                                {
                                    g_levelReached = LEVELHEIGHT.TWO;
                                    if (m_position.Y > SpriteManager.g_tilesizeY * 4 + SpriteManager.g_tilesizeY * 10)
                                    {
                                        g_levelReached = LEVELHEIGHT.ONE;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (m_velocity != Vector2.Zero)
            {
                m_velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds * m_speed;
                m_position += m_velocity;
            }
            else
            {
                if (!m_moving && !m_knocked)
                {
                    if(GameStateManager.Instance.GetCurrentGameState()==GameStateManager.GAMESTATE.ATTRACT)
                    {
                        m_dir = AIDir(enemies);
                        if(m_dir.X>0)
                        {
                            m_effect = SpriteEffects.None;
                        }
                        else
                        {
                            m_effect = SpriteEffects.FlipHorizontally;
                        }
                    }
                    else
                    {
                        m_dir = direction;
                    }
                    
                    ChangeDirection(m_dir, clientBounds);
                }
                else
                {
                    m_position += (m_dir * m_speed *
                (float)gameTime.ElapsedGameTime.TotalSeconds);
                    ClampWindow(clientBounds, ref m_position);
                    if (Vector2.Distance(m_position, m_destination) < 1)
                    {
                        m_position = m_destination;
                        m_moving = false;
                        if (m_knocked)
                            g_lives--;
                        m_knocked = false;
                    }
                }
            }
            if(m_ava==SpriteManager.AVATAR.PAULINE)
            {
                m_currentFrame.Y = 1;
            }
            else
            {
                m_currentFrame.Y = 0;
            }
            if(m_climbingLadder)
            {
                m_currentFrame.X = 9;
            }
            else if(m_dir.X==0)
            {
                m_currentFrame.X = 0;
            }
            else
            {
                m_timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (m_timeSinceLastFrame > m_millisecondsPerFrame)
                {
                    m_timeSinceLastFrame = 0;
                    ++m_currentFrame.X;
                    if (m_currentFrame.X >= 9)
                    {
                        m_currentFrame.X = 0;

                    }
                }
            }
            



        }
        
        public void SetAva(SpriteManager.AVATAR ava)
        {
            m_ava = ava;
        }
        public void ChangeDirection(Vector2 dir, Rectangle clientBounds)
        {




            if (m_climbingLadder)
            {
                Vector2 newDestinationY = m_position + dir * SpriteManager.g_tilesizeY;
                ClampWindow(clientBounds, ref newDestinationY);
                SpriteManager.TILE_TYPE typeY = SpriteManager.GetTileTypeAtPosition(newDestinationY);

                if (typeY == SpriteManager.TILE_TYPE.LADDER)
                {
                    if ((dir.X != 0 && dir.Y == 0) || (dir.Y != 0 && dir.X == 0))
                    {
                        m_destination = newDestinationY;
                        m_moving = true;
                        m_climbingLadder = true;
                    }

                }
                else if (typeY == SpriteManager.TILE_TYPE.WALL)
                {
                    if (dir.Y == 0)
                    {
                        m_destination = newDestinationY;
                        m_moving = true;
                        m_climbingLadder = false;
                    }



                }
                else if (typeY == SpriteManager.TILE_TYPE.SPRINT)
                {
                    if (dir.Y == 0)
                    {
                        m_destination = newDestinationY;
                        m_moving = true;
                        m_climbingLadder = false;
                        Tile tile = SpriteManager.GetTileAtPosition(newDestinationY);
                        tile.g_update = true;
                    }

                }

            }
            else
            {
                Vector2 newDestination = m_position + dir * SpriteManager.g_tilesize;
                ClampWindow(clientBounds, ref newDestination);
                SpriteManager.TILE_TYPE type = SpriteManager.GetTileTypeAtPosition(newDestination);
                if (type == SpriteManager.TILE_TYPE.LADDER)
                {
                    if ((dir.X != 0 && dir.Y == 0) || (dir.Y != 0 && dir.X == 0))
                    {
                        m_destination = newDestination;
                        m_moving = true;
                        m_climbingLadder = true;
                    }

                }
                else if (type == SpriteManager.TILE_TYPE.WALL)
                {
                    if (dir.Y == 0)
                    {
                        m_destination = newDestination;
                        m_moving = true;
                        m_climbingLadder = false;
                    }

                }
                else if (type == SpriteManager.TILE_TYPE.SPRINT)
                {
                    if (dir.Y == 0)
                    {
                        
                        Tile tile = SpriteManager.GetTileAtPosition(newDestination);
                        m_destination = newDestination;
                        m_moving = true;
                        m_climbingLadder = false;
                        tile.g_update = true;
                        
                    }

                }
            }
        }
    }
}

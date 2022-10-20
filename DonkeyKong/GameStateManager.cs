using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class GameStateManager
    {
        private static readonly GameStateManager instance = new GameStateManager();
        public enum GAMESTATE { GAME, WIN, LOSE, MENU, NONE };
        public enum PLAYSTATE { WIN, LOSE, PUSHED, NONE};
        private PLAYSTATE m_playState = PLAYSTATE.NONE;
        private GAMESTATE m_state = GAMESTATE.MENU;
        public GAMESTATE GetCurrentGameState() { return m_state; }
        public void SetCurrentGameState(GAMESTATE state) { m_state = state; }
        public PLAYSTATE GetCurrentPlayState() { return m_playState; }
        public void SetCurrentPlayState(PLAYSTATE state) { m_playState = state; }
        static GameStateManager()
        {
        }
        public static GameStateManager Instance
        {
            get
            {
                return instance;
            }
        }
       
    }
}

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

        private GAMESTATE m_state = GAMESTATE.MENU;
        public GAMESTATE GetCurrentGameState() { return m_state; }
        public void SetCurrentGameState(GAMESTATE state) { m_state = state; }
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

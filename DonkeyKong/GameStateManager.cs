using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class GameStateManager
    {
        public enum GAMESTATE { GAME, WIN, LOSE, MENU, NONE };

        private GAMESTATE m_state = GAMESTATE.MENU;
        public GAMESTATE GetCurrentGameState() { return m_state; }

        public void Update()
        {
            switch (m_state)
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
                        break;
                    }
                    case GAMESTATE.NONE:
                    {
                        break;
                    }

            }
        }
    }
}

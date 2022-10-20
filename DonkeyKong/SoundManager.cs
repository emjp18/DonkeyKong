using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using static DonkeyKong.GameStateManager;
namespace DonkeyKong
{
    internal class SoundManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<SoundEffect> m_soundEffects;
        public enum SOUNDEFFECT { WIN, PUSHED, LOSE};
        protected override void LoadContent()
        {
            m_soundEffects = new List<SoundEffect>();
            m_soundEffects.Add(Game.Content.Load<SoundEffect>("winS"));
            m_soundEffects.Add(Game.Content.Load<SoundEffect>("pushedS"));
            m_soundEffects.Add(Game.Content.Load<SoundEffect>("loseS"));

        }
        public override void Update(GameTime gameTime)
        {
            switch(Instance.GetCurrentPlayState())
            {
                case PLAYSTATE.WIN:
                    {
                        m_soundEffects[(int)SOUNDEFFECT.WIN].Play();
                        Instance.SetCurrentPlayState(PLAYSTATE.NONE);
                        break;
                    }
                case PLAYSTATE.PUSHED:
                    {
                        m_soundEffects[(int)SOUNDEFFECT.PUSHED].Play();
                        Instance.SetCurrentPlayState(PLAYSTATE.NONE);
                        break;
                    }
                case PLAYSTATE.LOSE:
                    {
                        m_soundEffects[(int)SOUNDEFFECT.LOSE].Play();
                        Instance.SetCurrentPlayState(PLAYSTATE.NONE);
                        break;
                    }
            }
        }
        public SoundManager(Game game)
            : base(game)
        {

        }
    }
}

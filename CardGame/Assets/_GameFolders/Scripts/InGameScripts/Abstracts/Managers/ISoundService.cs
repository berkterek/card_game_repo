using CardGame.Enums;

namespace CardGame.Abstracts.Managers
{
    public interface ISoundService
    {
        void Play(SoundType soundType);
    }
}
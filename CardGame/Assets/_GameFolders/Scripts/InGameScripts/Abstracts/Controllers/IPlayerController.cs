using CardGame.Abstracts.Handlers;
using CardGame.Abstracts.Inputs;
using UnityEngine;

namespace CardGame.Abstracts.Controllers
{
    public interface IPlayerController
    {
        IWorldPositionHandler WorldPositionHandler { get; set; }
        IInputReader InputReader { get; set; }
        Camera Camera { get; }
        int CurrentScore { get; }
        event System.Action<int> OnSuccessMatching;
        void Update();
        void PlayerCanPlay();
        void PlayerCantPlay();
    }
}

using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Managers;

namespace CardGame.Controllers
{
    public class MatchUiController : BaseUiTextCounter
    {
        IPlayerController _playerController;
        IGameService _gameService;
        
        [Zenject.Inject]
        void Constructor(IPlayerController playerController, IGameService gameService)
        {
            _playerController = playerController;
            _gameService = gameService;
        }
        
        void OnEnable()
        {
            _playerController.OnSuccessMatching += HandleOnTextValueChanged;
            _gameService.OnGameStarted += HandleOnGameStarted;
        }

        void OnDisable()
        {
            _playerController.OnSuccessMatching -= HandleOnTextValueChanged;
            _gameService.OnGameStarted -= HandleOnGameStarted;
        }
        
        void HandleOnGameStarted()
        {
            _counterText.SetText("0");
        }
    }
}


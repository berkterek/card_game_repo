using CardGame.Abstracts.Managers;

namespace CardGame.Controllers
{
    public class PlayerPlayCountUiController : BaseUiTextCounter
    {
        ICardService _cardService;
        IGameService _gameService;

        [Zenject.Inject]
        void Constructor(ICardService cardService, IGameService gameService)
        {
            _cardService = cardService;
            _gameService = gameService;
        }
        
        void OnEnable()
        {
            _cardService.OnPlayerPlayCount += HandleOnTextValueChanged;
            _gameService.OnGameStarted += HandleOnGameStarted;
        }

        void OnDisable()
        {
            _cardService.OnPlayerPlayCount -= HandleOnTextValueChanged;
            _gameService.OnGameStarted -= HandleOnGameStarted;
        }
        
        void HandleOnGameStarted()
        {
            _counterText.SetText("0");
        }
    }
}
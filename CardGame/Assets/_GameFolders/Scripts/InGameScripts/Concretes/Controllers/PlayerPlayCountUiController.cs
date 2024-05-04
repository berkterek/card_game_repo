using CardGame.Abstracts.Managers;

namespace CardGame.Controllers
{
    public class PlayerPlayCountUiController : BaseUiTextCounter
    {
        protected ICardService _cardService;

        [Zenject.Inject]
        void Constructor(ICardService cardService)
        {
            _cardService = cardService;
        }
        
        void OnEnable()
        {
            _cardService.OnPlayerPlayCount += HandleOnTextValueChanged;
        }

        void OnDisable()
        {
            _cardService.OnPlayerPlayCount -= HandleOnTextValueChanged;
        }
    }
}
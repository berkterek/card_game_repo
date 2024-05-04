
using CardGame.Abstracts.Controllers;

namespace CardGame.Controllers
{
    public class MatchUiController : BaseUiTextCounter
    {
        IPlayerController _playerController;
        
        [Zenject.Inject]
        void Constructor(IPlayerController playerController)
        {
            _playerController = playerController;
        }
        
        void OnEnable()
        {
            _playerController.OnSuccessMatching += HandleOnTextValueChanged;
        }

        void OnDisable()
        {
            _playerController.OnSuccessMatching -= HandleOnTextValueChanged;
        }
    }
}


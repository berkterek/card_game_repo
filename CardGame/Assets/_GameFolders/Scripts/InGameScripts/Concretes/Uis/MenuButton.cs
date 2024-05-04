using CardGame.Abstracts.Managers;
using CardGame.Abstracts.Uis;

namespace CardGame.Uis
{
    public class MenuButton : BaseButton
    {
        IGameService _gameService;

        [Zenject.Inject]
        void Constructor(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        protected override void HandleOnButtonClicked()
        {
            _gameService.ReturnMenu();
        }
    }    
}


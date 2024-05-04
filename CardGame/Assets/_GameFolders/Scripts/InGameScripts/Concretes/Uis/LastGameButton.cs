using System;
using CardGame.Abstracts.Managers;
using CardGame.Abstracts.Uis;
using CardGame.Managers;

namespace CardGame.Uis
{
    public class LastGameButton : BaseButton
    {
        IGameService _gameService;
        ISaveLoadService _saveLoadService;
        
        [Zenject.Inject]
        void Constructor(IGameService gameService, ISaveLoadService saveLoadService)
        {
            _gameService = gameService;
            _saveLoadService = saveLoadService;

            _gameService.OnReturnMenu += HandleOnReturnMenu;
        }

        void Start()
        {
            HandleOnReturnMenu();
        }

        void HandleOnReturnMenu()
        {
            _button.interactable = _saveLoadService.HasKeyAvailable(CardManager.CARD_MANAGER_KEY);
        }

        protected override void HandleOnButtonClicked()
        {
            _gameService.LoadLastGame();
        }
    }
}
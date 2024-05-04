using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Managers;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Managers
{
    public class GameManager : MonoBehaviour,IGameService
    {
        const string SAVE_LOAD_KEY = "game_manager_save_load_key"; 
        
        [SerializeField] int _bestScore = 0;
        [SerializeField] int _currentScore;
        [SerializeField] int _delayMillisecond = 3000;
        
        IPlayerController _playerController;
        ICardService _cardService;
        ISaveLoadService _saveLoadService;

        public event System.Action<int,int> OnGameEnded;
        public event System.Action OnGameStarted;
        public event System.Action OnReturnMenu;

        [Zenject.Inject]
        void Constructor(IPlayerController playerController, ICardService cardService, ISaveLoadService saveLoadService)
        {
            _playerController = playerController;
            _cardService = cardService;
            _saveLoadService = saveLoadService;
            if (_saveLoadService.HasKeyAvailable(SAVE_LOAD_KEY))
            {
                var model = _saveLoadService.LoadDataProcess<GameManagerSaveModel>(SAVE_LOAD_KEY);
                _bestScore = model.BestScore;
            }
            
            _cardService.OnGameOvered += HandleOnGameOvered;
        }

        void OnDisable()
        {
            _cardService.OnGameOvered -= HandleOnGameOvered;
        }

        [Button]
        public async void GameStart()
        {
            OnGameStarted?.Invoke();
            _cardService.CreateCards();
            await UniTask.Delay(_delayMillisecond);
            await _cardService.FlipAllCard();
            _playerController.PlayerCanPlay();
        }
        
        [Button]
        public async void LoadLastGame()
        {
            OnGameStarted?.Invoke();
            _cardService.LoadLastGameCards();
            await UniTask.Delay(_delayMillisecond);
            await _cardService.FlipAllCard();
            _playerController.PlayerCanPlay();
        }

        public void ReturnMenu()
        {
            _playerController.PlayerCantPlay();
            OnReturnMenu?.Invoke();
        }
        
        void HandleOnGameOvered()
        {
            if (_bestScore < _playerController.CurrentScore)
            {
                _bestScore = _playerController.CurrentScore;
                _saveLoadService.SaveDataProcess(SAVE_LOAD_KEY, new GameManagerSaveModel()
                {
                    BestScore = _bestScore
                });
            }
            
            _currentScore = _playerController.CurrentScore;
            _playerController.PlayerCantPlay();
            
            OnGameEnded?.Invoke(_currentScore, _bestScore);
        }
    }

    public struct GameManagerSaveModel
    {
        public int BestScore;
    }
}
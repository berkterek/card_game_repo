using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Managers;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Managers
{
    public class GameManager : MonoBehaviour,IGameService
    {
        [SerializeField] int _bestScore = 0;
        [SerializeField] int _currentScore;
        [SerializeField] int _delayMillisecond = 3000;
        
        IPlayerController _playerController;
        ICardService _cardService;

        public event System.Action<int,int> OnGameEnded;

        [Zenject.Inject]
        void Constructor(IPlayerController playerController, ICardService cardService)
        {
            _playerController = playerController;
            _cardService = cardService;
            GameStart();
            _cardService.OnGameOvered += HandleOnGameOvered;
        }

        [Button]
        public async void GameStart()
        {
            await UniTask.Delay(_delayMillisecond);
            await _cardService.FlipAllCard();
            _playerController.PlayerCanPlay();
        }
        
        void HandleOnGameOvered()
        {
            if (_bestScore < _playerController.CurrentScore)
            {
                _bestScore = _playerController.CurrentScore;
                _currentScore = _playerController.CurrentScore;
            }
            
            OnGameEnded?.Invoke(_currentScore, _bestScore);
        }
    }
}
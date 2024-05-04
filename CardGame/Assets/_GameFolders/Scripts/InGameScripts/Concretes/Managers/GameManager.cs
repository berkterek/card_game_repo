using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Managers;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Managers
{
    public class GameManager : MonoBehaviour,IGameService
    {
        [SerializeField] int _delayMillisecond = 3000;
        
        IPlayerController _playerController;
        ICardService _cardService;

        [Zenject.Inject]
        void Constructor(IPlayerController playerController, ICardService cardService)
        {
            _playerController = playerController;
            _cardService = cardService;
            GameStart();
        }
        
        [Button]
        public async void GameStart()
        {
            await UniTask.Delay(_delayMillisecond);
            await _cardService.FlipAllCard();
            _playerController.PlayerCanPlay();
        }
    }
}
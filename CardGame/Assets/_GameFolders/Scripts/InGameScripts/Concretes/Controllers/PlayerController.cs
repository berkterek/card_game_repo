using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Handlers;
using CardGame.Abstracts.Inputs;
using CardGame.Abstracts.Managers;
using UnityEngine;

namespace CardGame.Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] int _totalScore;
        [SerializeField] bool _canPlay = false;

        ICardService _cardService;
        
        public IWorldPositionHandler WorldPositionHandler { get; set; }
        public IInputReader InputReader { get; set; }
        public Camera Camera { get; private set; }
        public int CurrentScore => _totalScore;
        public event System.Action<int> OnSuccessMatching;

        [Zenject.Inject]
        void Constructor(IInputReader inputReader, IWorldPositionHandler worldPositionHandler, ICardService cardService)
        {
            InputReader = inputReader;
            WorldPositionHandler = worldPositionHandler;
            _cardService = cardService;
            _cardService.OnSuccessMatching += HandleOnSuccessMatching;
        }

        void Awake()
        {
            Camera = Camera.main;
        }

        void OnDisable()
        {
            _cardService.OnSuccessMatching -= HandleOnSuccessMatching;
        }

        public void Update()
        {
            if (!InputReader.IsTouch || !_canPlay) return;

            var cardController = WorldPositionHandler.ExecuteGetWorldPosition();
            if (cardController == null) return;

            _cardService.MatchCards(cardController);
        }

        public void PlayerCanPlay()
        {
            _canPlay = true;
        }
        
        public void PlayerCantPlay()
        {
            _canPlay = false;
        }
        
        void HandleOnSuccessMatching(int score)
        {
            _totalScore += score;
            OnSuccessMatching?.Invoke(_totalScore);
        }
    }
}
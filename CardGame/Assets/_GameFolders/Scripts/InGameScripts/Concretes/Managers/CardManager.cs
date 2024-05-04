using System.Collections.Generic;
using System.Linq;
using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.DataContainers;
using CardGame.Abstracts.Managers;
using CardGame.Controllers;
using CardGame.Enums;
using CardGame.Helpers;
using CardGame.ScriptableObjects;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Managers
{
    public class CardManager : MonoBehaviour, ICardService
    {
        public const string CARD_MANAGER_KEY = "card_manager_key";

        [SerializeField] CardController _prefab;
        [SerializeField] DeckValue[] _decks;
        [SerializeField] int _xLoopCount = 4;
        [SerializeField] int _yLoopCount = 4;
        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;
        [SerializeField] int _playerPlayCount;
        [SerializeField] int _comboStart = 1;
        [SerializeField, ReadOnly] List<CardController> _cards;

        Queue<CardController> _firstCardControllers;
        int _currentCombo;
        ISoundService _soundService;
        IGameService _gameService;
        ISaveLoadService _saveLoadService;
        DeckName _tempDeck;

        public event System.Action<int> OnSuccessMatching;
        public event System.Action<int> OnPlayerPlayCount;
        public event System.Action OnGameOvered;

        [Zenject.Inject]
        void Constructor(ISoundService soundService, IGameService gameService, ISaveLoadService saveLoadService)
        {
            _soundService = soundService;
            _gameService = gameService;
            _saveLoadService = saveLoadService;
            _gameService.OnReturnMenu += HandleOnReturnMenu;
        }

        void Awake()
        {
            _firstCardControllers = new Queue<CardController>();
        }

        [Button]
        public void CreateCards()
        {
            _playerPlayCount = 0;
            _currentCombo = _comboStart;
            CleanCards();
            int maxCount = _xLoopCount * _yLoopCount;
            _cards = new List<CardController>(maxCount);

            int randomDeck = Random.Range(0, (int)DeckName.Deck3 + 1);
            _tempDeck = (DeckName)randomDeck;
            var deckValue = _decks.FirstOrDefault(x => x.DeckName == _tempDeck);
            if (deckValue == null)
            {
                deckValue = _decks.FirstOrDefault(x => x.DeckName == DeckName.Deck1);
                _tempDeck = DeckName.Deck1;
            }

            var dataContainers1 = deckValue.GetCardDataContainers(maxCount / 2);
            var dataContainers2 = dataContainers1.ToArray();

            List<ICardDataContainer> allDataContainers = new List<ICardDataContainer>();
            AddList(dataContainers1, allDataContainers);
            AddList(dataContainers2, allDataContainers);

            allDataContainers.Shuffle();

            int index = 0;
            for (int i = 0; i < _xLoopCount; i++)
            {
                for (int j = 0; j < _yLoopCount; j++)
                {
                    var cardController = Instantiate(_prefab, transform);
                    cardController.Transform.localPosition = new Vector3(i * _xOffset, j * _yOffset, 0);
                    cardController.SetDataContainer(allDataContainers[index]);
                    _cards.Add(cardController);
                    index++;
                }
            }
        }

        public void LoadLastGameCards()
        {
            if (!_saveLoadService.HasKeyAvailable(CARD_MANAGER_KEY)) return;

            var model = _saveLoadService.LoadDataProcess<DeckDataModel>(CARD_MANAGER_KEY);

            _playerPlayCount = model.PlayerPlayCount;
            OnPlayerPlayCount?.Invoke(_playerPlayCount);
            _currentCombo = model.CurrentCombo;

            CleanCards();

            int maxCount = _xLoopCount * _yLoopCount;
            _cards = new List<CardController>(maxCount);


            _tempDeck = model.DeckName;
            var deckValue = _decks.FirstOrDefault(x => x.DeckName == _tempDeck);

            List<ICardDataContainer> loadedList = new List<ICardDataContainer>();

            for (int i = 0; i < model.CardDataModels.Count; i++)
            {
                loadedList.Add(deckValue.GetDataContainerByType(model.CardDataModels[i].CardType));
            }

            for (int i = 0; i < loadedList.Count; i++)
            {
                var cardController = Instantiate(_prefab, transform);
                cardController.Transform.localPosition = new Vector3(model.CardDataModels[i].XPosition,
                    model.CardDataModels[i].YPosition, 0f);
                cardController.SetDataContainer(loadedList[i]);
                _cards.Add(cardController);
            }
        }

        [Button]
        void CleanCards()
        {
            _cards = null;
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        void AddList(ICardDataContainer[] dataContainers, List<ICardDataContainer> list)
        {
            foreach (var cardDataContainer in dataContainers)
            {
                list.Add(cardDataContainer);
            }
        }

        public async UniTask FlipAllCard()
        {
            foreach (var cardController in _cards)
            {
                cardController.RotateCard();
                await UniTask.Delay(100);
            }
        }

        public async void MatchCards(ICardController cardController)
        {
            if (_firstCardControllers.Count <= 0)
            {
                _firstCardControllers.Enqueue(cardController as CardController);
                cardController.RotateCard();
                _soundService.Play(SoundType.Flip);
            }
            else
            {
                var secondCardController = cardController as CardController;
                secondCardController.RotateCard();
                _soundService.Play(SoundType.Flip);
                var firstCard = _firstCardControllers.Dequeue();

                if (firstCard.CardDataContainer.CardType == secondCardController.CardDataContainer.CardType)
                {
                    _cards.Remove(firstCard);
                    _cards.Remove(secondCardController);
                    await UniTask.Delay(1000);
                    OnSuccessMatching?.Invoke(firstCard.CardDataContainer.CardScore * _currentCombo);
                    Destroy(firstCard.gameObject);
                    Destroy(secondCardController.gameObject);
                    _currentCombo++;
                    _soundService.Play(SoundType.Success);
                }
                else
                {
                    await UniTask.Delay(1000);
                    firstCard.RotateCard();
                    secondCardController.RotateCard();
                    _currentCombo = _comboStart;
                    _soundService.Play(SoundType.Failed);
                }

                _playerPlayCount++;
                OnPlayerPlayCount?.Invoke(_playerPlayCount);

                if (_cards.Count <= 0)
                {
                    if (_saveLoadService.HasKeyAvailable(CARD_MANAGER_KEY))
                    {
                        _saveLoadService.DeleteData(CARD_MANAGER_KEY);
                    }

                    _soundService.Play(SoundType.Finished);
                    OnGameOvered?.Invoke();
                }
            }
        }

        void HandleOnReturnMenu()
        {
            if (_cards.Count > 0)
            {
                DeckDataModel model = new DeckDataModel();
                model.DeckName = _tempDeck;
                model.CurrentCombo = _currentCombo;
                model.PlayerPlayCount = _playerPlayCount;
                model.CardDataModels = new List<CardDataModel>();
                foreach (CardController cardController in _cards)
                {
                    model.CardDataModels.Add(new CardDataModel()
                    {
                        CardType = cardController.CardDataContainer.CardType,
                        XPosition = cardController.Transform.localPosition.x,
                        YPosition = cardController.Transform.localPosition.y
                    });
                }

                _saveLoadService.SaveDataProcess(CARD_MANAGER_KEY, model);
            }
        }
    }

    [System.Serializable]
    public class DeckValue
    {
        public DeckName DeckName;
        [SerializeField] CardDataContainerSO[] _cardDataContainers;

        public ICardDataContainer[] CardDataContainers { get; set; }

        public ICardDataContainer[] GetCardDataContainers(int count)
        {
            if (CardDataContainers == null) CardDataContainers = _cardDataContainers;

            if (CardDataContainers.Length < count) count = Mathf.Max(CardDataContainers.Length - 1, 0);

            List<ICardDataContainer> tempList = new List<ICardDataContainer>(count);

            while (tempList.Count < count)
            {
                var randomIndex = Random.Range(0, count);
                var cardData = CardDataContainers[randomIndex];

                while (tempList.Contains(cardData))
                {
                    randomIndex = Random.Range(0, count);
                    cardData = CardDataContainers[randomIndex];
                }

                tempList.Add(cardData);
            }

            return tempList.ToArray();
        }

        public ICardDataContainer GetDataContainerByType(CardType cardType)
        {
            return _cardDataContainers.FirstOrDefault(x => x.CardType == cardType);
        }
    }

    public class DeckDataModel
    {
        public DeckName DeckName;
        public int PlayerPlayCount;
        public int CurrentCombo;
        public List<CardDataModel> CardDataModels;
    }

    public class CardDataModel
    {
        public float XPosition;
        public float YPosition;
        public CardType CardType;
    }
}
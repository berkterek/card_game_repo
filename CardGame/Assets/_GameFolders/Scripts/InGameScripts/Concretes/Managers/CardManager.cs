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
        [SerializeField] CardController _prefab;
        [SerializeField] DeckValue[] _decks;
        [SerializeField] int _xLoopCount = 4;
        [SerializeField] int _yLoopCount = 4;
        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;
        [SerializeField] int _playerPlayCount;
        [SerializeField, ReadOnly] List<CardController> _cards;

        public event System.Action<int> OnSuccessMatching;
        public event System.Action<int> OnPlayerPlayCount;
        public event System.Action OnGameOvered;
 
        Queue<CardController> _firstCardControllers;

        void Awake()
        {
            CreateCards();
            _firstCardControllers = new Queue<CardController>();
        }

        void Start()
        {
            _playerPlayCount = 0;
        }

        [Button]
        public void CreateCards()
        {
            CleanCards();
            int maxCount = _xLoopCount * _yLoopCount;
            _cards = new List<CardController>(maxCount);

            int randomDeck = Random.Range(0, (int)DeckName.Deck3 + 1);
            var deckValue = _decks.FirstOrDefault(x => (int)x.DeckName == randomDeck);
            if (deckValue == null)
            {
                deckValue = _decks.FirstOrDefault(x => x.DeckName == DeckName.Deck1);
            }

            var dataContainers1 = deckValue.GetCardDataContainers(maxCount / 2);
            var dataContainers2 = dataContainers1.ToArray();

            List<ICardDataContainer> allDataContainer = new List<ICardDataContainer>();
            AddList(dataContainers1, allDataContainer);
            AddList(dataContainers2, allDataContainer);
            
            allDataContainer.Shuffle();

            int index = 0;
            for (int i = 0; i < _xLoopCount; i++)
            {
                for (int j = 0; j < _yLoopCount; j++)
                {
                    var cardController = Instantiate(_prefab, transform);
                    cardController.Transform.localPosition = new Vector3(i * _xOffset, j * _yOffset, 0);
                    cardController.SetDataContainer(allDataContainer[index]);
                    _cards.Add(cardController);
                    index++;
                }
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
            }
            else
            {
                var secondCardController = cardController as CardController;
                secondCardController.RotateCard();
                var firstCard = _firstCardControllers.Dequeue();

                if (firstCard.CardDataContainer.CardType == secondCardController.CardDataContainer.CardType)
                {
                    _cards.Remove(firstCard);
                    _cards.Remove(secondCardController);
                    await UniTask.Delay(1000);
                    OnSuccessMatching?.Invoke(firstCard.CardDataContainer.CardScore);
                    Destroy(firstCard.gameObject);
                    Destroy(secondCardController.gameObject);
                    
                }
                else
                {
                    await UniTask.Delay(1000);
                    firstCard.RotateCard();
                    secondCardController.RotateCard();
                }

                _playerPlayCount++;
                OnPlayerPlayCount?.Invoke(_playerPlayCount);
                
                if(_cards.Count <= 0) OnGameOvered?.Invoke();
            }
        }
    }

    public struct CardHolder
    {
        public CardController CardController1;
        public CardController CardController2;
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
    }
}
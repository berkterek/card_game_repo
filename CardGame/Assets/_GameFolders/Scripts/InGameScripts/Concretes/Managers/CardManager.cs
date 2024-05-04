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
        [SerializeField, ReadOnly] List<CardController> _cards;

        CardController _firstCardController;
        CardController _secondCardController;

        void Awake()
        {
            CreateCards();
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
            if (_firstCardController == null)
            {
                _firstCardController = cardController as CardController;
                _firstCardController.RotateCard();
            }
            else
            {
                _secondCardController = cardController as CardController;
                _secondCardController.RotateCard();
                await UniTask.Delay(1000);

                if (_firstCardController.CardDataContainer.CardType == _secondCardController.CardDataContainer.CardType)
                {
                    _cards.Remove(_firstCardController);
                    _cards.Remove(_secondCardController);
                    Destroy(_firstCardController.gameObject);
                    Destroy(_secondCardController.gameObject);
                }
                else
                {
                    _firstCardController.RotateCard();
                    _secondCardController.RotateCard();
                }

                _firstCardController = null;
                _secondCardController = null;
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
    }
}
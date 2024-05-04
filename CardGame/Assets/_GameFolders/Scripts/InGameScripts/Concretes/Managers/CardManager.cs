using System.Collections.Generic;
using CardGame.Abstracts.DataContainers;
using CardGame.Controllers;
using CardGame.ScriptableObjects;
using UnityEngine;

namespace CardGame.Managers
{
    public class CardManager : MonoBehaviour,ICardService
    {
        [SerializeField] CardController _prefab;
        [SerializeField] DeckValue[] _decks;
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
            
            if (CardDataContainers.Length < count) count = Mathf.Max(CardDataContainers.Length - 1,0);

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

    public enum DeckName : byte
    {
        Deck1,
        Deck2,
        Deck3
    }

    public interface ICardService
    {
        
    }
}
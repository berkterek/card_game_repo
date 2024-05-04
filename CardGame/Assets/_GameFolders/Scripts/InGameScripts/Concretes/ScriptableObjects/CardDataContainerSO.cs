using CardGame.Abstracts.DataContainers;
using CardGame.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Card Data Container", menuName = "Terek Gaming/Data Container/Card Data Container")]
    public class CardDataContainerSO : ScriptableObject,ICardDataContainer
    {
        
        [Header("Basic Info")]
        [BoxGroup("Card Details"), LabelText("Card Type")]
        [SerializeField] private CardType _cardType;

        [BoxGroup("Card Details"), LabelText("Card Score")]
        [SerializeField] private int _cardScore;
        
        [BoxGroup("Card Details"), LabelText("Card Sprite"), PreviewField(75, ObjectFieldAlignment.Left)]
        [SerializeField] private Sprite _cardSprite;

        public CardType CardType => _cardType;
        public Sprite CardSprite => _cardSprite;
        public int CardScore => _cardScore;

        public void SetEditorBuild(Sprite sprite, CardType cardType, int score = 1)
        {
            _cardSprite = sprite;
            _cardType = cardType;
            _cardScore = score;
        }
    }
}


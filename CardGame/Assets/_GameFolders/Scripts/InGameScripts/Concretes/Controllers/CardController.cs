using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.DataContainers;
using CardGame.Helpers;
using UnityEngine;

namespace CardGame.Controllers
{
    public class CardController : MonoBehaviour, ICardController
    {
        [SerializeField] SpriteRenderer _bodySpriteRenderer;

        public ICardDataContainer CardDataContainer { get; private set; }

        void OnValidate()
        {
            this.GetReference<SpriteRenderer>(ref _bodySpriteRenderer);
        }

        public void SetDataContainer(ICardDataContainer cardDataContainer)
        {
            CardDataContainer = cardDataContainer;
            _bodySpriteRenderer.sprite = cardDataContainer.CardSprite;
        }
    }
}
using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.DataContainers;
using CardGame.Helpers;
using UnityEngine;

namespace CardGame.Controllers
{
    public class CardController : MonoBehaviour, ICardController
    {
        [SerializeField] SpriteRenderer _bodySpriteRenderer;
        [SerializeField] Transform _transform;

        public ICardDataContainer CardDataContainer { get; private set; }
        public Transform Transform => _transform;

        void OnValidate()
        {
            this.GetReference<SpriteRenderer>(ref _bodySpriteRenderer);
            this.GetReference<Transform>(ref _transform);
        }

        public void SetDataContainer(ICardDataContainer cardDataContainer)
        {
            CardDataContainer = cardDataContainer;
            _bodySpriteRenderer.sprite = cardDataContainer.CardSprite;
        }
    }
}
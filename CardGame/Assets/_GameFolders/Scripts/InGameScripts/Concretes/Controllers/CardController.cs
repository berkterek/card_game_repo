using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.DataContainers;
using CardGame.Helpers;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Controllers
{
    public class CardController : MonoBehaviour, ICardController
    {
        [SerializeField] SpriteRenderer _bodySpriteRenderer;
        [SerializeField] Transform _transform;
        [SerializeField] bool _isFront = true;

        public ICardDataContainer CardDataContainer { get; private set; }
        public Transform Transform => _transform;

        void OnValidate()
        {
            this.GetReference<Transform>(ref _transform);
        }

        public void SetDataContainer(ICardDataContainer cardDataContainer)
        {
            CardDataContainer = cardDataContainer;
            _bodySpriteRenderer.sprite = cardDataContainer.CardSprite;
        }

        [Button]
        private void RotateCard()
        {
            _isFront = !_isFront;

            if (_isFront)
            {
                _transform.DORotate(new Vector3(0f, 90f, 0f), 0.25f).onComplete += () =>
                {
                    _bodySpriteRenderer.sprite = CardDataContainer.CardSprite;
                    _transform.DORotate(new Vector3(0f, 180f, 0f), 0.25f);
                };
            }
            else
            {
                _transform.DORotate(new Vector3(0f, 90f, 0f), 0.25f).onComplete += () =>
                {
                    _bodySpriteRenderer.sprite = null;
                    _transform.DORotate(new Vector3(0f, 0f, 0f), 0.25f);
                };
            }
        } 
    }
}
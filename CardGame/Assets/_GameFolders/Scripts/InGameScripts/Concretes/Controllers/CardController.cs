using CardGame.Helpers;
using UnityEngine;

namespace CardGame.Controllers
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _bodySpriteRenderer;

        void OnValidate()
        {
            this.GetReference<SpriteRenderer>(ref _bodySpriteRenderer);
        }
    }
}
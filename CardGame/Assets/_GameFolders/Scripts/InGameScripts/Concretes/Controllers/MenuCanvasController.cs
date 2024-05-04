using CardGame.Abstracts.Managers;
using CardGame.Helpers;
using UnityEngine;

namespace CardGame.Controllers
{
    public class MenuCanvasController : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;

        IGameService _gameService;

        [Zenject.Inject]
        void Constructor(IGameService gameService)
        {
            _gameService = gameService;
            _gameService.OnReturnMenu += HandleOnReturnMenu;
        }

        void OnValidate() => this.GetReference(ref _canvasGroup);

        void ActiveCanvasGroup() => SetCanvasValues(1f, true);

        public void DeactivateCanvasGroup() => SetCanvasValues(0f, false);

        void SetCanvasValues(float alpha, bool value)
        {
            _canvasGroup.alpha = alpha;
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }

        void HandleOnReturnMenu() => ActiveCanvasGroup();
    }
}
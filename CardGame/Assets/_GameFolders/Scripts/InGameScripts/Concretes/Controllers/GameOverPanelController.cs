using CardGame.Abstracts.Managers;
using CardGame.Helpers;
using TMPro;
using UnityEngine;

namespace CardGame.Controllers
{
    public class GameOverPanelController : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] TMP_Text _bestScoreText;
        [SerializeField] TMP_Text _currentScoreText;

        IGameService _gameService;

        void OnValidate()
        {
            this.GetReference(ref _canvasGroup);
        }

        [Zenject.Inject]
        void Constructor(IGameService gameService)
        {
            _gameService = gameService;
            _gameService.OnGameEnded += HandleOnGameEnded;
        }

        void OnDisable()
        {
            _gameService.OnGameEnded -= HandleOnGameEnded;
        }

        void HandleOnGameEnded(int currentScore, int bestScore)
        {
            _currentScoreText.SetText(currentScore.ToString());
            _bestScoreText.SetText(bestScore.ToString());
            SetCanvasGroupValues(1f, true);
        }

        void SetCanvasGroupValues(float alpha, bool value)
        {
            _canvasGroup.blocksRaycasts = value;
            _canvasGroup.interactable = value;
            _canvasGroup.alpha = alpha;
        }
    }
}
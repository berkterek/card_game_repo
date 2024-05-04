using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Inputs;
using UnityEngine;

namespace CardGame.Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public IInputReader InputReader { get; private set; }

        [Zenject.Inject]
        private void Constructor(IInputReader inputReader)
        {
            InputReader = inputReader;
        }
    }
}
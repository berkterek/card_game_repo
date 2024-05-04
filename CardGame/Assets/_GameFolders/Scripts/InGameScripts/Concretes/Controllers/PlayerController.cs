using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Handlers;
using CardGame.Abstracts.Inputs;
using UnityEngine;

namespace CardGame.Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public IWorldPositionHandler WorldPositionHandler { get; set; }
        public IInputReader InputReader { get; set; }
        public Camera Camera { get; private set; }

        [Zenject.Inject]
        void Constructor(IInputReader inputReader, IWorldPositionHandler worldPositionHandler)
        {
            InputReader = inputReader;
            WorldPositionHandler = worldPositionHandler;
        }

        void Awake()
        {
            Camera = Camera.main;
        }

        public void Update()
        {
            if (!InputReader.IsTouch) return;

            WorldPositionHandler.ExecuteGetWorldPosition();
        }
    }
}
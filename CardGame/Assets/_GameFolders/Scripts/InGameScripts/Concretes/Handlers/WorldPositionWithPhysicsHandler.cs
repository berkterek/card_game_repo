using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Handlers;
using UnityEngine;

namespace CardGame.Handlers
{
    public class WorldPositionWithPhysicsHandler : IWorldPositionHandler
    {
        readonly IPlayerController _playerController;

        [Zenject.Inject]
        public WorldPositionWithPhysicsHandler(IPlayerController playerController)
        {
            _playerController = playerController;
        }
        
        public void ExecuteGetWorldPosition()
        {
            Vector3 worldPosition = _playerController.Camera.ScreenToWorldPoint(_playerController.InputReader.TouchPosition);
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);
            Debug.Log(worldPosition);
        }
    }
}
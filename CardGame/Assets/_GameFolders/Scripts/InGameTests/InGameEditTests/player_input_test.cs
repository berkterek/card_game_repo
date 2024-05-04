using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Handlers;
using CardGame.Abstracts.Inputs;
using CardGame.Controllers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Player_Edit_Test
{
    public class player_input_test
    {
        [Test]
        public void player_input_is_touch_false()
        {
            //Setup
            GameObject playerObject = new GameObject();
            IPlayerController playerController = playerObject.AddComponent<PlayerController>();
            playerController.InputReader = Substitute.For<IInputReader>();
            playerController.WorldPositionHandler = Substitute.For<IWorldPositionHandler>();
        
            //Action
            playerController.InputReader.IsTouch.Returns(false);
            playerController.Update();

            //Result
            playerController.WorldPositionHandler.DidNotReceive().ExecuteGetWorldPosition();
        }
    
        [Test]
        public void player_input_is_touch_true()
        {
            //Setup
            GameObject playerObject = new GameObject();
            IPlayerController playerController = playerObject.AddComponent<PlayerController>();
            playerController.InputReader = Substitute.For<IInputReader>();
            playerController.WorldPositionHandler = Substitute.For<IWorldPositionHandler>();
        
            //Action
            playerController.InputReader.IsTouch.Returns(true);
            playerController.Update();

            //Result
            playerController.WorldPositionHandler.Received().ExecuteGetWorldPosition();
        }
    }
}
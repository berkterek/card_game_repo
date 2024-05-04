using CardGame.Abstracts.Controllers;
using CardGame.Abstracts.Handlers;
using CardGame.Abstracts.Inputs;
using CardGame.Controllers;
using CardGame.Handlers;
using CardGame.Inputs;
using Zenject;

namespace CardGame.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputReader>().To<NewInputReader>().AsSingle().NonLazy();
            Container.Bind<IPlayerController>().To<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<IWorldPositionHandler>().To<WorldPositionWithPhysicsHandler>().AsSingle().NonLazy();
        }
    }    
}
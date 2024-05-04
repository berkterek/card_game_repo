using CardGame.Abstracts.Managers;
using CardGame.Managers;
using Zenject;

namespace CardGame.Installers
{
    public class ManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICardService>().To<CardManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<IGameService>().To<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
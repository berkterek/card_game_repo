using CardGame.Abstracts.DataAccessLayers;
using CardGame.Abstracts.Managers;
using CardGame.DataAccessLayers;
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
            Container.Bind<ISoundService>().To<SoundManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ISaveLoadService>().To<SaveLoadManager>().AsSingle().NonLazy();
            Container.Bind<IDataSaveLoadDal>().To<PlayerPrefsDataSaveLoadDal>().AsSingle().NonLazy();
        }
    }
}
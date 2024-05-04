using CardGame.Abstracts.Inputs;
using CardGame.Inputs;
using Zenject;

namespace CardGame.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputReader>().To<NewInputReader>().AsSingle().NonLazy();
        }
    }    
}
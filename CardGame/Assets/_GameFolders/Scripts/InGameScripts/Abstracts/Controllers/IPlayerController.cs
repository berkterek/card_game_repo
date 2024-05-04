using CardGame.Abstracts.Inputs;

namespace CardGame.Abstracts.Controllers
{
    public interface IPlayerController
    {
        IInputReader InputReader { get; }
    }
}
namespace CardGame.Abstracts.Managers
{
    public interface IGameService
    {
        event System.Action<int,int> OnGameEnded;
    }
}
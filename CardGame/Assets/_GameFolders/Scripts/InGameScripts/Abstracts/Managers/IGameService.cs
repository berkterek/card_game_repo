namespace CardGame.Abstracts.Managers
{
    public interface IGameService
    {
        event System.Action<int,int> OnGameEnded;
        event System.Action OnGameStarted;
        event System.Action OnReturnMenu;
        void GameStart();
        void ReturnMenu();
        void LoadLastGame();
    }
}
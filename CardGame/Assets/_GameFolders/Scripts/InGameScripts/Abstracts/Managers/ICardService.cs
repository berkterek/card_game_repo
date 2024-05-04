using CardGame.Abstracts.Controllers;
using Cysharp.Threading.Tasks;

namespace CardGame.Abstracts.Managers
{
    public interface ICardService
    {
        UniTask FlipAllCard();
        void MatchCards(ICardController cardController);
    }
}
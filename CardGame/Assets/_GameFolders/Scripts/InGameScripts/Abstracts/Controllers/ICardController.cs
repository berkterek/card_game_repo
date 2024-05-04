using CardGame.Abstracts.DataContainers;

namespace CardGame.Abstracts.Controllers
{
    public interface ICardController
    {
        ICardDataContainer CardDataContainer { get; }
        bool IsFront { get; }
        void SetDataContainer(ICardDataContainer cardDataContainer);
        void RotateCard();
    }
}
using CardGame.Abstracts.DataContainers;

namespace CardGame.Abstracts.Controllers
{
    public interface ICardController
    {
        ICardDataContainer CardDataContainer { get; }
        void SetDataContainer(ICardDataContainer cardDataContainer);
    }
}
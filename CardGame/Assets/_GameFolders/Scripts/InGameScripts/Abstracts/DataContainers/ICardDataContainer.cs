using CardGame.Enums;
using UnityEngine;

namespace CardGame.Abstracts.DataContainers
{
    public interface ICardDataContainer
    {
        CardType CardType { get; }
        Sprite CardSprite { get; }
        int CardScore { get; }
    }
}
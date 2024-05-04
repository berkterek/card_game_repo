using CardGame.Abstracts.Stats;
using UnityEngine;

namespace CardGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Card Stats", menuName = "Terek Gaming/Stats/Card Stats")]
    public class CardStatsSO : ScriptableObject, ICardStats
    {
        [SerializeField] float _rotationDuration = 0.25f;

        public float RotationDuration => _rotationDuration;
    }
}
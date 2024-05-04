using UnityEngine;

namespace CardGame.Helpers
{
    public static class MonoExtensions
    {
        public static void GetReference<T>(this Component component,ref T value) where T : Component
        {
            if (value == null)
            {
                value = component.GetComponentInChildren<T>();    
            }
        }
    }
}
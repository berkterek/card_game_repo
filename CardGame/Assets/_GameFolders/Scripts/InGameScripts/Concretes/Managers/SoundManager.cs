using System.Collections.Generic;
using CardGame.Abstracts.Managers;
using CardGame.Controllers;
using CardGame.Enums;
using UnityEngine;

namespace CardGame.Managers
{
    public class SoundManager : MonoBehaviour,ISoundService
    {
        [SerializeField] SoundInspectorHolder[] _sounds;

        Dictionary<SoundType, SoundController> _soundDictionary;

        void Awake()
        {
            _soundDictionary = new Dictionary<SoundType, SoundController>();
            foreach (var soundInspectorHolder in _sounds)
            {
                _soundDictionary.Add(soundInspectorHolder.SoundType,soundInspectorHolder.SoundController);
            }
        }

        public void Play(SoundType soundType)
        {
            if (!_soundDictionary.ContainsKey(soundType)) return;
            
            _soundDictionary[soundType].Play();
        }
    }

    [System.Serializable]
    public class SoundInspectorHolder
    {
        public SoundType SoundType;
        public SoundController SoundController;
    }
}


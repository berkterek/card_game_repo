using CardGame.Abstracts.Controllers;
using CardGame.Helpers;
using UnityEngine;

namespace CardGame.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour,ISoundController
    {
        [SerializeField] AudioSource _audioSource;

        void OnValidate()
        {
            this.GetReference(ref _audioSource);
        }

        public void Play()
        {
            _audioSource.Play();
        }
    }
}
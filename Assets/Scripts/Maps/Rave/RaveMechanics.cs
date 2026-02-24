using UnityEngine;
using UnityEngine.Rendering;

namespace NeonProtocol.Maps.Rave
{
    public class RaveMechanics : MonoBehaviour
    {
        [Header("Rave Vision")]
        [SerializeField] private Volume globalVolume;
        [SerializeField] private VolumeProfile normalProfile;
        [SerializeField] private VolumeProfile raveProfile;
        
        private bool _isRaveMode = false;
        private float _transitionSpeed = 2f;

        public void ToggleRaveMode()
        {
            _isRaveMode = !_isRaveMode;
            StopAllCoroutines();
            StartCoroutine(TransitionVision());
        }

        private IEnumerator TransitionVision()
        {
            float targetWeight = _isRaveMode ? 1f : 0f;
            float startWeight = globalVolume.weight;
            float time = 0f;

            globalVolume.profile = raveProfile; // Swap logic might vary based on setup

            while (time < 1f)
            {
                time += Time.deltaTime * _transitionSpeed;
                globalVolume.weight = Mathf.Lerp(startWeight, targetWeight, time);
                yield return null;
            }
        }

        // Slasher Boss Logic placeholder
        public void SpawnSlasher()
        {
            // Spawn logic for Slasher who only takes damage in Rave Mode
            // Logic: if (!_isRaveMode) slasher.invulnerable = true;
        }
    }
}
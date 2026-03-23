using UnityEngine;
using NeonProtocol.Core.Combat;

namespace NeonProtocol.Maps.Spaceland.Upgrades
{
    public enum ElementType { Fire, Wind, Lightning, Poison }

    public class ArcaneCore : MonoBehaviour
    {
        [SerializeField] private ElementType currentElement;
        [SerializeField] private ParticleSystem elementalEffect;
        [SerializeField] private int soulsRequired = 20;

        private int _currentSouls;
        private bool _isCharged;

        public void ApplyToWeapon(NeonWeapon weapon)
        {
            if (!_isCharged)
            {
                Debug.Log($"Core not charged yet. Need {soulsRequired - _currentSouls} more souls.");
                return;
            }

            Debug.Log($"Applied {currentElement} to weapon.");
            if (elementalEffect) elementalEffect.Play();
            _isCharged = false;
            _currentSouls = 0;
        }

        public void AddSoul()
        {
            if (_isCharged) return;

            _currentSouls++;
            if (_currentSouls >= soulsRequired)
            {
                _isCharged = true;
                Debug.Log($"{currentElement} core fully charged! Ready for pickup.");
            }
        }

        public bool IsCharged => _isCharged;
        public int SoulsRemaining => Mathf.Max(0, soulsRequired - _currentSouls);
    }
}
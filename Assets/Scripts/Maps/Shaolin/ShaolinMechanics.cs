using UnityEngine;
using System.Collections.Generic;

namespace NeonProtocol.Maps.Shaolin
{
    public enum ChiStyle { None, Dragon, Tiger, Snake, Crane }

    public class ShaolinMechanics : MonoBehaviour
    {
        public static ShaolinMechanics Instance;
        
        [Header("Chi System")]
        [SerializeField] private float chiRegenRate = 5f;
        [SerializeField] private float maxChi = 100f;
        
        private ChiStyle _currentStyle = ChiStyle.None;
        private float _currentChi;

        private void Awake() => Instance = this;

        public void ActivateStyle(ChiStyle style)
        {
            _currentStyle = style;
            _currentChi = maxChi;
            // Provide UI Feedback
            // Change Player Hands/Material
        }

        public void PerformChiAttack()
        {
            if (_currentChi < 10) return;

            switch (_currentStyle)
            {
                case ChiStyle.Dragon:
                    // Fire AOE Logic
                    SpawnFireBlast();
                    break;
                case ChiStyle.Tiger:
                    // Single target heavy melee
                    break;
                case ChiStyle.Snake:
                    // Poison DoT or Life Steal
                    break;
                case ChiStyle.Crane:
                    // Push back / Wind
                    break;
            }
            _currentChi -= 10f;
        }

        private void SpawnFireBlast()
        {
            // Use NeonPooler
            // NeonPooler.Instance.Spawn("DragonFire", transform.position, transform.rotation);
        }

        private void Update()
        {
            if (_currentChi < maxChi)
                _currentChi += chiRegenRate * Time.deltaTime;
        }
    }
}
using UnityEngine;
using NeonProtocol.Core.Movement;

namespace NeonProtocol.Maps.Beast.Environment
{
    public class LowGravityZone : MonoBehaviour
    {
        [SerializeField] private float gravityMultiplier = 0.3f;
        [SerializeField] private float jumpBoost = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && NeonMovement.Instance != null)
            {
                NeonMovement.Instance.SetGravityMultiplier(gravityMultiplier);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && NeonMovement.Instance != null)
            {
                NeonMovement.Instance.SetGravityMultiplier(1f);
            }
        }
    }
}
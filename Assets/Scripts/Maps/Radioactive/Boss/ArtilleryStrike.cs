using UnityEngine;

namespace NeonProtocol.Maps.Radioactive.Boss
{
    public class ArtilleryStrike : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private float strikeDelay = 2f;
        [SerializeField] private float blastRadius = 8f;
        [SerializeField] private float damage = 500f;
        [SerializeField] private LayerMask damageLayers;
        
        public void CallStrike(Vector3 targetPosition)
        {
            Debug.Log("Artillery inbound!");
            StartCoroutine(ExecuteStrike(targetPosition));
        }

        private System.Collections.IEnumerator ExecuteStrike(Vector3 pos)
        {
            yield return new WaitForSeconds(strikeDelay);
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, pos, Quaternion.identity);

            Collider[] hits = Physics.OverlapSphere(pos, blastRadius, damageLayers);
            foreach (Collider col in hits)
            {
                if (col.TryGetComponent(out NeonProtocol.Core.AI.ZombieController zombie))
                {
                    zombie.TakeDamage(damage);
                }
            }
        }
    }
}
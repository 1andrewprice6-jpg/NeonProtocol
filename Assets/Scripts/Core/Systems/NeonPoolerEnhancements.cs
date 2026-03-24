using System.Collections.Generic;
using UnityEngine;

namespace NeonProtocol.Core
{
    /// <summary>
    /// Extends the NeonPooler system with specialized particle effect pooling and pre-warming.
    /// Provides efficient reuse of particle effects for muzzle flashes, explosions, and impacts.
    /// </summary>
    public class NeonPoolerEnhancements : MonoBehaviour
    {
        private static Dictionary<string, Queue<GameObject>> _particleEffectPools =
            new Dictionary<string, Queue<GameObject>>();

        [Header("Pool Pre-warming")]
        [SerializeField] private bool autoPrewarm = true;

        /// <summary>
        /// Pre-warms a particle effect pool by instantiating multiple copies.
        /// </summary>
        /// <param name="key">The identifier key for this pool.</param>
        /// <param name="prefab">The particle effect prefab to pool.</param>
        /// <param name="count">The number of instances to pre-create.</param>
        public void PrewarmParticlePool(string key, GameObject prefab, int count)
        {
            if (prefab == null)
            {
                Debug.LogError($"[NeonPoolerEnhancements] Cannot prewarm pool '{key}': prefab is null.");
                return;
            }

            if (!_particleEffectPools.ContainsKey(key))
            {
                _particleEffectPools[key] = new Queue<GameObject>();
            }

            Queue<GameObject> pool = _particleEffectPools[key];

            for (int i = 0; i < count; i++)
            {
                GameObject instance = Instantiate(prefab, transform);
                instance.name = $"{prefab.name}_Pooled_{i}";
                instance.SetActive(false);
                pool.Enqueue(instance);
            }

            Debug.Log($"[NeonPoolerEnhancements] Pre-warmed pool '{key}' with {count} instances.");
        }

        /// <summary>
        /// Retrieves a particle effect from the pool, or creates a new one if pool is empty.
        /// </summary>
        /// <param name="key">The identifier key for the particle pool.</param>
        /// <returns>A GameObject with a ParticleSystem, or null if the pool doesn't exist.</returns>
        public GameObject GetParticleEffect(string key)
        {
            if (!_particleEffectPools.ContainsKey(key))
            {
                Debug.LogWarning($"[NeonPoolerEnhancements] Pool '{key}' does not exist.");
                return null;
            }

            Queue<GameObject> pool = _particleEffectPools[key];

            if (pool.Count > 0)
            {
                GameObject effect = pool.Dequeue();
                effect.SetActive(true);

                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }

                return effect;
            }

            Debug.LogWarning($"[NeonPoolerEnhancements] Pool '{key}' is empty. Consider pre-warming more instances.");
            return null;
        }

        /// <summary>
        /// Returns a particle effect to its pool for reuse.
        /// </summary>
        /// <param name="key">The identifier key for the particle pool.</param>
        /// <param name="obj">The GameObject to return to the pool.</param>
        public void ReturnParticleEffect(string key, GameObject obj)
        {
            if (obj == null) return;

            if (!_particleEffectPools.ContainsKey(key))
            {
                Debug.LogWarning($"[NeonPoolerEnhancements] Pool '{key}' does not exist. Creating new pool.");
                _particleEffectPools[key] = new Queue<GameObject>();
            }

            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop();
            }

            obj.SetActive(false);
            _particleEffectPools[key].Enqueue(obj);

            Debug.Log($"[NeonPoolerEnhancements] Returned particle effect to pool '{key}'.");
        }

        /// <summary>
        /// Clears all particle effect pools and destroys pooled objects.
        /// </summary>
        public void ClearAllParticlePools()
        {
            foreach (var poolEntry in _particleEffectPools)
            {
                while (poolEntry.Value.Count > 0)
                {
                    GameObject obj = poolEntry.Value.Dequeue();
                    Destroy(obj);
                }
            }

            _particleEffectPools.Clear();
            Debug.Log("[NeonPoolerEnhancements] All particle pools cleared.");
        }

        /// <summary>
        /// Gets the count of available instances in a specific pool.
        /// </summary>
        /// <param name="key">The identifier key for the particle pool.</param>
        /// <returns>The number of available instances, or -1 if pool doesn't exist.</returns>
        public int GetPoolCount(string key)
        {
            return _particleEffectPools.ContainsKey(key) ? _particleEffectPools[key].Count : -1;
        }

        /// <summary>
        /// Gets all active pool keys in the system.
        /// </summary>
        /// <returns>A list of pool identifier keys.</returns>
        public List<string> GetAllPoolKeys()
        {
            return new List<string>(_particleEffectPools.Keys);
        }
    }
}

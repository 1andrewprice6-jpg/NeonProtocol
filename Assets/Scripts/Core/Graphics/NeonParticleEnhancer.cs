using System.Collections.Generic;
using UnityEngine;

namespace NeonProtocol.Graphics
{
    /// <summary>
    /// Enhances particle systems with glowing neon effects, trails, and visual impacts.
    /// Provides factory methods for creating muzzle flashes, blood spatters, and explosions.
    /// </summary>
    public class NeonParticleEnhancer : MonoBehaviour
    {
        [Header("Particle Settings")]
        [SerializeField] private Color glowColor = new Color(0, 1, 1); // Cyan
        [SerializeField] private float emissionRate = 50f;
        [SerializeField] private float particleLifetime = 2f;
        [SerializeField] private float velocityMultiplier = 3f;

        [Header("Trail Settings")]
        [SerializeField] private bool enableTrails = true;
        [SerializeField] private float trailWidth = 0.1f;
        [SerializeField] private float trailLifetime = 0.5f;

        private static Dictionary<string, Queue<GameObject>> _particlePoolDict = new Dictionary<string, Queue<GameObject>>();

        private static NeonParticleEnhancer _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Enhances an existing ParticleSystem with neon glow and trail effects.
        /// </summary>
        /// <param name="ps">The ParticleSystem to enhance.</param>
        public void EnhanceSystem(ParticleSystem ps)
        {
            if (ps == null) return;

            var mainModule = ps.main;
            mainModule.startColor = glowColor;
            mainModule.startLifetime = particleLifetime;
            mainModule.simulationSpeed = 1f;

            var emissionModule = ps.emission;
            emissionModule.rateOverTime = emissionRate;

            var velocityModule = ps.velocityOverLifetime;
            velocityModule.enabled = true;
            velocityModule.x = new ParticleSystem.MinMaxCurve(-velocityMultiplier, velocityMultiplier);
            velocityModule.y = new ParticleSystem.MinMaxCurve(-velocityMultiplier, velocityMultiplier);
            velocityModule.z = new ParticleSystem.MinMaxCurve(-velocityMultiplier, velocityMultiplier);

            if (enableTrails)
            {
                AddGlowTrails(ps);
            }

            Debug.Log($"[NeonParticleEnhancer] Enhanced particle system: {ps.name}");
        }

        /// <summary>
        /// Adds glowing trails to a ParticleSystem for enhanced visual impact.
        /// </summary>
        /// <param name="ps">The ParticleSystem to add trails to.</param>
        public void AddGlowTrails(ParticleSystem ps)
        {
            var trailModule = ps.trails;
            trailModule.enabled = true;
            trailModule.lifetime = new ParticleSystem.MinMaxCurve(trailLifetime);
            trailModule.minVertexDistance = 0.01f;
            trailModule.inheritParticleColor = true;

            var trailRenderer = ps.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.startWidth = trailWidth;
                trailRenderer.endWidth = 0f;
                trailRenderer.time = trailLifetime;
                trailRenderer.material.SetColor("_Color", glowColor);
            }
        }

        /// <summary>
        /// Creates a muzzle flash effect at the specified position and rotation.
        /// </summary>
        /// <param name="pos">World position of the muzzle flash.</param>
        /// <param name="rot">Rotation of the muzzle flash.</param>
        public void CreateMuzzleFlash(Vector3 pos, Quaternion rot)
        {
            GameObject muzzleFlash = new GameObject("MuzzleFlash_Particle");
            muzzleFlash.transform.SetPositionAndRotation(pos, rot);

            ParticleSystem ps = muzzleFlash.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr = muzzleFlash.GetComponent<ParticleSystemRenderer>();

            var mainModule = ps.main;
            mainModule.startColor = new Color(1, 0.5f, 0); // Orange-yellow
            mainModule.startLifetime = 0.3f;
            mainModule.startSize = 0.2f;
            mainModule.duration = 0.2f;

            var emissionModule = ps.emission;
            emissionModule.rateOverTime = 100f;

            var shapeModule = ps.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Cone;

            psr.material = new Material(Shader.Find("Standard"));
            psr.material.SetColor("_Color", new Color(1, 0.5f, 0));

            Object.Destroy(muzzleFlash, 1f);
        }

        /// <summary>
        /// Creates a blood splatter effect at the specified position.
        /// </summary>
        /// <param name="pos">World position of the splatter.</param>
        public void CreateBloodSplatter(Vector3 pos)
        {
            GameObject splatter = new GameObject("BloodSplatter_Particle");
            splatter.transform.position = pos;

            ParticleSystem ps = splatter.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr = splatter.GetComponent<ParticleSystemRenderer>();

            var mainModule = ps.main;
            mainModule.startColor = new Color(0.8f, 0.1f, 0.1f); // Dark red
            mainModule.startLifetime = 2f;
            mainModule.startSize = 0.3f;
            mainModule.duration = 0.5f;

            var emissionModule = ps.emission;
            emissionModule.rateOverTime = 50f;

            var shapeModule = ps.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Sphere;

            psr.material = new Material(Shader.Find("Standard"));
            psr.material.SetColor("_Color", new Color(0.8f, 0.1f, 0.1f));

            Object.Destroy(splatter, 3f);
        }

        /// <summary>
        /// Creates an explosion effect at the specified position with a given radius.
        /// </summary>
        /// <param name="pos">World position of the explosion center.</param>
        /// <param name="radius">Radius of the explosion effect.</param>
        public void CreateExplosion(Vector3 pos, float radius)
        {
            GameObject explosion = new GameObject("Explosion_Particle");
            explosion.transform.position = pos;

            ParticleSystem ps = explosion.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr = explosion.GetComponent<ParticleSystemRenderer>();

            var mainModule = ps.main;
            mainModule.startColor = new Color(1, 0.7f, 0); // Orange
            mainModule.startLifetime = 1.5f;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.8f);
            mainModule.duration = 0.3f;

            var emissionModule = ps.emission;
            emissionModule.rateOverTime = 150f;

            var velocityModule = ps.velocityOverLifetime;
            velocityModule.enabled = true;
            velocityModule.x = new ParticleSystem.MinMaxCurve(-radius, radius);
            velocityModule.y = new ParticleSystem.MinMaxCurve(-radius, radius);
            velocityModule.z = new ParticleSystem.MinMaxCurve(-radius, radius);

            var shapeModule = ps.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Sphere;
            shapeModule.radius = radius * 0.5f;

            psr.material = new Material(Shader.Find("Standard"));
            psr.material.SetColor("_Color", new Color(1, 0.7f, 0));

            Object.Destroy(explosion, 2.5f);
        }

        /// <summary>
        /// Gets or creates a particle effect from the pool.
        /// </summary>
        /// <param name="key">Identifier for the particle effect pool.</param>
        /// <returns>A GameObject containing the particle system.</returns>
        public static GameObject GetParticleEffect(string key)
        {
            if (_particlePoolDict.ContainsKey(key) && _particlePoolDict[key].Count > 0)
            {
                return _particlePoolDict[key].Dequeue();
            }
            return null;
        }

        /// <summary>
        /// Returns a particle effect to the pool for reuse.
        /// </summary>
        /// <param name="key">Identifier for the particle effect pool.</param>
        /// <param name="obj">The GameObject to return to the pool.</param>
        public static void ReturnParticleEffect(string key, GameObject obj)
        {
            if (!_particlePoolDict.ContainsKey(key))
            {
                _particlePoolDict[key] = new Queue<GameObject>();
            }
            obj.SetActive(false);
            _particlePoolDict[key].Enqueue(obj);
        }

        /// <summary>
        /// Clears all particle pools, freeing up resources.
        /// </summary>
        public static void ClearAllParticlePools()
        {
            foreach (var poolEntry in _particlePoolDict)
            {
                foreach (var particleObj in poolEntry.Value)
                {
                    Object.Destroy(particleObj);
                }
            }
            _particlePoolDict.Clear();
        }
    }
}

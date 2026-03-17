using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NeonProtocol.Core.Input;
using NeonProtocol.Core.Systems;
using NeonProtocol.Core.AI;

namespace NeonProtocol.Core.Combat
{
    public class NeonWeapon : MonoBehaviour
    {
        [Header("Weapon Stats")]
        [SerializeField] private float damage = 20f;
        [SerializeField] private float range = 50f;
        [SerializeField] private float fireRate = 0.1f;
        [SerializeField] private int maxAmmo = 30;
        
        [Header("Visuals")]
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private GameObject hitEffectPrefab;

        private float _nextFireTime;
        private int _currentAmmo;
        private Transform _camTransform;
        private Queue<GameObject> _hitEffectPool;
        private const int HitEffectPoolSize = 10;
        private const float HitEffectLifetime = 2f;

        private void Awake()
        {
            _camTransform = Camera.main.transform;
            _currentAmmo = maxAmmo;

            // Pre-warm hit effect pool to avoid Instantiate spikes during combat
            _hitEffectPool = new Queue<GameObject>(HitEffectPoolSize);
            if (hitEffectPrefab)
            {
                for (int i = 0; i < HitEffectPoolSize; i++)
                {
                    var fx = Instantiate(hitEffectPrefab);
                    fx.SetActive(false);
                    _hitEffectPool.Enqueue(fx);
                }
            }
        }

        private void Update()
        {
            if (NeonInputHandler.Instance.FireInput && Time.time >= _nextFireTime && _currentAmmo > 0)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            _nextFireTime = Time.time + fireRate;
            _currentAmmo--;
            
            if (muzzleFlash) muzzleFlash.Play();

            RaycastHit hit;
            if (Physics.Raycast(_camTransform.position, _camTransform.forward, out hit, range))
            {
                // Damage Logic
                if (hit.collider.TryGetComponent(out ZombieController zombie))
                {
                    zombie.TakeDamage(damage);
                }

                // Pooled hit effect – no allocation on each shot
                if (hitEffectPrefab)
                    SpawnHitEffect(hit.point, hit.normal);
            }
        }

        private void SpawnHitEffect(Vector3 point, Vector3 normal)
        {
            // Overflow objects are also returned to the pool by ReturnHitEffectToPool,
            // so the pool self-regulates to the maximum concurrent in-flight count.
            GameObject fx = _hitEffectPool.Count > 0
                ? _hitEffectPool.Dequeue()
                : Instantiate(hitEffectPrefab);

            fx.transform.SetPositionAndRotation(point, Quaternion.LookRotation(normal));
            fx.SetActive(true);
            StartCoroutine(ReturnHitEffectToPool(fx));
        }

        private IEnumerator ReturnHitEffectToPool(GameObject fx)
        {
            yield return new WaitForSeconds(HitEffectLifetime);
            fx.SetActive(false);
            _hitEffectPool.Enqueue(fx);
        }

        public void Reload()
        {
            _currentAmmo = maxAmmo;
            // IW-style reload logic
        }
    }
}
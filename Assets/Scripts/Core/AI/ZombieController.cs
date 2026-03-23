using UnityEngine;
using UnityEngine.AI;
using NeonProtocol.Core.Systems;
using NeonProtocol.Core.Player;
using NeonProtocol.Core.Economy;

namespace NeonProtocol.Core.AI
{
    public enum ZombieState { Spawning, Chasing, Attacking, Dying }

    [RequireComponent(typeof(NavMeshAgent))]
    public class ZombieController : MonoBehaviour, IPoolable
    {
        [Header("Stats")]
        [SerializeField] private float baseHealth = 100f;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float attackCooldown = 1.5f;

        private NavMeshAgent _agent;
        private Transform _player;
        private ZombieState _state;
        private float _currentHealth;
        private float _nextAttackTime;
        private int _tickOffset;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            // Spread out AI updates across frames to maintain 60FPS
            _tickOffset = Random.Range(0, 10);
        }

        public void OnSpawn()
        {
            float healthMul = HordeManager.Instance != null ? HordeManager.Instance.HealthMultiplier : 1f;
            float speedMul = HordeManager.Instance != null ? HordeManager.Instance.SpeedMultiplier : 1f;
            _currentHealth = baseHealth * healthMul;
            _agent.speed = 3.5f * speedMul;
            _state = ZombieState.Chasing;
            _agent.enabled = true;
        }

        public void OnDespawn()
        {
            _agent.enabled = false;
        }

        private void Update()
        {
            if (_state == ZombieState.Dying || _player == null) return;

            // Optimization: Only update pathfinding every 10 frames
            if ((Time.frameCount + _tickOffset) % 10 == 0)
            {
                _agent.SetDestination(_player.position);
            }

            float dist = Vector3.Distance(transform.position, _player.position);
            if (dist <= attackRange && Time.time >= _nextAttackTime)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _nextAttackTime = Time.time + attackCooldown;
            if (PlayerHealth.Instance != null)
                PlayerHealth.Instance.TakeDamage(damage);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            if (PointsSystem.Instance != null)
                PointsSystem.Instance.AddPoints(10);

            if (_currentHealth <= 0) Die();
        }

        private void Die()
        {
            _state = ZombieState.Dying;
            if (PointsSystem.Instance != null)
                PointsSystem.Instance.AddPoints(60);
            if (HordeManager.Instance != null)
                HordeManager.Instance.OnZombieDeath();
            gameObject.SetActive(false);
        }
    }
}
using Fusion;
using UnityEngine;

namespace GNW2.Player
{
    public class Healthbar : NetworkBehaviour
    {
        public bool spawnDeathFx = true;
        public GameObject player;
        public int maxHealth = 60;
        [Networked] public int _currentHealth { get; set; }
        [SerializeField] private ParticleSystem _hitFx;
        [SerializeField] private ParticleSystem _deathFx;

        private Player _currentPlayer;


        private void Start()
        {
            _currentHealth = maxHealth;
            _currentPlayer = GetComponent<Player>();
            _currentPlayer.OnTakeDamage += TakeHealthDamage;
        }

        public override void FixedUpdateNetwork()
        {
            Debug.Log($"Player: {Runner.LocalPlayer.PlayerId} Health: {_currentHealth}");
            if (_currentHealth < 0)
            {
                RPC_DeathFx(transform.position);
                spawnDeathFx = false;
                //player.SetActive(false);
                //Runner?.Despawn(Object);
            }
        }

        private void TakeHealthDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log($"Current Health: {_currentHealth}");
            RPC_SpawnHitFx(transform.position);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnHitFx(Vector3 position)
        {

            if (_hitFx != null)
            {
                Instantiate(_hitFx, position, Quaternion.identity);
            }
        }
        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_DeathFx(Vector3 position)
        {

            if (_deathFx != null && spawnDeathFx)
            {
                
                Instantiate(_deathFx, position, Quaternion.identity);
                Runner?.Despawn(Object);
            }
        }



    }
}
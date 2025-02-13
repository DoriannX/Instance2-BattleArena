using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Mechanics.Trap
{
    public class ShelfTrap : NetworkBehaviour
    {
        private Collider2D _collider;
        private bool _isTrapActive = true;

        public int DamageAmount = 30;
        public float BlockDuration = 10f;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _isTrapActive && IsServer)
            {
                int chance = Random.Range(1, 4); 

                if (chance == 1) 
                {
                    ActivateTrap(other.gameObject);
                }
            }
        }

        private void ActivateTrap(GameObject player)
        {
            PlayerStats.PlayerStats playerStats = player.GetComponent<PlayerStats.PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(40);
            }

            _isTrapActive = false;
            _collider.isTrigger = false; 

            StartCoroutine(ResetTrapAfterDelay());
        }

        private IEnumerator ResetTrapAfterDelay()
        {
            yield return new WaitForSeconds(BlockDuration);
            _isTrapActive = true;
            _collider.isTrigger = true; 
        }
    }
}

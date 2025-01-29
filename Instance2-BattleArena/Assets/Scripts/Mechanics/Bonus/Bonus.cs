using UnityEngine;

public class Bonus : MonoBehaviour
{
    private BonusEffects _bonusEffect;  
    private bool _isCollected = false; 

    private void Start()
    {
        _bonusEffect = GetComponent<BonusEffects>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isCollected && other.CompareTag("Player"))  
        {
            _isCollected = true;  
            if (_bonusEffect != null)
            {
                _bonusEffect.ApplyEffect(other.gameObject);  
            }
            RandomSpawner.Instance?.RespawnBonus(gameObject); 
            Destroy(gameObject);
        }
    }
    public void Respawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;  
        gameObject.SetActive(true);  
        _isCollected = false;  
    }
}

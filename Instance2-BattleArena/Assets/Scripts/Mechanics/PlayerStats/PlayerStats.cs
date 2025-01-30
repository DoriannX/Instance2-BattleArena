using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int _attack; 
    public int _speed; 
    public void SetStats(int attack, int speed)
    {
        _attack = attack;
        _speed = speed;
    }
}

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Attack; 
    public int Speed; 
    public void SetStats(int attack, int speed)
    {
        Attack = attack;
        Speed = speed;
    }
}

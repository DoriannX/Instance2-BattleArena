using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Attack; 
    public int Speed; 
    public void SetStats(int newAttack, int newSpeed)
    {
        Attack = newAttack;
        Speed = newSpeed;
    }
}

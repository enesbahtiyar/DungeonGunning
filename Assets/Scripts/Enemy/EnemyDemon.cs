using UnityEngine;
using UnityEngine.UIElements;

public class EnemyDemon : EnemyBase
{
    private void Start()
    {
        coinDropChance = 0.5f;
        coinValue = 15;
    }
}

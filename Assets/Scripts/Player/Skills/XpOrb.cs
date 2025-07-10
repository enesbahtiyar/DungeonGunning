using UnityEngine;

public class XpOrb : MonoBehaviour
{
    public int xpAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.GainXp(xpAmount);
            Destroy(gameObject);
        }
    }
}

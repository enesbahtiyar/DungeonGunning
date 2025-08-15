using UnityEngine;
using System.Collections;

public class BulletOrb : MonoBehaviour
{
    public int bulletAmount = 5;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OsmanAttack attack = collision.GetComponent<OsmanAttack>();
            if (attack == null)
            {
                attack = collision.GetComponentInParent<OsmanAttack>();
            }
            if (attack != null && attack.weapons.Count > attack.activeWeaponIndex)
            {
                var weapon = attack.weapons[attack.activeWeaponIndex];
                int maxAmmo = weapon.magazineSize;
                int current = weapon.currentAmmo;
                int toAdd = Mathf.Min(bulletAmount, maxAmmo - current);
                if (toAdd > 0)
                {
                    weapon.currentAmmo += toAdd;
                    weapon.totalAmmo += toAdd;
                }
            }
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                StartCoroutine(DestroyAfterSound());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}

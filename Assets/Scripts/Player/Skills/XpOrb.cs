using UnityEngine;
using System.Collections;

public class XpOrb : MonoBehaviour
{
    public int xpAmount;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.GainXp(xpAmount);
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.volume = PlayerPrefs.GetFloat("Volume_Sounds", 1f);
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

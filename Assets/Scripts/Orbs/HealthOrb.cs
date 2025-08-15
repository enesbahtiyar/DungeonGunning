using UnityEngine;
using System.Collections;

public class HealthOrb : MonoBehaviour
{
    public float healthValue = 10f; 
    public AudioSource audioSource;
    public ParticleSystem particleEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OsmanHealth playerHealth = collision.GetComponent<OsmanHealth>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseHealth(healthValue);
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    particleEffect.Play();
                    StartCoroutine(DestroyAfterSound());
                }
            }
           
        }
    }
    
    private IEnumerator DestroyAfterSound()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {            
            GetComponent<SpriteRenderer>().enabled = false;
        }

        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
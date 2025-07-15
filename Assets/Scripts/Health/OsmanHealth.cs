using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OsmanHealth : MonoBehaviour
{
    public EntityType entityType;
    public float health;
    public float maxHealth;

    public AudioClip damageSound;
    public AudioClip deathSound;

    public Image healthImage;
    public TextMeshProUGUI healthText;

    private EnemyBase enemyBase;
    public GameObject DiePanel;
    public Button closebutton, restartButton;
    void Start()
    {
        maxHealth = PlayerStats.Instance.maxHealth.Value;
        health = maxHealth;
        enemyBase = GetComponent<EnemyBase>();
        if (entityType == EntityType.Player && DiePanel != null)
        {
            DiePanel.SetActive(false);
            if (closebutton != null)
                closebutton.onClick.AddListener(CloseDiePanel);
            if (restartButton != null)
                restartButton.onClick.AddListener(PlayAgain);
        }
    }
    public void CloseDiePanel()
    {
        if (entityType == EntityType.Player && DiePanel != null)
        {
            DiePanel.SetActive(false);
        }
    }
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
        if (healthText != null)
            healthText.text = $"{health} / {maxHealth}";
    }

    public void IncreaseHealth(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
    }

    public void DecreaseHealth(float amount)
    {
        //hasar aldığı zaman  hasar sesi çalma işlemi 
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }

        health = Mathf.Max(health - amount, 0);
        if (health <= 0)
        {
            OnDeath();
        }
        else
        {
            if (enemyBase != null)
            {
                enemyBase.TakeDamage();
            }
        }
    }

    void OnDeath()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        if (entityType == EntityType.AI)
        {
            //Destroy(gameObject);
            enemyBase.ChangeEnemyState(EnemyState.die);
            //isterseniz enemy ölünce burada daha farklı yapılacakları yazabiliriz animasyon hariç.
        }
        else if (entityType == EntityType.Player)
        {
            if (DiePanel != null)
            {
                DiePanel.SetActive(true);
            }
            // Player ölünce yapılacak işlemler
            Debug.Log("Player has died.");
        }
    }
}
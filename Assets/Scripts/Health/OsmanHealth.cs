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
    //public TextMeshProUGUI healthText;

    private EnemyBase enemyBase;
    public GameObject DiePanel;
    public Button closebutton, restartButton;
    public Animator animator;
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
        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
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

    public void IncreaseHealth(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        //hasar aldığı zaman  hasar sesi çalma işlemi 
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }

        health = Mathf.Max(health - amount, 0);
        PopupSpawner.Instance.ShowPopup(transform.position, amount.ToString(), Color.red);

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
        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
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
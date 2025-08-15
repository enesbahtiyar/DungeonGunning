using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public string sceneName;
    
    [Header("Enemy Health Increase Settings")]
    public float healthIncreaseAmount = 10f;
    public float healthIncreaseInterval = 30f;
    
    private TimerDisplay timerDisplay;
    private float lastHealthIncreaseTime = 0f;
    private bool hasStartedHealthIncrease = false;
    
    void Start()
    {
        if ( entityType == EntityType.Player)
        {
            maxHealth = PlayerStats.Instance.maxHealth.Value;
        }
        
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
            
        if (entityType == EntityType.AI)
        {
            timerDisplay = FindObjectOfType<TimerDisplay>();
        }
    }
    
    void Update()
    {
        if (entityType == EntityType.AI && timerDisplay != null && gameObject.activeInHierarchy)
        {
            float currentTime = timerDisplay.GetElapsedTime();
            
            if (!hasStartedHealthIncrease && currentTime > 0)
            {
                hasStartedHealthIncrease = true;
                lastHealthIncreaseTime = currentTime;
            }
            
            if (hasStartedHealthIncrease && currentTime - lastHealthIncreaseTime >= healthIncreaseInterval)
            {
                IncreaseMaxHealthOverTime();
                lastHealthIncreaseTime = currentTime;
            }
        }
    }
    
    private void IncreaseMaxHealthOverTime()
    {
        maxHealth += healthIncreaseAmount;
        health += healthIncreaseAmount;
        
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
        StartCoroutine(ReloadSceneAfterDelay());
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
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
            GameManager.Instance.SetState(GameState.GameOver);
            //Eğer oyuncu öldüyse burada bir event çalışsın bu event mesela oyunun olduğu state'i değiştirsin oyunun state'i game over olduğu zaman bi kodda kontroller yapılsın bu kontroller işte silahı
            //kapat düşmanları dondur oyuncuyu dondur gibi gibi 
            if (DiePanel != null)
            {
                DiePanel.SetActive(true);
            }
            // Player ölünce yapılacak işlemler
            Debug.Log("Player has died.");
        }
    }
}
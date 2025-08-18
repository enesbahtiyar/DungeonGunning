using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OsmanMusicController : MonoBehaviour
{
    [Header("Ses Ayarları")]
    public AudioClip clickSound;
    public AudioSource audioSource;

    [Header("Takip Edilecek Paneller")]
    public List<GameObject> panels = new List<GameObject>();
    private Dictionary<GameObject, bool> panelStates = new Dictionary<GameObject, bool>();

    [Header("Pause Panel ve Sesler")]
    public GameObject pausePanel;
    public AudioSource backgroundMusic;
    public Slider backgroundSlider;
    public Slider soundsSlider;
    public List<AudioSource> sounds = new List<AudioSource>();

    const string PREF_BG = "Volume_Background";
    const string PREF_SFX = "Volume_Sounds";

    void Awake()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        foreach (var panel in panels)
        {
            if (panel != null)
                panelStates[panel] = panel.activeSelf;
        }
    }

    void Start()
    {
        if (backgroundSlider != null)
            backgroundSlider.onValueChanged.AddListener(SetBackgroundVolume);
        if (soundsSlider != null)
            soundsSlider.onValueChanged.AddListener(SetSoundsVolume);

        RefreshSoundsList(); 
        LoadVolumes();      
    }

    void Update()
    {
        foreach (var panel in panels)
        {
            if (panel == null) continue;

            bool prevState = panelStates[panel];
            bool currState = panel.activeSelf;

            if (!prevState && currState)
                AddListenersToPanel(panel);

            panelStates[panel] = currState;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pausePanel != null)
        {
            bool isActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isActive);

            if (!isActive)
            {
                RefreshSoundsList(); 
                LoadVolumes();      
                GameManager.Instance.SetState(GameState.Paused);
            }
            else
            {
                GameManager.Instance.SetState(GameState.Playing);
            }
        }
    }

    void AddListenersToPanel(GameObject panel)
    {
        Button[] panelButtons = panel.GetComponentsInChildren<Button>(true);
        foreach (Button btn in panelButtons)
        {
            btn.onClick.RemoveListener(() => PlayClickSound(btn));
            btn.onClick.AddListener(() => PlayClickSound(btn));
        }
    }

    public void PlayClickSound(Button clickedButton)
    {
        if (clickSound != null && audioSource != null)
        {
            float volume = soundsSlider != null ? soundsSlider.value : 1f;
            audioSource.PlayOneShot(clickSound, volume);
        }
    }

    void RefreshSoundsList()
    {
        sounds.Clear();
        AudioSource[] allSources = FindObjectsOfType<AudioSource>(true);
        foreach (var src in allSources)
        {
            if (src != null && src != backgroundMusic && src != audioSource)
                sounds.Add(src);
        }
    }

    void SetBackgroundVolume(float value)
    {
        if (backgroundMusic != null)
            backgroundMusic.volume = value;

        PlayerPrefs.SetFloat(PREF_BG, value);
        PlayerPrefs.Save();
    }

    void SetSoundsVolume(float value)
    {
        foreach (var src in sounds)
        {
            if (src != null)
                src.volume = value;
        }

        PlayerPrefs.SetFloat(PREF_SFX, value);
        PlayerPrefs.Save();
    }

    void LoadVolumes()
    {
        float bgVolume = PlayerPrefs.HasKey(PREF_BG) ? PlayerPrefs.GetFloat(PREF_BG) : 1f;
        float sfxVolume = PlayerPrefs.HasKey(PREF_SFX) ? PlayerPrefs.GetFloat(PREF_SFX) : 1f;

        if (backgroundMusic != null)
            backgroundMusic.volume = bgVolume;

        foreach (var src in sounds)
        {
            if (src != null)
                src.volume = sfxVolume;
        }

        if (backgroundSlider != null)
            backgroundSlider.value = bgVolume;
        if (soundsSlider != null)
            soundsSlider.value = sfxVolume;
    }
}

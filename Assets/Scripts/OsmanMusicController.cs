using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OsmanMusicController : MonoBehaviour
{
    [Header("Ses Ayarları")]
    public AudioClip clickSound;
    public AudioSource audioSource;

    [Header("Takip Edilecek Paneller")]
    public List<GameObject> panels = new List<GameObject>();

    private Dictionary<GameObject, bool> panelStates = new Dictionary<GameObject, bool>();

    void Awake()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D ses

        // Başlangıç durumlarını kaydet
        foreach (var panel in panels)
        {
            if (panel != null)
                panelStates[panel] = panel.activeSelf;
        }
    }

    void Update()
    {
        foreach (var panel in panels)
        {
            if (panel == null) continue;

            bool previousState = panelStates[panel];
            bool currentState = panel.activeSelf;

            if (!previousState && currentState)
            {
                Debug.Log($"[OsmanMusicController] Panel açıldı → {panel.name}");
                AddListenersToPanel(panel);
            }

            panelStates[panel] = currentState;
        }
    }

    void AddListenersToPanel(GameObject panel)
    {
        Button[] panelButtons = panel.GetComponentsInChildren<Button>(true);

        foreach (Button btn in panelButtons)
        {
            btn.onClick.RemoveListener(() => PlayClickSound(btn));
            btn.onClick.AddListener(() => PlayClickSound(btn));
            Debug.Log($"[OsmanMusicController] Listener eklendi → {btn.name} (Panel: {panel.name})");
        }
    }

    void PlayClickSound(Button clickedButton)
    {
        Debug.Log($"[OsmanMusicController] '{clickedButton.name}' tıklandı, ses çalınıyor...");
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("[OsmanMusicController] Click sound atanmadı!");
        }
    }
}

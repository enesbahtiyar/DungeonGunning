using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;     
    [SerializeField] private float fadeDuration = 1f;  

    private TextMeshPro textMesh;
    private float timer;
    private Color originalColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
        timer = fadeDuration;
    }

    public void Setup(string message, Color color, float fontSize = 3f)
    {
        textMesh.text = message;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        timer = fadeDuration;
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        timer -= Time.deltaTime;
        Color c = textMesh.color;
        c.a = Mathf.Clamp01(timer / fadeDuration);
        textMesh.color = c;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

}

using UnityEngine;

public class PopupSpawner : SingletonMonoBehaviour<PopupSpawner>
{
    [SerializeField] private GameObject popupTextPrefab;

    public void ShowPopup(Vector3 worldPosition, string message, Color color, float fontSize = 3f)
    {
        GameObject popup = Instantiate(popupTextPrefab, worldPosition, Quaternion.identity);
        TextPopup textPopup = popup.GetComponent<TextPopup>();
        textPopup.Setup(message, color, fontSize);
    }
}

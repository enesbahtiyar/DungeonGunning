using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button button;
    public void SetCard(UpgradeCard card, System.Action onClick)
    {
        icon.sprite = card.icon;
        nameText.text = card.name;
        descriptionText.text = card.cardDescription;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick.Invoke());
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);    
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button buyButton;

    public void SetItem(ShopItem item,System.Action onClick)
    {
        icon.sprite = item.icon;
        nameText.text = item.name;
        costText.text=item.cost.ToString();
        descriptionText.text = item.description.ToString();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onClick.Invoke());
        gameObject.SetActive(true);
    }
    
}

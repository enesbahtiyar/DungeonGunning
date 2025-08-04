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
    [SerializeField] private Button damageUpgaredeButton;
    [SerializeField] private Button fireRateUpgaredeButton;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject upgradePanel;

    public void SetItem(ShopItem item, System.Action onClick, bool isBought, System.Action onClickDmgUpgrade, System.Action onClickFireRateUpgrade)
    {
        icon.sprite = item.icon;
        nameText.text = item.name;
        costText.text = item.cost.ToString();
        descriptionText.text = item.description.ToString();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onClick.Invoke());
        damageUpgaredeButton.onClick.RemoveAllListeners();
        damageUpgaredeButton.onClick.AddListener(() => onClickDmgUpgrade.Invoke());
        fireRateUpgaredeButton.onClick.RemoveAllListeners();
        fireRateUpgaredeButton.onClick.AddListener(() => onClickFireRateUpgrade.Invoke());
        gameObject.SetActive(true);
        BuyStatus(isBought);
    }
    public void BuyStatus(bool isBought)
    {
        if (isBought)
        {
            buyPanel.SetActive(false);
            upgradePanel.SetActive(true);
        }
        else
        {
            buyPanel.SetActive(true);
            upgradePanel.SetActive(false);
        }
    }
}

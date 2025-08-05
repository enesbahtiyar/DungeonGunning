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
    [SerializeField] private TextMeshProUGUI damageUpgradeCost;
    [SerializeField] private TextMeshProUGUI fireRateUpgradeCost;
    [SerializeField] private TextMeshProUGUI damageUpgradeLevelText;
    [SerializeField] private TextMeshProUGUI fireRateUpgradeLevelText;
    public int damageUpgradeLevel = 1;
    public int fireRateUpgradeLevel = 1;
    private int damageUpgradeCostValue = 100;
    private int fireRateUpgradeCostValue = 100;
    public void SetItem(ShopItem item, System.Action onClick, bool isBought, System.Action onClickDmgUpgrade, System.Action onClickFireRateUpgrade,int _damageUpgradeCostValue,int _fireRateUpgradeCostValue)
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
        damageUpgradeCostValue= _damageUpgradeCostValue;
        fireRateUpgradeCostValue = _fireRateUpgradeCostValue;

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
    public void UpgradeDamage()
    {
        if (PlayerStats.Instance.coinCount < damageUpgradeCostValue)
        {
            Debug.LogWarning("Not Enough Coins for Damage Upgrade");
            return;
        }
        PlayerStats.Instance.AddCoins(-damageUpgradeCostValue);
        damageUpgradeCostValue = (int)(damageUpgradeCostValue * 1.2f);


        damageUpgradeLevel++;
        damageUpgradeLevelText.text = "Level: " + damageUpgradeLevel;
        damageUpgradeCost.text = damageUpgradeCostValue.ToString();
    }
    public void UpgradeFireRate()
    {
        if (PlayerStats.Instance.coinCount < fireRateUpgradeCostValue)
        {
            Debug.LogWarning("Not Enough Coins for Fire Rate Upgrade");
            return;
        }
        PlayerStats.Instance.AddCoins(-fireRateUpgradeCostValue);
        fireRateUpgradeCostValue = (int)(fireRateUpgradeCostValue * 1.2f);

        fireRateUpgradeLevel++;
        fireRateUpgradeLevelText.text = "Level: " + fireRateUpgradeLevel;
        fireRateUpgradeCost.text = fireRateUpgradeCostValue.ToString();
    }
}
